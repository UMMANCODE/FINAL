namespace Final_Data.Configurations;
public class FeatureConfiguration : IEntityTypeConfiguration<Feature> {
  public void Configure(EntityTypeBuilder<Feature> builder) {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
    builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
    builder.Property(x => x.IconLink).IsRequired();
  }
}
