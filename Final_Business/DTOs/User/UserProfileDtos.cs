namespace Final_Business.DTOs.User;

public record UserProfileDto(
     string Id,
     string FullName, 
     string UserName, 
     string Email, 
     string? AvatarLink, 
     string Nationality, 
     List<Order> Orders, 
     List<Comment> Comments, 
     List<Bid> Bids, 
     List<House> Houses
);

public record UserChangeDetailsDto(
     string? FullName, string? UserName, IFormFile? Avatar
);

public record UserChangePasswordDto(
     string OldPassword, string NewPassword, string ConfirmPassword
);
