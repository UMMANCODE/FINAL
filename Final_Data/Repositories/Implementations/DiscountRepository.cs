using Final_Core.Entities;
using Final_Data.Repositories.Interfaces;

namespace Final_Data.Repositories.Implementations;
public class DiscountRepository(AppDbContext context) : Repository<Discount>(context), IDiscountRepository {
}