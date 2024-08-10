using Final_Business.DTOs.General;

namespace Final_Business.Tests;

public class DiscountServiceTests {
  private readonly Mock<IDiscountRepository> _discountRepositoryMock;
  private readonly Mock<IHouseRepository> _houseRepositoryMock;
  private readonly Mock<IMapper> _mapperMock;
  private readonly DiscountService _discountService;

  public DiscountServiceTests() {
    _discountRepositoryMock = new Mock<IDiscountRepository>();
    _houseRepositoryMock = new Mock<IHouseRepository>();
    _mapperMock = new Mock<IMapper>();
    _discountService = new DiscountService(
      _discountRepositoryMock.Object,
      _mapperMock.Object,
      _houseRepositoryMock.Object
    );
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenHouseNotFound() {
    // Arrange
    var createDto = new DiscountCreateDto("TEST", 1, DateTime.Now.AddDays(10), 1);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync((House?)null);

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _discountService.Create(createDto));
    Assert.Equal(StatusCodes.Status404NotFound, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenExpiryDateIsInPast() {
    // Arrange
    var createDto = new DiscountCreateDto("TEST", 1, DateTime.Now.AddDays(-1), 1);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForSale, Price = 100000 });

    _discountRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
      .ReturnsAsync(new List<Discount>().AsQueryable());


    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _discountService.Create(createDto));
    Assert.Equal(StatusCodes.Status400BadRequest, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenDiscountCodeAlreadyExists() {
    // Arrange
    var createDto = new DiscountCreateDto("TEST", 1, DateTime.Now.AddDays(10), 1);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForSale, Price = 100000 });

    _discountRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
      .ReturnsAsync(
        new List<Discount> {
          new() { Code = "TEST", HouseId = 1 }
        }.AsQueryable());

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _discountService.Create(createDto));
    Assert.Equal(StatusCodes.Status400BadRequest, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenActiveDiscountExistsForHouse() {
    // Arrange
    var createDto = new DiscountCreateDto("TEST", 1, DateTime.Now.AddDays(5), 1);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForSale, Price = 100000 });

    _discountRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
      .ReturnsAsync(
        new List<Discount> {
        new() { HouseId = 1, ExpiryDate = DateTime.Now.AddDays(10) }
      }.AsQueryable());

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _discountService.Create(createDto));
    Assert.Equal(StatusCodes.Status400BadRequest, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldThrowException_WhenDiscountAmountIsGreaterThanHousePrice() {
    // Arrange
    var createDto = new DiscountCreateDto("TEST", 150000, DateTime.Now.AddDays(10), 1);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForSale, Price = 100000 });

    _discountRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
      .ReturnsAsync(new List<Discount>().AsQueryable());

    // Act & Assert
    var exception = await Assert.ThrowsAsync<RestException>(() => _discountService.Create(createDto));
    Assert.Equal(StatusCodes.Status400BadRequest, exception.Code);
  }

  [Fact]
  public async Task Create_ShouldAddDiscount_WhenValid() {
    // Arrange
    var createDto = new DiscountCreateDto("TEST", 50000, DateTime.Now.AddDays(5), 1);

    _houseRepositoryMock
      .Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<House, bool>>>()))
      .ReturnsAsync(new House { Id = 1, Status = PropertyStatus.ForSale, Price = 100000 });

    _discountRepositoryMock
      .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
      .ReturnsAsync(new List<Discount>().AsQueryable());

    var discount = new Discount {
      Code = createDto.Code,
      Amount = createDto.Amount,
      ExpiryDate = createDto.ExpiryDate,
      HouseId = createDto.HouseId
    };

    _mapperMock
      .Setup(mapper => mapper.Map<Discount>(It.IsAny<DiscountCreateDto>()))
      .Returns(discount);

    // Act
    var response = await _discountService.Create(createDto);

    // Assert
    Assert.Equal(201, response.StatusCode);
    _discountRepositoryMock.Verify(repo => repo.AddAsync(discount), Times.Once);
    _discountRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
  }
}