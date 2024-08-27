namespace Final_Business.Services.Interfaces;
public interface IHouseImageService {
  public Task<BaseResponse> GetById(int id);
}
