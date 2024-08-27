namespace Final_API.Controllers;
[Route("api/trait/[controller]")]
[ApiController]
public class DiscountsController(IDiscountService discountService) : ControllerBase {
  [HttpGet]
  [Authorize]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await discountService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("all")]
  [Authorize]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> GetAll() {
    var response = await discountService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("{id:int}")]
  [Authorize]
  // [TypeFilter(typeof(RedisCacheFilter))]
  public async Task<IActionResult> GetById(int id) {
    var response = await discountService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreateDiscount([FromForm] DiscountCreateDto discountCreateDto) {
    var response = await discountService.Create(discountCreateDto);
    return StatusCode(response.StatusCode, response);
  }

  [HttpDelete("{id:int}")]
  [Authorize]
  public async Task<IActionResult> DeleteDiscount(int id) {
    var response = await discountService.Delete(id);
    return StatusCode(response.StatusCode, response);
  }
}
