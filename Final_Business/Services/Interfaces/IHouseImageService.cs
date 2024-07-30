using Final_Business.DTOs.General;

namespace Final_Business.Services.Interfaces;
public interface IHouseImageService {
  public Task<HouseImageGetDto> GetById(int id);
}
