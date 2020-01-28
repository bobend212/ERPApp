using ERPApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPApp.Data
{
    public class AppDBContext : IdentityDbContext<IdentityUser>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        //Creating roles
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                    new {Id = "1", Name = "Admin", NormalizedName = "ADMIN"},
                    new { Id = "2", Name = "User", NormalizedName = "USER" },
                    new { Id = "3", Name = "Leader", NormalizedName = "LEADER" },
                    new { Id = "4", Name = "Guest", NormalizedName = "GUEST" }
                );
        }

        public DbSet<ProjectModel> Projects{ get; set; }

    }
}
