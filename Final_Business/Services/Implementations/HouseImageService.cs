using AutoMapper;
using Final_Business.DTOs.General;
using Final_Business.Exceptions;
using Final_Business.Services.Interfaces;
using Final_Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Final_Business.Services.Implementations {
  public class HouseImageService(IHouseImageRepository houseImageRepository, IMapper mapper) : IHouseImageService {
    public async Task<HouseImageGetDto> GetById(int id) {
      var houseImage = await houseImageRepository.GetAsync(x => x.Id == id, includes: "House");

      return houseImage == null
        ? throw new RestException(StatusCodes.Status404NotFound, "House Image not found")
        : mapper.Map<HouseImageGetDto>(houseImage);
    }
  }
}
