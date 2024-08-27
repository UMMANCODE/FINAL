using System.Security.Claims;

namespace Final_Business.Services.Implementations;
public class UserBidService(IBidRepository bidRepository, IHouseRepository houseRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
  : IUserBidService {
  public async Task<BaseResponse> Create(UserBidCreateDto createDto) {
    var bid = mapper.Map<Bid>(createDto);

    var token = httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()
                ?? throw new RestException(StatusCodes.Status401Unauthorized, "Unauthorized");

    bid.AppUserId = JwtHelper.GetClaimFromJwt(token, ClaimTypes.NameIdentifier)!;

    var house = await houseRepository.GetAsync(x => x.Id == createDto.HouseId && !x.IsDeleted && x.Status == PropertyStatus.ForAuction) 
                ?? throw new RestException(StatusCodes.Status404NotFound, "House not found or not for auction");

    var bids = await bidRepository.GetAllAsync(x => x.HouseId == createDto.HouseId);

    if (bids.Any(x => x.Amount >= createDto.Amount)) {
      throw new RestException(StatusCodes.Status400BadRequest, "Amount must be greater than the highest bid");
    }

    house.Price = createDto.Amount;

    await bidRepository.AddAsync(bid);

    await houseRepository.SaveAsync();
    await bidRepository.SaveAsync();

    return new BaseResponse(201, "Created successfully!", mapper.Map<UserBidGetDto>(bid), []);
  }

  public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var bids = await bidRepository.GetPaginatedAsync(x => true, pageNumber, pageSize);
    var paginated = PaginatedList<Bid>.Create(bids, pageNumber, pageSize);

    var data = new PaginatedList<UserBidGetDto>(
      mapper.Map<List<UserBidGetDto>>(paginated.Items),
      paginated.TotalPages,
      pageNumber,
      pageSize
    );

    return new BaseResponse(200, "Success", data, []);
  }

  public async Task<BaseResponse> GetAll() {
    var bids = await bidRepository.GetAllAsync(x => true);

    return new BaseResponse(200, "Success", mapper.Map<List<UserBidGetDto>>(bids), []);
  }

  public async Task<BaseResponse> GetById(int id) {
    var bid = await bidRepository.GetAsync(x => x.Id == id, "AppUser", "House");

    return bid == null
      ? throw new RestException(StatusCodes.Status404NotFound, "bid not found")
      : new BaseResponse(200, "Success", mapper.Map<UserBidGetDto>(bid), []);
  }
}
