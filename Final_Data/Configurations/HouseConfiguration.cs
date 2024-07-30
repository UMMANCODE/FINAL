using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class HouseConfiguration : IEntityTypeConfiguration<House> {
  public void Configure(EntityTypeBuilder<House> builder) {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
    builder.Property(x => x.Location).IsRequired().HasMaxLength(100);
    builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
    builder.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
  }
}
