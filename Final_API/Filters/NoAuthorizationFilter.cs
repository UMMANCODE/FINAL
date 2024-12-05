using Hangfire.Dashboard;

namespace Final_API.Filters;

public class NoAuthorizationFilter : IDashboardAuthorizationFilter {
  public bool Authorize(DashboardContext context) {
    return true;
  }
}