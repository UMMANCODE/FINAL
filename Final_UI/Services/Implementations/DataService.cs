using System.Text.Json;
using Final_UI.Helpers.Enums;
using Final_UI.Helpers.Exceptions;
using Final_UI.Models.Responses;
using Final_UI.Services.Interfaces;
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

  public async Task<List<OrderResponse>?> GetOrders() {
    AddAuthorizationHeader();
    var response = await _client.GetAsync($"{_apiUrl}/Orders/admin/all");
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);
    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<List<OrderResponse>>(baseResponse.Data.ToString()
      ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }

  public async Task<List<CommentResponse>?> GetComments() {
    AddAuthorizationHeader();
    var response = await _client.GetAsync($"{_apiUrl}/Comments/admin/all");
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);
    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<List<CommentResponse>>(baseResponse.Data.ToString()
         ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }

  public async Task<bool> UpdateOrderStatus(int id, OrderStatus status) {
    AddAuthorizationHeader();
    var response = await _client.PutAsync($"{_apiUrl}/Orders/admin/{id}/status?status={status}", null);
    if (!response.IsSuccessStatusCode) return false;

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return false;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Message != null && baseResponse.Message.Contains("success");
  }

  public async Task<bool> UpdateCommentStatus(int id, CommentStatus status) {
    AddAuthorizationHeader();
    var response = await _client.PutAsync($"{_apiUrl}/Comments/admin/{id}/status?status={status}", null);
    if (!response.IsSuccessStatusCode) return false;

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return false;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Message != null && baseResponse.Message.Contains("success");
  }
}
