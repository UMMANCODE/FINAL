namespace Final_UI.Services.Interfaces;

public interface ICrudService {
  Task<TResponse?> CreateAsync<TRequest, TResponse>(TRequest data, string url);
  Task<TResponse?> UpdateAsync<TRequest, TResponse>(TRequest data, string url);
  Task<PaginatedResponse<TResponse>?> GetAllPaginatedAsync<TResponse>(int page, string baseUrl, Dictionary<string, string> parameters);
  Task<TResponse?> GetAsync<TResponse>(string url);
  Task<TResponse?> DeleteAsync<TResponse>(string url);
  MultipartFormDataContent CreateMultipartContent<TRequest>(TRequest request);
}