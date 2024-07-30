using Final_Business.DTOs;
using Final_Business.DTOs.User;

namespace Final_Business.Services.Interfaces;
public interface IUserHouseService {
  public Task<int> Create(UserHouseCreateDto createDto);
  public Task<PaginatedList<UserHouseGetAllDto>> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<List<UserHouseGetAllDto>> GetAll();
  public Task<UserHouseGetOneDto> GetById(int id);
  public Task Update(int id, UserHouseUpdateDto updateDto);
  public Task Delete(int id);
}
