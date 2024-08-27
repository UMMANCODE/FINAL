namespace Final_API.Filters;

public class AuthDocumentFilter : IDocumentFilter {
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {

    if (context.DocumentName != "auth") return;

    var authPaths = new OpenApiPaths();

    foreach (var path in swaggerDoc.Paths
               .Where(path =>
                 path.Key.Contains("auth", StringComparison.OrdinalIgnoreCase))) {
      authPaths.Add(path.Key, path.Value);
    }

    swaggerDoc.Paths = authPaths;
  }
}