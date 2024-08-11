﻿namespace Final_UI.Models.Responses;

public class ErrorResponse {
  public int StatusCode { get; set; }
  public string? Message { get; set; }
  public List<ErrorResponseItem> Errors { get; set; } = [];
}

public class ErrorResponseItem {
  public string? Key { get; set; }
  public string? Message { get; set; }
}