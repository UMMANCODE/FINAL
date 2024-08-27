using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Final_Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options) {
  public DbSet<Comment> Comments { get; set; }
  public DbSet<Discount> Discounts { get; set; }
  public DbSet<Bid> Bids { get; set; }
  public DbSet<Slider> Sliders { get; set; }
  public DbSet<Feature> Features { get; set; }
  public DbSet<House> Houses { get; set; }
  public DbSet<HouseImage> HouseImages { get; set; }
  public DbSet<HouseFeature> HouseFeatures { get; set; }
  public DbSet<Order> Orders { get; set; }

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);
    builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
  }
}