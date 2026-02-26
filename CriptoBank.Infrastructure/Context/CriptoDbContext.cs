

using CriptoBank.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Context
{
    public class CriptoDbContext : DbContext
    {
        public CriptoDbContext(DbContextOptions<CriptoDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Crypto> Cryptos { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Holding> Holdings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CriptoDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
