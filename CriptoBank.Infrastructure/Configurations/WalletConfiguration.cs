

using CriptoBank.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CriptoBank.Infrastructure.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Balance)
                   .HasPrecision(18, 2)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.HasOne(x => x.User)
                       .WithOne()
                       .HasForeignKey<Wallet>(x => x.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
