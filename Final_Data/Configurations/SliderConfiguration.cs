namespace Final_Data.Configurations;
public class SliderConfiguration : IEntityTypeConfiguration<Slider> {
  public void Configure(EntityTypeBuilder<Slider> builder) {
    builder.Property(s => s.Title).HasMaxLength(50).IsRequired();
    builder.Property(s => s.SubTitle1).HasMaxLength(100).IsRequired();
    builder.Property(s => s.SubTitle2).HasMaxLength(100).IsRequired();
    builder.Property(s => s.BtnText1).HasMaxLength(20);
    builder.Property(s => s.BtnText2).HasMaxLength(20);
    builder.Property(s => s.Description).HasMaxLength(500).IsRequired();
    builder.Property(s => s.ImageLink).IsRequired();
    builder.Property(s => s.Order).IsRequired();
  }
}
