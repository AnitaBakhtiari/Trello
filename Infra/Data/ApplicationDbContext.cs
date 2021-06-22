using System;
using System.Collections.Generic;
using System.Text;
using Infra.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> AppplicationRoles { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Category> Categories { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>().HasData(new ApplicationRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id= "8699e184-5e8f-4390-b158-6a2bcd2b90c6",
                ConcurrencyStamp= "fece92c4-2918-45d7-9155-93af67ff14d0"


            });



        }
    }
}
