using Final_Business.DTOs.User;
using Final_Business.Helpers;

namespace Final_Business.Services.Interfaces;
public interface IUserAuthService {
  Task<BaseResponse> Login(UserLoginDto loginDto);
  Task<BaseResponse> Register(UserRegisterDto registerDto);
  Task<BaseResponse> ForgetPassword(UserForgetPasswordDto forgetPasswordDto);
  Task<BaseResponse> ResetPassword(UserResetPasswordDto resetPasswordDto);
  Task<BaseResponse> VerifyEmail(UserVerifyEmailDto verifyEmailDto);
  Task<BaseResponse> SendVerifyEmail(UserSendVerifyEmailDto sendVerifyEmailDto);
  Task<BaseResponse> GetUsers();
}
