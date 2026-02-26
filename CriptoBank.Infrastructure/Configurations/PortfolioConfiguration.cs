using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CriptoBank.Domain.Models;

namespace CriptoBank.Infrastructure.Configurations;

public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.ToTable("Portfolios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasMany(x => x.Transactions)
            .WithOne(x => x.Portfolio)
            .HasForeignKey(x => x.PortfolioId);

        builder.HasMany(x => x.Holdings)
            .WithOne(x => x.Portfolio)
            .HasForeignKey(x => x.PortfolioId);
    }
}