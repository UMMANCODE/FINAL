using Final_UI.Services.Interfaces;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using Final_UI.Helpers.Exceptions;
using Final_UI.Models.Responses;
using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;
#pragma warning disable CA1869

namespace Final_UI.Services.Implementations;

public class CrudService(IHttpContextAccessor contextAccessor) : ICrudService {
  private readonly HttpClient _client = new();

  private void AddAuthorizationHeader() {
    var token = contextAccessor.HttpContext!.Request.Cookies["token"];
    if (string.IsNullOrEmpty(token)) return;
    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
    _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
  }

  public async Task<TResponse?> CreateAsync<TRequest, TResponse>(TRequest data, string url) {
    AddAuthorizationHeader();
    var content = CreateMultipartContent(data);
    var response = await _client.PostAsync(url, content);
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return default;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<TResponse>(baseResponse.Data.ToString()
      ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }

  public async Task<TResponse?> DeleteAsync<TResponse>(string url) {
    AddAuthorizationHeader();
    var response = await _client.DeleteAsync(url);
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return default;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<TResponse>(baseResponse.Data.ToString()
      ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }


  public async Task<TResponse?> GetAsync<TResponse>(string url) {
    AddAuthorizationHeader();
    var response = await _client.GetAsync(url);
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);
    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<TResponse>(baseResponse.Data.ToString()
      ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }


  public async Task<PaginatedResponse<TResponse>?> GetAllPaginatedAsync<TResponse>(int page, string baseUrl, Dictionary<string, string> parameters) {
    AddAuthorizationHeader();
    var queryParams = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
    var url = $"{baseUrl}?pageNumber={page}&{queryParams}";

    var response = await _client.GetAsync(url);
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);
    var bodyStr = await response.Content.ReadAsStringAsync();

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (baseResponse?.Data == null)
      return null;

    var paginatedData = JsonSerializer.Deserialize<PaginatedResponse<TResponse>>(baseResponse.Data.ToString()
      ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return paginatedData;
  }


  public async Task<TResponse?> UpdateAsync<TRequest, TResponse>(TRequest data, string url) {
    AddAuthorizationHeader();
    var content = CreateMultipartContent(data);
    var response = await _client.PutAsync(url, content);
    if (!response.IsSuccessStatusCode) throw new HttpResponseException(response);

    var bodyStr = await response.Content.ReadAsStringAsync();

    if (string.IsNullOrEmpty(bodyStr)) return default;

    var baseResponse = JsonSerializer.Deserialize<BaseResponse>(bodyStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    return baseResponse?.Data == null ? default : JsonSerializer.Deserialize<TResponse>(baseResponse.Data.ToString()
      ?? string.Empty, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
  }


  public MultipartFormDataContent CreateMultipartContent<TRequest>(TRequest request) {
    var content = new MultipartFormDataContent();
    var properties = typeof(TRequest).GetProperties();

    foreach (var property in properties) {
      var value = property.GetValue(request);
      if (value == null) continue;
      if (property.PropertyType == typeof(IFormFile)) {
        var file = (IFormFile)value;
        var streamContent = CreateFileContent(file, property.Name);
        content.Add(streamContent);
      }
      else if (property.PropertyType == typeof(List<IFormFile>)) {
        var files = (List<IFormFile>)value;
        foreach (var streamContent in files.Select(file => CreateFileContent(file, property.Name))) {
          content.Add(streamContent);
        }
      }
      else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)) {
        var dateString = ((DateTime)value).ToString("yyyy-MM-dd");
        var stringContent = new StringContent(dateString);
        stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
          Name = property.Name
        };
        content.Add(stringContent);
      }
      else if (property.PropertyType == typeof(List<int>))
      {
        var intList = (List<int>)value;
        foreach (var stringContent in intList.Select(intItem => new StringContent(intItem.ToString()))) {
          stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
            Name = property.Name
          };
          content.Add(stringContent);
        }
      }
      else {
        var stringContent = new StringContent(value.ToString() ?? string.Empty);
        stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
          Name = property.Name
        };
        content.Add(stringContent);
      }
    }
    return content;
  }

  private static StreamContent CreateFileContent(IFormFile file, string name) {
    var streamContent = new StreamContent(file.OpenReadStream());

    streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") {
      Name = name,
      FileName = file.FileName
    };

    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

    return streamContent;
  }
}