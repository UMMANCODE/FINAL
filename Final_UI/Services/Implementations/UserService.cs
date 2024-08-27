using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Net.Http.Headers;

namespace Final_UI.Services.Implementations;

public class UserService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : IUserService {
  private readonly HttpClient _client = new();
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;

  public string? GetUserId() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"]
                ?? string.Empty;
    return GetClaimFromToken(token, ClaimTypes.NameIdentifier);
  }

  public string? GetUserName() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"]
                ?? string.Empty;
    return GetClaimFromToken(token, ClaimTypes.Name);
  }

  public string? GetRole() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"]
                ?? string.Empty;
    return GetClaimFromToken(token, ClaimTypes.Role);
  }

  public string? GetEmail() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"]
                ?? string.Empty;
    return GetClaimFromToken(token, ClaimTypes.Email);
  }

  public string? GetFullName() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"]
                ?? string.Empty;
    return GetClaimFromToken(token, "FullName");
  }

  public async Task<List<AppUserResponse>?> GetUsers() {
    // Add Authorization header
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return null;
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);

    var response = await _client.GetAsync($"{_apiUrl}/Auth/community");
    if (!response.IsSuccessStatusCode) return null;

    var bodyStr = await response.Content.ReadAsStringAsync();
    if (string.IsNullOrEmpty(bodyStr)) return null;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? null : JsonSerializer.Deserialize<List<AppUserResponse>>(baseResponse.Data.ToString()
         ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }

  private string? GetClaimFromToken(string token, string claimType) {
    if (string.IsNullOrEmpty(token))
      return null;

    token = token.Replace("Bearer ", string.Empty);

    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

    var claim = jwtToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
    return claim;
  }
}
