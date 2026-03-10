using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CriptoBank.Domain.Models;

namespace CriptoBank.Infrastructure.Configurations;

public class HoldingConfiguration : IEntityTypeConfiguration<Holding>
{
    public void Configure(EntityTypeBuilder<Holding> builder)
    {
        builder.ToTable("Holdings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .HasColumnType("decimal(18,8)")
            .IsRequired();

        builder.Property(x => x.AveragePrice)
            .HasColumnType("decimal(18,8)")
            .IsRequired();

        builder.HasIndex(x => new { x.PortfolioId, x.CryptoId })
            .IsUnique();
    }
}