using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.AuthoMapper
{

    public static class AutoMapperExtention
    {
        public static IServiceCollection AddAutomapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AuthoMapperProfile()));
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }

}
