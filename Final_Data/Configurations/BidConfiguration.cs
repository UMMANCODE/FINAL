using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class BidConfiguration : IEntityTypeConfiguration<Bid> {
  public void Configure(EntityTypeBuilder<Bid> builder) {
    builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
    builder.Property(x => x.CreatedAt).IsRequired();
  }
}
