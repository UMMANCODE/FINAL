using Final_Business.DTOs.User;

namespace Final_Business.Tests;

public class UserBidServiceTests {
  private readonly Mock<IBidRepository> _bidRepositoryMock;
  private readonly Mock<IHouseRepository> _houseRepositoryMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly UserBidService _userBidService;
  private readonly Mock<IHttpContextAccessor> _contextAccessorMock;

  public UserBidServiceTests() {
    _bidRepositoryMock = new Mock<IBidRepository>();
    _houseRepositoryMock = new Mock<IHouseRepository>();
    _mapperMock = new Mock<IMapper>();
    _contextAccessorMock = new Mock<IHttpContextAccessor>();
    _userBidService = new UserBidService(
      _bidRepositoryMock.Object,
      _houseRepositoryMock.Object,
      _mapperMock.Object,
      _contextAccessorMock.Object
    );
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenHouseNotFound() {
    // Arrange
    var createDto = new UserBidCreateDto(1, 100000);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync((House?)null);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _userBidService.Create(createDto));
    Assert.Equal(StatusCodes.Status404NotFound, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenAmountIsLessThanOrEqualToHighestBid() {
    // Arrange
    var createDto = new UserBidCreateDto(1, 50000);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForAuction, Price = 100000 });

    _bidRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Bid, bool>>>()))
      .ReturnsAsync(new List<Bid> { new() { Amount = 50000 } }.AsQueryable());

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _userBidService.Create(createDto));
    Assert.Equal(StatusCodes.Status400BadRequest, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldAddBid_WhenValid() {
    // Arrange
    var createDto = new UserBidCreateDto(1, 150000);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForAuction, Price = 100000 });

    _bidRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Bid, bool>>>()))
      .ReturnsAsync(new List<Bid>().AsQueryable());

    var bid = new Bid {
      HouseId = createDto.HouseId,
      Amount = createDto.Amount
    };

    _mapperMock
      .Setup(mapper => mapper.Map<Bid>(It.IsAny<UserBidCreateDto>()))
      .Returns(bid);

    // Act
    var response = await _userBidService.Create(createDto);

    // Assert
    Assert.Equal(201, response.StatusCode);
    _bidRepositoryMock.Verify(repo => repo.AddAsync(bid), Times.Once);
    _houseRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
    _bidRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
  }
}
