using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
}
