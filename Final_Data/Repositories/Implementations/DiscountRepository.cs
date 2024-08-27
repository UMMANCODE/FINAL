namespace Final_Data.Repositories.Implementations;
public class DiscountRepository(AppDbContext context) : Repository<Discount>(context), IDiscountRepository {
}