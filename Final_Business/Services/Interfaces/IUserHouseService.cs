using Final_Business.DTOs;
using Final_Business.DTOs.User;
using Final_Business.Helpers;

namespace Final_Business.Services.Interfaces;
public interface IUserHouseService {
  public Task<BaseResponse> Create(UserHouseCreateDto createDto);
  public Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<BaseResponse> GetAll();
  public Task<BaseResponse> GetById(int id);
  public Task<BaseResponse> Update(int id, UserHouseUpdateDto updateDto);
  public Task<BaseResponse> Delete(int id);
}
