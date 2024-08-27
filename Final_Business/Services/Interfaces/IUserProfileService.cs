namespace Final_Business.Services.Interfaces;
public interface IUserProfileService {
  Task<BaseResponse> GetProfile();
  Task<BaseResponse> UpdateProfile(UserChangeDetailsDto userChangeDetailsDto);
  Task<BaseResponse> ChangePassword(UserChangePasswordDto userChangePasswordDto);
}
