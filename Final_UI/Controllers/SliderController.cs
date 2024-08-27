namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
[ServiceFilter(typeof(AdminOrSuperAdminFilter))]
public class SliderController(ICrudService crudService, IConfiguration configuration, IMapper mapper)  : Controller {
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;

  public async Task<IActionResult> Index(int page = 1) {
    var data = await crudService.GetAllPaginatedAsync<SliderResponse>
      (page, $"{_apiUrl}/Sliders/admin", new Dictionary<string, string> { { "pageSize", "3" } });

    if (data != null && data.TotalPages != 0 && data.TotalPages < page)
      return RedirectToAction("Index", new { page = data.TotalPages });

    return View(data);
  }


  public Task<IActionResult> Create() {
    return Task.FromResult<IActionResult>(View());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromForm] SliderCreateRequest createRequest) {
    if (!ModelState.IsValid) {
      return View(createRequest);
    }

    if (UploadExtension.IsValidImage(createRequest.Image) == false) {
      ModelState.AddModelError("Image", "Not a valid image type!");
      return View(createRequest);
    }

    await crudService.CreateAsync<SliderCreateRequest, SliderResponse>(createRequest, $"{_apiUrl}/Sliders/admin");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id) {
    var slider = await crudService.GetAsync<SliderResponse>($"{_apiUrl}/Sliders/admin/{id}");
    if (slider == null)
      return RedirectToAction("Index");

    var request = mapper.Map<SliderUpdateRequest>(slider);

    ViewBag.ImageUrl = slider.ImageLink;

    return View(request);
  }

  [HttpPost]
  public async Task<IActionResult> Edit(int id, [FromForm] SliderUpdateRequest editRequest) {
    if (!ModelState.IsValid) {
      return View(editRequest);
    }

    if (editRequest.Image is not null && UploadExtension.IsValidImage(editRequest.Image) == false) {
      ModelState.AddModelError("Image", "Not a valid image type!");
      return View(editRequest);
    }

    await crudService.UpdateAsync<SliderUpdateRequest, SliderResponse>(editRequest, $"{_apiUrl}/Sliders/admin/{id}");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Delete(int id) {
    await crudService.DeleteAsync<SliderResponse>($"{_apiUrl}/Sliders/admin/{id}");
    return RedirectToAction("Index");
  }
}

