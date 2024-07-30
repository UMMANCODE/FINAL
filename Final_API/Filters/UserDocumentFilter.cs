using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;


namespace Final_API.Filters;

public class UserDocumentFilter : IDocumentFilter {
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {

    if (context.DocumentName != "user") return;

    var userPaths = new OpenApiPaths();

    foreach (var path in swaggerDoc.Paths
               .Where(path =>
                 path.Key.Contains("user", StringComparison.OrdinalIgnoreCase))) {
      userPaths.Add(path.Key, path.Value);
    }

    swaggerDoc.Paths = userPaths;
  }
}
