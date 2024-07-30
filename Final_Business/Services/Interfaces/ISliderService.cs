using Final_Business.DTOs;
using Final_Business.DTOs.General;

namespace Final_Business.Services.Interfaces;

public interface ISliderService {
  public Task<int> Create(SliderCreateDto createDto);
  public Task<PaginatedList<SliderGetAllDto>> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<List<SliderGetAllDto>> GetAll();
  public Task<SliderGetOneDto> GetById(int id);
  public Task Update(int id, SliderUpdateDto updateDto);
  public Task Delete(int id);
}