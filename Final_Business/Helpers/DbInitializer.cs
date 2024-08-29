using System.Text.Json;
using Final_Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Final_Business.Helpers;

public static class DbInitializer {
  public static async Task SeedData(IServiceProvider serviceProvider) {
    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
    var context = serviceProvider.GetRequiredService<AppDbContext>();


    // Seed roles
    if (!await roleManager.RoleExistsAsync("Admin")) {
      await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("Member")) {
      await roleManager.CreateAsync(new IdentityRole("Member"));
    }
    if (!await roleManager.RoleExistsAsync("SuperAdmin")) {
      await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
    }

    // Seed default users
    var admin = new AppUser {
      Id = "4ba6d714-eb0b-41a6-9a58-bdb3977694e8",
      FullName = "Admin",
      UserName = "admin",
      Email = "admin@quarter.est.com",
      AvatarLink = "admin.jpg",
      Nationality = "AZ",
      EmailConfirmed = true
    };
    var member = new AppUser {
      Id = "f64d8b52-6bc8-4bcf-92be-d0fa2ac38c56",
      FullName = "Member",
      UserName = "member",
      Email = "member@quarter.est.com",
      AvatarLink = "member.png",
      Nationality = "AZ",
      EmailConfirmed = true
    };
    var superAdmin = new AppUser {
      Id = "538160f4-a16b-402f-922d-80e3c657d07f",
      FullName = "SuperAdmin",
      UserName = "superadmin",
      Email = "superadmin@quarter.est.com",
      AvatarLink = "superadmin.jpg",
      Nationality = "AZ",
      EmailConfirmed = true
    };

    if (userManager.Users.All(u => u.UserName != admin.UserName)) {
      await userManager.CreateAsync(admin, "Admin123");
      await userManager.AddToRoleAsync(admin, "Admin");
    }
    if (userManager.Users.All(u => u.UserName != member.UserName)) {
      await userManager.CreateAsync(member, "Member123");
      await userManager.AddToRoleAsync(member, "Member");
    }
    if (userManager.Users.All(u => u.UserName != superAdmin.UserName)) {
      await userManager.CreateAsync(superAdmin, "SuperAdmin123");
      await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
    }

    // Seed users
    var users = await ReadData<AppUser>(env, "users.json");
    foreach (var user in users.Where(user => userManager.Users.All(u => u.UserName != user.UserName))) {
      await userManager.CreateAsync(user, $"{user.UserName}123");
      await userManager.AddToRoleAsync(user, "Member");
    }

    // Seed dev data
    var features = await ReadData<Feature>(env, "features.json");
    var sliders = await ReadData<Slider>(env, "sliders.json");
    var houseImages = await ReadData<HouseImage>(env, "houseImages.json");
    var houses = await ReadData<House>(env, "houses.json");
    var comments = await ReadData<Comment>(env, "comments.json");
    var orders = await ReadData<Order>(env, "orders.json");
    var bids = await ReadData<Bid>(env, "bids.json");
    var discounts = await ReadData<Discount>(env, "discounts.json");
    var houseFeatures = await ReadData<HouseFeature>(env, "houseFeatures.json");

    // Check if data already exists to prevent duplicates
    if (!context.Features.Any()) {
      context.Features.AddRange(features);
    }
    if (!context.Sliders.Any()) {
      context.Sliders.AddRange(sliders);
    }
    if (!context.Houses.Any()) {
      context.Houses.AddRange(houses);
    }

    // Save base data
    await context.SaveChangesAsync();

    if (!context.HouseImages.Any()) {
      context.HouseImages.AddRange(houseImages);
    }
    if (!context.HouseFeatures.Any()) {
      context.HouseFeatures.AddRange(houseFeatures);
    }
    if (!context.Comments.Any()) {
      context.Comments.AddRange(comments);
    }
    if (!context.Orders.Any()) {
      context.Orders.AddRange(orders);
    }
    if (!context.Bids.Any()) {
      context.Bids.AddRange(bids);
    }
    if (!context.Discounts.Any()) {
      context.Discounts.AddRange(discounts);
    }

    // Save all data
    await context.SaveChangesAsync();
  }

  private static async Task<List<T>> ReadData<T>(IWebHostEnvironment env, string filePath) {
    var fullFilePath = Path.Combine(env.WebRootPath, "data", filePath);
    if (!File.Exists(fullFilePath)) return [];

    var json = await File.ReadAllTextAsync(fullFilePath);

    var data = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions {
      PropertyNameCaseInsensitive = true
    });

    return data ?? [];
  }
}