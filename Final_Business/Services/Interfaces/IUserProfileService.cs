using Final_Business.DTOs.User;
using Final_Business.Helpers;
using Final_Core.Entities;

namespace Final_Business.Services.Interfaces;
public interface IUserProfileService {
  Task<BaseResponse> GetProfile();
  Task<BaseResponse> UpdateProfile(UserChangeDetailsDto userChangeDetailsDto);
  Task<BaseResponse> ChangePassword(UserChangePasswordDto userChangePasswordDto);
}
