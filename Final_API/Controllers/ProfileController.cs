using AutoMapper;
using Azure;
using Final_Business.DTOs.User;
using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;
[Route("api/[controller]/user")]
[ApiController]
public class ProfileController(IUserProfileService userProfileService) : ControllerBase {

  [HttpGet]
  [Authorize(Roles = "Member")]
  public async Task<IActionResult> GetProfile() {
    var response = await userProfileService.GetProfile();
    return StatusCode(response.StatusCode, response);
  }

  [HttpPut("update-details")]
  [Authorize(Roles = "Member")]
  public async Task<IActionResult> UpdateProfile([FromForm] UserChangeDetailsDto changeDetailsDto) {
    var response = await userProfileService.UpdateProfile(changeDetailsDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPut("change-password")]
  [Authorize(Roles = "Member")]
  public async Task<IActionResult> ChangePassword([FromForm] UserChangePasswordDto changePasswordDto) {
    var response = await userProfileService.ChangePassword(changePasswordDto);
    return StatusCode(response.StatusCode, response);
  }
}
