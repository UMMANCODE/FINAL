using System.Linq.Expressions;

namespace Final_Data.Repositories.Interfaces;
public interface ISliderRepository : IRepository<Slider> {
  Task<bool> ExistsAsync(Expression<Func<Slider, bool>> predicate, params string[] includes);
}
