using Final_Business.DTOs;
using Final_Business.DTOs.Admin;

namespace Final_Business.Services.Interfaces;
public interface IAdminHouseService {
  public Task<int> Create(AdminHouseCreateDto createDto);
  public Task<PaginatedList<AdminHouseGetAllDto>> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<List<AdminHouseGetAllDto>> GetAll();
  public Task<AdminHouseGetOneDto> GetById(int id);
  public Task Update(int id, AdminHouseUpdateDto updateDto);
  public Task Delete(int id);
}
