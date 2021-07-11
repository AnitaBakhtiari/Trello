using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.AuthoMapper;
using Application.CommandHandlers;
using Application.Hubs;
using Application.Workers;
using Infra.Data;
using Infra.Models;
using Infra.Repositories;
using Infra.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Trello.Hubs;

namespace Trello
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                  .AddJwtBearer(cfg =>
                  {
                      cfg.RequireHttpsMetadata = false;
                      cfg.SaveToken = true;

                      cfg.TokenValidationParameters = new TokenValidationParameters()
                      {
                          ValidateIssuerSigningKey = true,
                          ValidateIssuer = true,
                          ValidateAudience = false,
                          // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                          ClockSkew = TimeSpan.Zero,
                          ValidIssuer = Configuration["JWT:Issuer"],
                          ValidAudience = Configuration["JWT:Issuer"],
                          IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                      };

                      cfg.Events = new JwtBearerEvents
                      {
                          OnMessageReceived = context =>
                          {

                              var accessToken = context.Request.Query["access_token"];

                              // If the request is for our hub...
                              var path = context.HttpContext.Request.Path;
                              if (!string.IsNullOrEmpty(accessToken) &&
                                  (path.StartsWithSegments("/NotificationHub")))
                              {
                                  // Read the token out of the query string
                                  context.Token = accessToken;
                              }

                              return Task.CompletedTask;
                          }
                      };
                  }
                  );


            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 1;
                o.Password.RequiredUniqueChars = 0;
                o.Password.RequireLowercase = false;
                o.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<ApplicationUser>)));
            })
                //.AddDefaultUI(UIFramework.Bootstrap4)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSignalR();
            services.AddScoped<INotificationHub, NotificationHub>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllersWithViews();

            services.AddAutomapper();

            //MediatR
            services.AddMediatR(new[]
            {
                Assembly.GetAssembly(typeof(RegisterCommandHandler)),
            });
            var handlers = typeof(RegisterCommandHandler).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                );
            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)), handler);
            }

            services.AddScoped<IJwtService, JwtService>();


            services.AddHostedService<RejectWorker>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Trello", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trello v1"));

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/NotificationHub");

            });

          

        }
    }
}
