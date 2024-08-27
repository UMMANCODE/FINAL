using System.Linq.Expressions;

namespace Final_Data.Repositories.Implementations;

public class SliderRepository(AppDbContext context) : Repository<Slider>(context), ISliderRepository {
  private readonly AppDbContext _context = context;

  public async Task<bool> ExistsAsync(Expression<Func<Slider, bool>> predicate, params string[] includes) {
    var query = _context.Set<Slider>().AsQueryable();

    query = includes.Aggregate(query, (current, include) => current.Include(include));

    return await query.AnyAsync(predicate);
  }
}
