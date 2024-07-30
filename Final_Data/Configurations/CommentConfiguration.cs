using Final_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Final_Data.Configurations;
public class CommentConfiguration : IEntityTypeConfiguration<Comment> {
  public void Configure(EntityTypeBuilder<Comment> builder) {
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Content).IsRequired();
    builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
    builder.HasOne(x => x.AppUser).WithMany(x => x.Comments).HasForeignKey(x => x.AppUserId);
    builder.HasOne(x => x.House).WithMany(x => x.Comments).HasForeignKey(x => x.HouseId);
    builder.Property(x => x.Content).HasMaxLength(200);
  }
}
