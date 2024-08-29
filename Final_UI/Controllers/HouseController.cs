﻿using Microsoft.AspNetCore.Mvc.Rendering;

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

  public async Task<IActionResult> Create() {
    ViewBag.Features = await PopulateFeatures();
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([FromForm] HouseCreateRequest createRequest) {
    if (!ModelState.IsValid) {
      ViewBag.Features = await PopulateFeatures();
      return View(createRequest);
    }

    if (createRequest.Images!.Any(file => !UploadExtension.IsValidImage(file))) {
      ModelState.AddModelError("Images", "Not a valid image type!");
      ViewBag.Features = await PopulateFeatures();
      return View(createRequest);
    }

    if (createRequest.SelectedFeatures.Count != 0) {
      createRequest.SelectedFeatures = createRequest.SelectedFeatures;
    }

    await crudService.CreateAsync<HouseCreateRequest, HouseResponse>(createRequest, $"{_apiUrl}/Houses/admin");

    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id) {
    var house = await crudService.GetAsync<HouseResponse>($"{_apiUrl}/Houses/admin/{id}");

    if (house == null)
      return RedirectToAction("Index");

    var request = mapper.Map<HouseUpdateRequest>(house);

    ViewBag.Features = await PopulateFeatures();
    ViewBag.SelectedFeatures = house.Features;

    ViewBag.Images = house.HouseImages;
    ViewBag.Ids = house.HouseImages.Select(x => x.Id).ToList();

    return View(request);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(int id, [FromForm] HouseUpdateRequest editRequest) {
    if (!ModelState.IsValid) {
      var house = await crudService.GetAsync<HouseResponse>($"{_apiUrl}/Houses/admin/{id}");

      if (house == null)
        return RedirectToAction("Index");

      var request = mapper.Map<HouseUpdateRequest>(house);

      ViewBag.Features = await PopulateFeatures();
      ViewBag.SelectedFeatures = house.Features;

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

  private async Task<List<SelectListItem>> PopulateFeatures() {
    var features = await crudService.GetAsync<List<FeatureResponse>>($"{_apiUrl}/Features/admin/all") ?? [];

    return features.Select(x => new SelectListItem {
      Value = x.Id.ToString(),
      Text = x.Name
    }).ToList();
  }
}