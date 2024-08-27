using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
[ServiceFilter(typeof(AdminOrSuperAdminFilter))]
public class ProfileController(IConfiguration configuration, ICrudService crudService, IDataService dataService, IHttpContextAccessor contextAccessor) : Controller {

  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;
  private readonly HttpClient _client = new();

  public async Task<IActionResult?> Index() {
    var profile = await dataService.GetProfile();
    if (profile == null) return RedirectToAction("Unauthorized", "Error");

    var profileResponse = CreateProfileResponse(profile);

    ViewBag.AvatarLink = profile.AvatarLink;

    return View(profileResponse);
  }

  [HttpPost]
  public async Task<IActionResult> ChangeDetails([FromForm] ChangeDetailsRequest changeDetailsRequest) {
    if (!ModelState.IsValid) {
      var profile = await dataService.GetProfile();
      if (profile == null) return RedirectToAction("Unauthorized", "Error");
      var profileResponse = CreateProfileResponse(profile);
      return View("Index", profileResponse);
    }

    if (changeDetailsRequest.Avatar is not null && !UploadExtension.IsValidImage(changeDetailsRequest.Avatar)) {
      ModelState.AddModelError("Avatar", "Not a valid image type!");
      var profile = await dataService.GetProfile();
      if (profile == null) return RedirectToAction("Unauthorized", "Error");
      var profileResponse = CreateProfileResponse(profile);
      return View("Index", profileResponse);
    }

    // Add Authorization header
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return RedirectToAction("Unauthorized", "Error");
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = crudService.CreateMultipartContent(changeDetailsRequest);

    using var response = await _client.PutAsync($"{_apiUrl}/trait/Profile/update-details", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    TempData["Type"] = "success";
    TempData["Message"] = apiResponse?.Message;

    // ReSharper disable once InvertIf
    if (!response.IsSuccessStatusCode) {
      TempData["Type"] = "error";
    }

    return RedirectToAction("Index", "Profile");
  }

  [HttpPost]
  public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest) {
    if (!ModelState.IsValid) {
      var profile = await dataService.GetProfile();
      if (profile == null) return RedirectToAction("Unauthorized", "Error");
      var profileResponse = CreateProfileResponse(profile);
      return View("Index", profileResponse);
    }

    // Add Authorization header
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return RedirectToAction("Unauthorized", "Error");
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(changePasswordRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PutAsync($"{_apiUrl}/trait/Profile/change-password", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    TempData["Type"] = "success";
    TempData["Message"] = apiResponse?.Message;

    // ReSharper disable once InvertIf
    if (!response.IsSuccessStatusCode) {
      TempData["Type"] = "error";
    }

    return RedirectToAction("Index", "Profile");
  }

  private static ProfileResponse CreateProfileResponse(AppUserResponse profile) {
    return new ProfileResponse {
      ChangeDetailsRequest = new ChangeDetailsRequest {
        FullName = profile.FullName,
        UserName = profile.UserName
      },
      ChangePasswordRequest = new ChangePasswordRequest()
    };
  }
}
