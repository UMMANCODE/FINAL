using Final_Business.DTOs.General;
using Final_Business.Helpers;
using Final_Core.Enums;

namespace Final_Business.Services.Interfaces;
public interface ICommentService {
  public Task<BaseResponse> Create(CommentCreateDto createDto);
  public Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<BaseResponse> GetAll();
  public Task<BaseResponse> GetById(int id);
  public Task<BaseResponse> UpdateStatus(int id, CommentStatus status);
}
