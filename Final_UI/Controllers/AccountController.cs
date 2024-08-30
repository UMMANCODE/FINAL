using System.Net;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

namespace Final_UI.Controllers;

public class AccountController(IConfiguration configuration, ICrudService crudService, IHttpContextAccessor contextAccessor) : Controller {

  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;
  private readonly HttpClient _client = new();

  public IActionResult Login(string returnUrl = "") {
    ViewBag.ReturnUrl = returnUrl;
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginRequest loginRequest, string? returnUrl) {
    if (!ModelState.IsValid) {
      return View(loginRequest);
    }
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(loginRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/login", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    if (response.IsSuccessStatusCode) {
      var token = apiResponse?.Data?.ToString();
      var message = apiResponse?.Message!;

      if (string.IsNullOrEmpty(token)) {
        TempData["Error"] = "Invalid token received.";
        return View();
      }

      var cookieOptions = new CookieOptions {
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.Lax,
        Expires = loginRequest.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddDays(1)
      };

      Response.Cookies.Append("token", "Bearer " + token, cookieOptions);

      if (message.Contains("Change")) {
        return RedirectToAction("ChangePassword", "Account");
      }

      if (!string.IsNullOrEmpty(returnUrl)) {
        return Redirect(returnUrl);
      }
      return RedirectToAction("Index", "Home");
    }



    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
    switch (response.StatusCode) {
      case HttpStatusCode.BadRequest:
        ModelState.AddModelError("", "UserName or Password incorrect!");
        return View();
      case HttpStatusCode.Unauthorized:
      case HttpStatusCode.Forbidden:
        ModelState.AddModelError("", apiResponse?.Message ?? "Something went wrong!");

        if (!apiResponse!.Message!.Contains("Email not verified!")) return View();
        var actualEmail = apiResponse.Message.Split('!')[1];
        TempData["Email"] = actualEmail;
        return RedirectToAction("SendVerifyEmail");
      default:
        TempData["Error"] = "Something went wrong";
        return View();
    }
  }

  public IActionResult Register() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Register([FromForm] RegisterRequest registerRequest) {
    if (!ModelState.IsValid) {
      return View(registerRequest);
    }

    if (!UploadExtension.IsValidImage(registerRequest.Avatar)) {
      ModelState.AddModelError("Avatar", "Not a valid image type!");
      return View(registerRequest);
    }

    var content = crudService.CreateMultipartContent(registerRequest);

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/register", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (!response.IsSuccessStatusCode) {
      TempData["Error"] = apiResponse?.Message;
      return View(registerRequest);
    }

    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (baseResponse?.Data == null) {
      TempData["Error"] = $"Error: {baseResponse?.Message ?? "No data received."}";
      return View(registerRequest);
    }

    JsonSerializer.Deserialize<RegisterResponse>(baseResponse.Data.ToString()
        ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    TempData["Success"] = baseResponse.Message;
    return RedirectToAction("VerifyEmail");
  }

  public IActionResult CreateAdmin() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult?> CreateAdmin([FromForm] CreateAdminRequest createAdminRequest) {
    // Add Authorization header
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return null;
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

    if (!ModelState.IsValid) {
      return View(createAdminRequest);
    }

    if (createAdminRequest.Avatar is not null && !UploadExtension.IsValidImage(createAdminRequest.Avatar)) {
      ModelState.AddModelError("Avatar", "Not a valid image type!");
      return View(createAdminRequest);
    }

    var content = crudService.CreateMultipartContent(createAdminRequest);

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/create-super", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (!response.IsSuccessStatusCode) {
      TempData["Error"] = apiResponse?.Message;
      return View(createAdminRequest);
    }

    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (baseResponse?.Data == null) {
      TempData["Error"] = $"Error: {baseResponse?.Message ?? "No data received."}";
      return View(createAdminRequest);
    }

    JsonSerializer.Deserialize<CreateAdminResponse>(baseResponse.Data.ToString() 
        ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return RedirectToAction("Index", "Home");
  }


  public IActionResult ForgetPassword() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest forgetPasswordRequest) {
    if (!ModelState.IsValid) {
      return View(forgetPasswordRequest);
    }

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(forgetPasswordRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/forget-password", content);
    if (response.IsSuccessStatusCode) {
      TempData["Success"] = "Password reset link sent to your email.";
      return View();
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);
    TempData["Error"] = apiResponse?.Message;
    return View();
  }

  public IActionResult ResetPassword() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest) {
    if (!ModelState.IsValid) {
      return View(resetPasswordRequest);
    }

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(resetPasswordRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/reset-password", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);
    if (response.IsSuccessStatusCode) {
      TempData["Message"] = "Verify email link sent to your email.";
      TempData["Type"] = "success";
      return RedirectToAction("Login");
    }

    TempData["Error"] = apiResponse?.Message;
    return View();
  }

  public IActionResult VerifyEmail() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> VerifyEmail(VerifyEmailRequest verifyEmailRequest) {
    if (!ModelState.IsValid) {
      return View(verifyEmailRequest);
    }

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(verifyEmailRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/verify-email", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);
    if (response.IsSuccessStatusCode) {
      TempData["Message"] = apiResponse?.Message;
      TempData["Type"] = "success";
      return RedirectToAction("Login");
    }

    TempData["Error"] = apiResponse?.Message;
    return View();
  }

  public IActionResult SendVerifyEmail() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> SendVerifyEmail(SendVerifyEmailRequest sendVerifyEmailRequest) {
    if (!ModelState.IsValid) {
      return View(sendVerifyEmailRequest);
    }

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(sendVerifyEmailRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/send-verify-email", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    if (response.IsSuccessStatusCode) {
      TempData["Success"] = apiResponse?.Message;
      return RedirectToAction("VerifyEmail");
    }

    TempData["Error"] = apiResponse?.Message;
    return View();
  }


  public IActionResult Logout() {
    Response.Cookies.Delete("token");
    return RedirectToAction("Login");
  }

  public IActionResult ExternalLoginCallback(string token) {
    if (string.IsNullOrEmpty(token)) {
      return RedirectToAction("Login", "Account");
    }

    var cookieOptions = new CookieOptions {
      HttpOnly = true,
      Secure = true,
      Expires = DateTimeOffset.UtcNow.AddDays(1)
    };

    Response.Cookies.Append("token", "Bearer " + token, cookieOptions);

    return RedirectToAction("Index", "Home");
  }

  public IActionResult ChangePassword() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ChangePassword(ForceChangePasswordRequest changePasswordRequest) {
    if (!ModelState.IsValid) {
      return View(changePasswordRequest);
    }

    // Add Authorization header
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return RedirectToAction("Unauthorized", "Error");
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(changePasswordRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PutAsync($"{_apiUrl}/Auth/change-password", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    if (response.IsSuccessStatusCode) {
      TempData["Type"] = "success";
      TempData["Message"] = apiResponse?.Message;
      return RedirectToAction("Index", "Home");
    }

    TempData["Error"] = apiResponse?.Message;
    return View();
  }
}
