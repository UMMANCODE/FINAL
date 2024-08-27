namespace Final_API.Filters;

public class TraitDocumentFilter : IDocumentFilter {
  public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context) {

    if (context.DocumentName != "trait") return;

    var featurePaths = new OpenApiPaths();

    foreach (var path in swaggerDoc.Paths
               .Where(path =>
                 path.Key.Contains("trait", StringComparison.OrdinalIgnoreCase))) {
      featurePaths.Add(path.Key, path.Value);
    }

    swaggerDoc.Paths = featurePaths;
  }
}