using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class AppUserConfiguration : IEntityTypeConfiguration<AppUser> {
  public void Configure(EntityTypeBuilder<AppUser> builder) {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
    builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);
    builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);
  }
}
