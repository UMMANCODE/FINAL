using Final_Business.DTOs;
using Final_Business.DTOs.General;

namespace Final_Business.Services.Interfaces;

public interface IFeatureService {
  public Task<int> Create(FeatureCreateDto createDto);
  public Task<PaginatedList<FeatureGetAllDto>> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<List<FeatureGetAllDto>> GetAll();
  public Task<FeatureGetOneDto> GetById(int id);
  public Task Update(int id, FeatureUpdateDto updateDto);
  public Task Delete(int id);
}
