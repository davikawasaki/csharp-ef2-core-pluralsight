﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext() { }
        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options) { }

        public static readonly LoggerFactory MyConsoleLoggerFactory = new LoggerFactory(new[]
        {
            new ConsoleLoggerProvider((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information, true)  // Adding filters such as SQL commands and only basic level info
        });

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // For WPF applications
            //var connectionString = ConfigurationManager.ConnectionStrings["WPFDatabase"].ToString();
            //optionsBuilder
            //    .UseLoggerFactory(MyConsoleLoggerFactory)
            //    .EnableSensitiveDataLogging(true)
            //    .UseSqlServer(connectionString);

            // For normal console applications
            optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .EnableSensitiveDataLogging(true)
                .UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = SamuraiAppData; Trusted_Connection = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });
        }
    }
}
