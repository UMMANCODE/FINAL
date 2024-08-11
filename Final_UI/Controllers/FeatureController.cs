using AutoMapper;
using Final_UI.Helpers.Extensions;
using Final_UI.Helpers.Filters;
using Final_UI.Models.Requests;
using Final_UI.Models.Responses;
using Final_UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
public class FeatureController(ICrudService crudService, IConfiguration configuration, IMapper mapper) : Controller {
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;

  public async Task<IActionResult> Index(int page = 1) {
    var data = await crudService.GetAllPaginatedAsync<FeatureResponse>
      (page, $"{_apiUrl}/Features/admin", new Dictionary<string, string> { { "pageSize", "3" } });

    if (data != null && data.TotalPages != 0 && data.TotalPages < page)
      return RedirectToAction("Index", new { page = data.TotalPages });

    return View(data);
  }


  public Task<IActionResult> Create() {
    return Task.FromResult<IActionResult>(View());
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromForm] FeatureCreateRequest createRequest) {
    if (!ModelState.IsValid) {
      return View(createRequest);
    }

    if (UploadExtension.IsValidImage(createRequest.Icon) == false) {
      ModelState.AddModelError("Image", "Not a valid image type!");
      return View(createRequest);
    }

    await crudService.CreateAsync<FeatureCreateRequest, FeatureResponse>(createRequest, $"{_apiUrl}/Features/admin");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id) {
    var feature = await crudService.GetAsync<FeatureResponse>($"{_apiUrl}/Features/admin/{id}");
    if (feature == null)
      return RedirectToAction("Index");

    var request = mapper.Map<FeatureUpdateRequest>(feature);

    ViewBag.ImageUrl = feature.IconLink!;

    return View(request);
  }

  [HttpPost]
  public async Task<IActionResult> Edit(int id, [FromForm] FeatureUpdateRequest editRequest) {
    if (!ModelState.IsValid) {
      return View(editRequest);
    }

    if (editRequest.Icon is not null && UploadExtension.IsValidImage(editRequest.Icon) == false) {
      ModelState.AddModelError("Image", "Not a valid image type!");
      return View(editRequest);
    }

    await crudService.UpdateAsync<FeatureUpdateRequest, FeatureResponse>(editRequest, $"{_apiUrl}/Features/admin/{id}");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Delete(int id) {
    await crudService.DeleteAsync<FeatureResponse>($"{_apiUrl}/Features/admin/{id}");
    return RedirectToAction("Index");
  }
}
