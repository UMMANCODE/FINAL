using Final_Business.DTOs.User;
using Final_Core.Entities;

namespace Final_Business.Services.Interfaces;
public interface IUserAuthService {
  Task<string> Login(UserLoginDto loginDto);
  Task<string> Register(UserRegisterDto registerDto);
  Task<string> ForgetPassword(UserForgetPasswordDto forgetPasswordDto);
  Task<string> ResetPassword(UserResetPasswordDto resetPasswordDto);
  Task<string> VerifyEmail(UserVerifyEmailDto verifyEmailDto);
  Task<List<AppUser>> GetUsers();
}
