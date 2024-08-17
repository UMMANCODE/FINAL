using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order> {
  public void Configure(EntityTypeBuilder<Order> builder) {
    builder.Property(x => x.CreatedAt).IsRequired();
    builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
    builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
    builder.HasOne(x => x.AppUser).WithMany(x => x.Orders).HasForeignKey(x => x.AppUserId);
    builder.HasOne(x => x.House).WithMany(x => x.Orders).HasForeignKey(x => x.HouseId);
  }
}
