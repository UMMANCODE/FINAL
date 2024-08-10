﻿using Final_Business.DTOs.User;
using Final_Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_API.Controllers;
[Route("api/trait/[controller]")]
[ApiController]
public class BidsController(IUserBidService bidService) : ControllerBase {
  [HttpGet]
  public async Task<IActionResult> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var response = await bidService.GetPaginated(pageNumber, pageSize);
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("all")]
  public async Task<IActionResult> GetAll() {
    var response = await bidService.GetAll();
    return StatusCode(response.StatusCode, response);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id) {
    var response = await bidService.GetById(id);
    return StatusCode(response.StatusCode, response);
  }

  [HttpPost]
  public async Task<IActionResult> CreateBid([FromForm] UserBidCreateDto bidCreateDto) {
    var response = await bidService.Create(bidCreateDto);
    return StatusCode(response.StatusCode, response);
  }
}
