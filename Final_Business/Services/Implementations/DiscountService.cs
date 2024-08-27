namespace Final_Business.Services.Implementations;
public class DiscountService(IDiscountRepository discountRepository, IMapper mapper, IHouseRepository houseRepository)
  : IDiscountService {
  public async Task<BaseResponse> Create(DiscountCreateDto createDto) {
    var house = await houseRepository.GetAsync(x => x.Id == createDto.HouseId && !x.IsDeleted && x.Status == PropertyStatus.ForSale)
                ?? throw new RestException(StatusCodes.Status404NotFound, "House not found or not for auction");

    var discount = mapper.Map<Discount>(createDto)
                   ?? throw new RestException(StatusCodes.Status400BadRequest, "Discount creation failed");

    var discounts = (await discountRepository.GetAllAsync(x => x.HouseId == createDto.HouseId))
                    ?? throw new RestException(StatusCodes.Status400BadRequest, "Failed to retrieve discounts for the house");

    if (discount.ExpiryDate < DateTime.Now) {
      throw new RestException(StatusCodes.Status400BadRequest, "Expiry date must be greater than the current date");
    }

    if (discounts.Any(x => x.Code == createDto.Code)) {
      throw new RestException(StatusCodes.Status400BadRequest, "Discount code already exists");
    }

    if (discounts.Any(x => x.HouseId == house.Id && x.ExpiryDate >= DateTime.Now)) {
      throw new RestException(StatusCodes.Status400BadRequest, "There is an active discount for this house");
    }

    if (discount.Amount > house.Price) {
      throw new RestException(StatusCodes.Status400BadRequest, "Discount amount must be less than the house price");
    }

    await discountRepository.AddAsync(discount);
    await discountRepository.SaveAsync();

    return new BaseResponse(201, "Created successfully!", mapper.Map<DiscountGetDto>(discount), []);
  }

  public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var discounts = await discountRepository.GetPaginatedAsync(x => true, pageNumber, pageSize);
    var paginated = PaginatedList<Discount>.Create(discounts, pageNumber, pageSize);

    var data = new PaginatedList<DiscountGetDto>(
      mapper.Map<List<DiscountGetDto>>(paginated.Items),
      paginated.TotalPages,
      pageNumber,
      pageSize
    );

    return new BaseResponse(200, "Success", data, []);
  }

  public async Task<BaseResponse> GetAll() {
    var discounts = await discountRepository.GetAllAsync(x => true);

    return new BaseResponse(200, "Success", mapper.Map<List<DiscountGetDto>>(discounts), []);
  }

  public async Task<BaseResponse> GetById(int id) {
    var discount = await discountRepository.GetAsync(x => x.Id == id, "AppUser", "House");

    return discount == null
      ? throw new RestException(StatusCodes.Status404NotFound, "Discount not found")
      : new BaseResponse(200, "Success", mapper.Map<DiscountGetDto>(discount), []);
  }

  public async Task<BaseResponse> Delete(int id) {
    var discount = await discountRepository.GetAsync(x => x.Id == id)
                   ?? throw new RestException(StatusCodes.Status404NotFound, "Discount not found");

    await discountRepository.DeleteAsync(discount);
    await discountRepository.SaveAsync();

    return new BaseResponse(204, "Deleted successfully!", null, []);
  }
}