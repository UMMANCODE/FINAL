namespace Final_Data.Repositories.Implementations;
public class CommentRepository(AppDbContext context) : Repository<Comment>(context), ICommentRepository {
}