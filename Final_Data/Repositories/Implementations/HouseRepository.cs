using System.Linq.Expressions;

namespace Final_Data.Repositories.Implementations;
public class HouseRepository(AppDbContext context) : Repository<House>(context), IHouseRepository {
}
