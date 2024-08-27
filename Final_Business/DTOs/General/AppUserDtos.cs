namespace Final_Business.DTOs.General;

public record AppUserGetDto(
  string Id,
  string UserName,
  string FullName,
  string Email,
  string AvatarLink,
  string Nationality,
  List<string> Roles
);
