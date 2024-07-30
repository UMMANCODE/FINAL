using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class HouseImageConfiguration : IEntityTypeConfiguration<HouseImage> {
  public void Configure(EntityTypeBuilder<HouseImage> builder) {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.ImageLink).IsRequired();
    builder.HasOne(x => x.House).WithMany(x => x.Images).HasForeignKey(x => x.HouseId);
  }
}
