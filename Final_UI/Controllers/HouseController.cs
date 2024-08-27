namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
[ServiceFilter(typeof(AdminOrSuperAdminFilter))]
public class HouseController(ICrudService crudService, IConfiguration configuration, IMapper mapper) : Controller {
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;

  public async Task<IActionResult> Index(int page = 1) {
    var data = await crudService.GetAllPaginatedAsync<HouseResponse>
      (page, $"{_apiUrl}/Houses/admin", new Dictionary<string, string> { { "pageSize", "3" } });

    if (data == null)
      return RedirectToAction("Index");

    if (data.TotalPages != 0 && data.TotalPages < page)
      return RedirectToAction("Index", new { page = data.TotalPages });

    ViewBag.Images = data.Items.Select(x => x.HouseImages.FirstOrDefault(x => x.IsMain)).ToList();

    return View(data);
  }

  public Task<IActionResult> Create() {
    return Task.FromResult<IActionResult>(View());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromForm] HouseCreateRequest createRequest) {
    if (!ModelState.IsValid) {
      return View(createRequest);
    }

    if (createRequest.Images!.Any(file => UploadExtension.IsValidImage(file) == false)) {
      ModelState.AddModelError("Images", "Not a valid image type!");
      return View(createRequest);
    }

    await crudService.CreateAsync<HouseCreateRequest, HouseResponse>(createRequest, $"{_apiUrl}/Houses/admin");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id) {
    var house = await crudService.GetAsync<HouseResponse>($"{_apiUrl}/Houses/admin/{id}");

    if (house == null)
      return RedirectToAction("Index");

    var request = mapper.Map<HouseUpdateRequest>(house);

    ViewBag.Images = house.HouseImages;
    ViewBag.Ids = house.HouseImages.Select(x => x.Id).ToList();

    return View(request);
  }

  [HttpPost]
  public async Task<IActionResult> Edit(int id, [FromForm] HouseUpdateRequest editRequest) {
    if (!ModelState.IsValid) {
      var house = await crudService.GetAsync<HouseResponse>($"{_apiUrl}/Houses/admin/{id}");

      if (house == null)
        return RedirectToAction("Index");

      var request = mapper.Map<HouseUpdateRequest>(house);

      ViewBag.Images = house.HouseImages;
      ViewBag.Ids = house.HouseImages.Select(x => x.Id).ToList();

      return View(request);
    }

    if (editRequest.Images!.Any(file => UploadExtension.IsValidImage(file) == false)) {
      ModelState.AddModelError("Images", "Not a valid image type!");
      return View(editRequest);
    }

    await crudService.UpdateAsync<HouseUpdateRequest, HouseResponse>(editRequest, $"{_apiUrl}/Houses/admin/{id}");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Delete(int id) {
    await crudService.DeleteAsync<HouseResponse>($"{_apiUrl}/Houses/admin/{id}");
    return RedirectToAction("Index");
  }
}