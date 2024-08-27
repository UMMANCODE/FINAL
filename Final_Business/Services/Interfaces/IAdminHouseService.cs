namespace Final_Business.Services.Interfaces;
public interface IAdminHouseService {
  public Task<BaseResponse> Create(AdminHouseCreateDto createDto);
  public Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1);
  public Task<BaseResponse> GetAll();
  public Task<BaseResponse> GetById(int id);
  public Task<BaseResponse> Update(int id, AdminHouseUpdateDto updateDto);
  public Task<BaseResponse> Delete(int id);
}
