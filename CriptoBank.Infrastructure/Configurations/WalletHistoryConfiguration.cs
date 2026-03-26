
using CriptoBank.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Configurations
{
    public class WalletHistoryConfiguration : IEntityTypeConfiguration<WalletHistory>
    {
        public void Configure(EntityTypeBuilder<WalletHistory> builder)
        {
            builder.ToTable("WalletHistories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.HasOne<Wallet>()
                   .WithMany() 
                   .HasForeignKey(x => x.WalletId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
