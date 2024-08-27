namespace Final_Data.Repositories.Implementations;
public class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository {
}
