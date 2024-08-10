using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class DiscountConfiguration : IEntityTypeConfiguration<Discount> {
  public void Configure(EntityTypeBuilder<Discount> builder) {
    builder.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
    builder.Property(x => x.ExpiryDate).IsRequired();
    builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
  }
}
