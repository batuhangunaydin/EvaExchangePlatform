using EvaExchangePlatform.Model.ExchangePlatform;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaExchangePlatform.Repository.Context
{
    public class ExchangeContext : DbContext
    {
        public DbSet<Traders> Traders { get; set; }
        public DbSet<RegisteredShares> RegisteredShares { get; set; }
        public DbSet<TradersPortfolios> TradersPortfolios { get; set; }
        public DbSet<TransactionLogs> TransactionLogs { get; set; }
        public ExchangeContext(DbContextOptions<ExchangeContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Traders>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Firstname).IsRequired().HasMaxLength(35);
                entity.Property(e => e.Lastname).IsRequired().HasMaxLength(35);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(55); 
                entity.Property(e => e.Password).IsRequired().HasMaxLength(64);
                entity.Property(e => e.Balance).IsRequired().HasDefaultValue(0.00);
                entity.Property(e => e.BlockedBalance).IsRequired().HasDefaultValue(0.00);
                entity.Property(e => e.RegisteredDate).IsRequired().HasDefaultValueSql("current_timestamp");
            });

            builder.Entity<RegisteredShares>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.traderId).IsRequired();
                entity.Property(e => e.ShareName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ShareCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.RegisteredAmount).IsRequired();
                entity.Property(e => e.SharePrice).IsRequired();
                entity.Property(e => e.TradeSide).IsRequired().HasMaxLength(10);
                entity.Property(e => e.LastUpdateDate).IsRequired().HasDefaultValueSql("current_timestamp");
            });

            builder.Entity<TradersPortfolios>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.traderId).IsRequired();
                entity.Property(e => e.ShareName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ShareCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.ShareAmount).IsRequired().HasDefaultValue(0.00);
                entity.Property(e => e.ShareBlockedAmount).IsRequired().HasDefaultValue(0.00);
            });

            builder.Entity<TransactionLogs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.traderId).IsRequired();
                entity.Property(e => e.ShareCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.ShareAmount).IsRequired();
                entity.Property(e => e.SharePrice).IsRequired();
                entity.Property(e => e.TradeSide).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(30);
                entity.Property(e => e.TransactionDate).IsRequired().HasDefaultValueSql("current_timestamp");
            });
        }
    }
}


