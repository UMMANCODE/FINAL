namespace Final_Data.Configurations;
public class HouseImageConfiguration : IEntityTypeConfiguration<HouseImage> {
  public void Configure(EntityTypeBuilder<HouseImage> builder) {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.ImageLink).IsRequired();
    builder.HasOne(x => x.House).WithMany(x => x.HouseImages).HasForeignKey(x => x.HouseId);
  }
}
