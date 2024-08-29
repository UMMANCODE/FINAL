namespace Final_API.Controllers;
[Route("api/trait/[controller]")]
[ApiController]
public class ChartsController(IChartService chartService) : ControllerBase {
  [HttpGet("doughnut")]
  public async Task<IActionResult> GetDoughnutChart() {
    var response = await chartService.GetDoughnutChart();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("line")]
  public async Task<IActionResult> GetLineChart() {
    var response = await chartService.GetLineChart();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("bar")]
  public async Task<IActionResult> GetBarChart() {
    var response = await chartService.GetBarChart();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("pie1")]
  public async Task<IActionResult> GetPieChart1() {
    var response = await chartService.GetPieChart1();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("pie2")]
  public async Task<IActionResult> GetPieChart2() {
    var response = await chartService.GetPieChart2();
    return StatusCode(response.StatusCode, response);
  }
}
