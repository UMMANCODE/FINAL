namespace Final_UI.Helpers.Exceptions;

public class HttpResponseException(HttpResponseMessage response) : Exception($"HTTP Error: {response.StatusCode}") {
  public HttpResponseMessage Response { get; } = response;
}