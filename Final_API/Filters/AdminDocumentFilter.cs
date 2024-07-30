using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Final_API.Filters;

public class AdminDocumentFilter : IDocumentFilter {
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {

    if (context.DocumentName != "admin") return;

    var adminPaths = new OpenApiPaths();

    foreach (var path in swaggerDoc.Paths
               .Where(path =>
                 path.Key.Contains("admin", StringComparison.OrdinalIgnoreCase))) {
      adminPaths.Add(path.Key, path.Value);
    }

    swaggerDoc.Paths = adminPaths;
  }
}