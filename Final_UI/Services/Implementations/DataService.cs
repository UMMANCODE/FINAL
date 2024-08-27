using System.Text.Json;
using Microsoft.Net.Http.Headers;

namespace Final_UI.Services.Implementations;

public class DataService(IHttpContextAccessor contextAccessor, IConfiguration configuration) : IDataService {
  private readonly HttpClient _client = new();
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;

  private void AddAuthorizationHeader() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return;
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
  }

  public async Task<AppUserResponse?> GetProfile() {
    AddAuthorizationHeader();
    using var response = await _client.GetAsync($"{_apiUrl}/trait/Profile");
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);
    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<AppUserResponse>(baseResponse.Data.ToString()
         ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }

  public async Task<List<OrderResponse>?> GetOrders() {
    AddAuthorizationHeader();
    using var response = await _client.GetAsync($"{_apiUrl}/Orders/admin/all");

    if (!response.IsSuccessStatusCode)
      throw new HttpResponseException(response);

    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    var orders = baseResponse?.Data == null
      ? default
      : JsonSerializer.Deserialize<List<OrderResponse>>(baseResponse.Data.ToString() ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (orders == null) return orders;
    foreach (var order in orders.Where(order => !string.IsNullOrEmpty(order.AppUserAvatarLink))) {
      if (!order.AppUserAvatarLink.Contains("google"))
        order.AppUserAvatarLink = $"{_apiUrl.Replace("/api", "/")}images/users/{order.AppUserAvatarLink}";
    }

    return orders;
  }

  public async Task<List<CommentResponse>?> GetComments() {
    AddAuthorizationHeader();
    using var response = await _client.GetAsync($"{_apiUrl}/Comments/admin/all");

    if (!response.IsSuccessStatusCode)
      throw new HttpResponseException(response);

    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    var comments = baseResponse?.Data == null
      ? default
      : JsonSerializer.Deserialize<List<CommentResponse>>(baseResponse.Data.ToString() ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (comments == null) return comments;
    foreach (var comment in comments.Where(comment => !string.IsNullOrEmpty(comment.AppUserAvatarLink))) {
      if (!comment.AppUserAvatarLink.Contains("google"))
        comment.AppUserAvatarLink = $"{_apiUrl.Replace("/api", "/")}images/users/{comment.AppUserAvatarLink}";
    }

    return comments;
  }

  public async Task<bool> UpdateOrderStatus(int id, OrderStatus status) {
    AddAuthorizationHeader();
    using var response = await _client.PutAsync($"{_apiUrl}/Orders/admin/{id}/status?status={status}", null);
    if (!response.IsSuccessStatusCode) return false;

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return false;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Message != null && baseResponse.Message.Contains("success");
  }

  public async Task<bool> UpdateCommentStatus(int id, CommentStatus status) {
    AddAuthorizationHeader();
    using var response = await _client.PutAsync($"{_apiUrl}/Comments/admin/{id}/status?status={status}", null);
    if (!response.IsSuccessStatusCode) return false;

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return false;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Message != null && baseResponse.Message.Contains("success");
  }
}
