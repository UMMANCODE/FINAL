using AutoMapper;
using Final_UI.Helpers.Filters;
using Final_UI.Models.Requests;
using Final_UI.Models.Responses;
using Final_UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Final_UI.Controllers;

[ServiceFilter(typeof(AuthFilter))]
[ServiceFilter(typeof(AdminOrSuperAdminFilter))]
public class DiscountController(ICrudService crudService, IConfiguration configuration) : Controller {
  private readonly string _apiUrl = configuration.GetSection("APIEndpoint").Value!;

  public async Task<IActionResult> Index(int page = 1) {
    var data = await crudService.GetAllPaginatedAsync<DiscountResponse>
      (page, $"{_apiUrl}/trait/Discounts", new Dictionary<string, string> { { "pageSize", "3" } });

    if (data != null && data.TotalPages != 0 && data.TotalPages < page)
      return RedirectToAction("Index", new { page = data.TotalPages });

    return View(data);
  }


  public async Task<IActionResult> Create() {
    await PopulateHouses();
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromForm] DiscountCreateRequest createRequest) {
    if (!ModelState.IsValid) {
      await PopulateHouses();
      return View(createRequest);
    }

    await crudService.CreateAsync<DiscountCreateRequest, DiscountResponse>(createRequest, $"{_apiUrl}/trait/Discounts");
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Delete(int id) {
    await crudService.DeleteAsync<DiscountResponse>($"{_apiUrl}/trait/Discounts/{id}");
    return RedirectToAction("Index");
  }

  private async Task PopulateHouses() {
    var houses = await crudService.GetAsync<List<HouseResponse>>($"{_apiUrl}/Houses/admin/all");
    ViewBag.Houses = new SelectList(houses, "Id", "Name");
  }
}
