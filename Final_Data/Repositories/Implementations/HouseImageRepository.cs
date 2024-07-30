using Final_Core.Entities;
using Final_Data.Repositories.Interfaces;

namespace Final_Data.Repositories.Implementations;
public class HouseImageRepository(AppDbContext context) : Repository<HouseImage>(context), IHouseImageRepository {
}
