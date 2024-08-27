namespace Final_Data.Repositories.Implementations;
public class FeatureRepository(AppDbContext context) : Repository<Feature>(context), IFeatureRepository {
}
