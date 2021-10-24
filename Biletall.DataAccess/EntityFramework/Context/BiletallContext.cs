using Biletall.Core.Constants;
using Biletall.DataAccess.EntityFramework.Configurations.Concrete;
using Biletall.DataAccess.EntityFramework.Configurations.Identity;
using Biletall.Entities.Concrete;
using Biletall.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;


namespace Biletall.DataAccess.EntityFramework.Context
{
    public class BiletallContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public BiletallContext(DbContextOptions<BiletallContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration;

            if (Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ASPNETCORE_ENVIRONMENT) == null)
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
            else
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ASPNETCORE_ENVIRONMENT)}.json")
                    .Build();
            }
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BiletallConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(60));
        }

        #region ENTITIES

        public virtual DbSet<Reservation> Reservations { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity Configurations

            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());

            #endregion

            #region Entity Configurations

            builder.ApplyConfiguration(new ReservationConfiguration());

            #endregion
        }
    }
}
