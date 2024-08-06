using Final_Business.DTOs.General;
using Final_Business.Helpers;

namespace Final_Business.Services.Interfaces;

public interface IFeatureService {
  public Task<BaseResponse> Create(FeatureCreateDto createDto);
  public Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<BaseResponse> GetAll();
  public Task<BaseResponse> GetById(int id);
  public Task<BaseResponse> Update(int id, FeatureUpdateDto updateDto);
  public Task<BaseResponse> Delete(int id);
}
