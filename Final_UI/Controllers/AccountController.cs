using Final_UI.Models.Requests;
using Final_UI.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Final_UI.Services.Interfaces;
using Final_UI.Helpers.Extensions;
using Final_UI.Helpers.Exceptions;

namespace Final_UI.Controllers;

public class AccountController(IConfiguration configuration, ICrudService crudService) : Controller {

  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;
  private readonly HttpClient _client = new();

  public IActionResult Login(string returnUrl = "") {
    ViewBag.ReturnUrl = returnUrl;
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginRequest loginRequest, string? returnUrl) {
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(loginRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/login", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    if (response.IsSuccessStatusCode) {
      var token = apiResponse?.Data?.ToString();

      if (string.IsNullOrEmpty(token)) {
        TempData["Error"] = "Invalid token received.";
        return View();
      }

      var cookieOptions = new CookieOptions {
        HttpOnly = true,
        Secure = true,
        Expires = loginRequest.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddDays(1)
      };

      Response.Cookies.Append("token", "Bearer " + token, cookieOptions);

      if (!string.IsNullOrEmpty(returnUrl)) {
        return Redirect(returnUrl);
      }
      return RedirectToAction("Index", "Home");
    }

    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
    switch (response.StatusCode) {
      case System.Net.HttpStatusCode.BadRequest:
        ModelState.AddModelError("", "UserName or Password incorrect!");
        return View();
      case System.Net.HttpStatusCode.Unauthorized:
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
    // Check if the model state is valid
    if (!ModelState.IsValid) {
      return View(registerRequest);
    }

    // Check if the uploaded avatar is a valid image
    if (!UploadExtension.IsValidImage(registerRequest.Avatar)) {
      ModelState.AddModelError("Avatar", "Not a valid image type!");
      return View(registerRequest);
    }

    // Create multipart content for the request
    var content = crudService.CreateMultipartContent(registerRequest);

    // Send the POST request to the API
    using var response = await _client.PostAsync($"{_apiUrl}/Auth/register", content);

    // Check if the API response is successful
    if (!response.IsSuccessStatusCode) {
      TempData["Error"] = $"Something went wrong: {response.ReasonPhrase}";
      return View(registerRequest);
    }

    // Read the response content
    var bodyStr = await response.Content.ReadAsStringAsync();

    // Check if the response body is empty
    if (string.IsNullOrEmpty(bodyStr)) {
      TempData["Error"] = "Received an empty response from the server.";
      return View(registerRequest);
    }

    // Deserialize the response content
    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    // Check if the deserialization was successful and data is present
    if (baseResponse?.Data == null) {
      TempData["Error"] = $"Error: {baseResponse?.Message ?? "No data received."}";
      return View(registerRequest);
    }

    // Deserialize the data into RegisterResponse
    JsonSerializer.Deserialize<RegisterResponse>(baseResponse.Data.ToString()
        ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    // Set success message and redirect to the VerifyEmail page
    TempData["Success"] = baseResponse.Message;
    return RedirectToAction("VerifyEmail");
  }


  public IActionResult ForgetPassword() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest forgetPasswordRequest) {
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(forgetPasswordRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/forget-password", content);
    if (response.IsSuccessStatusCode) {
      TempData["Success"] = "Password reset link sent to your email.";
      return View();
    }

    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);
    TempData["Error"] = $"Something went wrong: {apiResponse?.Message}";
    return View();
  }

  public IActionResult ResetPassword() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest) {
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

    TempData["Error"] = $"Something went wrong: {apiResponse?.Message}";
    return View();
  }

  public IActionResult VerifyEmail() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> VerifyEmail(VerifyEmailRequest verifyEmailRequest) {
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

    TempData["Error"] = $"Something went wrong: {apiResponse?.Message}";
    return View();
  }

  public IActionResult SendVerifyEmail() {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> SendVerifyEmail(SendVerifyEmailRequest sendVerifyEmailRequest) {
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var content = new StringContent(JsonSerializer.Serialize(sendVerifyEmailRequest, options), System.Text.Encoding.UTF8, "application/json");

    using var response = await _client.PostAsync($"{_apiUrl}/Auth/send-verify-email", content);
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var apiResponse = JsonSerializer.Deserialize<BaseResponse>(jsonResponse, options);

    if (response.IsSuccessStatusCode) {
      TempData["Success"] = apiResponse?.Message;
      return RedirectToAction("VerifyEmail");
    }

    TempData["Error"] = $"Something went wrong: {apiResponse?.Message}";
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
}