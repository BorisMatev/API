using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    internal class AppDbContext : DbContext
    {

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"
                Server=(localdb)\MSSQLLocalDB;
                Database=BonFire;
                TrustServerCertificate=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            #region User

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "adminpass",
                    Profile_Photo = "",
                    Email = "admin@admin.com"
                });

            #endregion

            #region Movie

            modelBuilder.Entity<Movie>()
                .HasKey(u => u.Id);

            #endregion
        }
    }
}
