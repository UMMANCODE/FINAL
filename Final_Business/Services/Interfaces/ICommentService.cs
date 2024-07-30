using Final_Business.DTOs;
using Final_Business.DTOs.General;

namespace Final_Business.Services.Interfaces;
public interface ICommentService {
  public Task<int> Create(CommentCreateDto createDto);
  public Task<PaginatedList<CommentGetDto>> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<List<CommentGetDto>> GetAll();
  public Task<CommentGetDto> GetById(int id);
}
