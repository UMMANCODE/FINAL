using AutoMapper;
using Final_Business.DTOs;
using Final_Business.DTOs.General;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Final_Data.Repositories.Interfaces;

namespace Final_Business.Services.Implementations;
public class CommentService(ICommentRepository commentRepository, IMapper mapper)
  : ICommentService  {
  public async Task<int> Create(CommentCreateDto createDto) {
    var comment = mapper.Map<Comment>(createDto);
    await commentRepository.AddAsync(comment);
    await commentRepository.SaveAsync();
    return comment.Id;
  }

  public async Task<PaginatedList<CommentGetDto>> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var comments = await commentRepository.GetPaginatedAsync(x => true, pageNumber, pageSize);
    var paginated = PaginatedList<Comment>.Create(comments, pageNumber, pageSize);

    return new PaginatedList<CommentGetDto>(
      mapper.Map<List<CommentGetDto>>(paginated.Items),
      paginated.TotalPages,
      pageNumber,
      pageSize
      );
  }

  public async Task<List<CommentGetDto>> GetAll() {
    var comments = await commentRepository.GetAllAsync(x => true);
    return mapper.Map<List<CommentGetDto>>(comments);
  }

  public async Task<CommentGetDto> GetById(int id) {
    var comment = await commentRepository.GetAsync(x => x.Id == id, "AppUser", "House");
    return mapper.Map<CommentGetDto>(comment);
  }
}
