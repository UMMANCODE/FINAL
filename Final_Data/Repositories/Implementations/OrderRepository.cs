using Final_Core.Entities;
using Final_Data.Repositories.Interfaces;

namespace Final_Data.Repositories.Implementations;
public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository {
}
