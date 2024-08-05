using Final_Business.DTOs.User;

namespace Final_Business.Services.Interfaces;
public interface IUserProfileService {
  Task<string> GetProfile(string token);
  Task<string> UpdateProfile(string token, UserChangeDetailsDto userChangeDetailsDto);
  Task<string> ChangePassword(string token, UserChangePasswordDto userChangePasswordDto);
}
