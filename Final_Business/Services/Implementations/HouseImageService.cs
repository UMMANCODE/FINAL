namespace Final_Business.Services.Implementations {
  public class HouseImageService(IHouseImageRepository houseImageRepository, IMapper mapper) : IHouseImageService {
    public async Task<BaseResponse> GetById(int id) {
      var houseImage = await houseImageRepository.GetAsync(x => x.Id == id, includes: "House");

      return houseImage == null
        ? throw new RestException(StatusCodes.Status404NotFound, "House Image not found")
        : new BaseResponse(200, "Success", mapper.Map<HouseImageGetDto>(houseImage), []);
    }
  }
}
