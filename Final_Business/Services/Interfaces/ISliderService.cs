using Final_Business.DTOs;
using Final_Business.DTOs.General;
using Final_Business.Helpers;

namespace Final_Business.Services.Interfaces;

public interface ISliderService {
  public Task<BaseResponse> Create(SliderCreateDto createDto);
  public Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<BaseResponse> GetAll();
  public Task<BaseResponse> GetById(int id);
  public Task<BaseResponse> Update(int id, SliderUpdateDto updateDto);
  public Task<BaseResponse> Delete(int id);
}