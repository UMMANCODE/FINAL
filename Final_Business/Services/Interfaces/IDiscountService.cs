namespace Final_Business.Services.Interfaces;
public interface IDiscountService {
  public Task<BaseResponse> Create(DiscountCreateDto createDto);
  public Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<BaseResponse> GetAll();
  public Task<BaseResponse> GetById(int id);
  public Task<BaseResponse> Delete(int id);
}
