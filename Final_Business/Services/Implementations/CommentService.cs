using System.Security.Claims;

namespace Final_Business.Services.Implementations;
public class CommentService(ICommentRepository commentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
  : ICommentService  {
  public async Task<BaseResponse> Create(CommentCreateDto createDto) {
    var comment = mapper.Map<Comment>(createDto);

    var token = httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
      ?? throw new RestException(StatusCodes.Status401Unauthorized, "Unauthorized");

    comment.AppUserId = JwtHelper.GetClaimFromJwt(token, ClaimTypes.NameIdentifier)!;

    await commentRepository.AddAsync(comment);
    await commentRepository.SaveAsync();

    return new BaseResponse(201, "Created successfully!", mapper.Map<CommentGetDto>(comment), []);
  }

  public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var comments = await commentRepository.GetPaginatedAsync(x => true, pageNumber, pageSize, "AppUser", "House");
    var paginated = PaginatedList<Comment>.Create(comments, pageNumber, pageSize);

    var data = new PaginatedList<CommentGetDto>(
      mapper.Map<List<CommentGetDto>>(paginated.Items),
      paginated.TotalPages,
      pageNumber,
      pageSize
      );

    return new BaseResponse(200, "Success", data, []);
  }

  public async Task<BaseResponse> GetAll() {
    var comments = await commentRepository.GetAllAsync(x => true, "AppUser", "House");

    return new BaseResponse(200, "Success", mapper.Map<List<CommentGetDto>>(comments), []);
  }

  public async Task<BaseResponse> GetById(int id) {
    var comment = await commentRepository.GetAsync(x => x.Id == id, "AppUser", "House");

    return comment == null
      ? throw new RestException(StatusCodes.Status404NotFound, "Comment not found")
      : new BaseResponse(200, "Success", mapper.Map<CommentGetDto>(comment), []);
  }

  public async Task<BaseResponse> UpdateStatus(int id, CommentStatus status) {
    var comment = await commentRepository.GetAsync(x => x.Id == id)
                ?? throw new RestException(StatusCodes.Status404NotFound, "Comment not found");

    if (comment.Status != CommentStatus.Pending)
      return new BaseResponse(400, "Comment status can only be updated if it's pending", null, []);

    comment.Status = status;
    await commentRepository.SaveAsync();

    return new BaseResponse(204, "Updated successfully!", null, []);
  }
}
