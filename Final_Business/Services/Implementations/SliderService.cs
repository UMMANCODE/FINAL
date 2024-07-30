﻿using AutoMapper;
using Final_Business.DTOs;
using Final_Business.Exceptions;
using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Final_Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Transactions;
using Final_Business.DTOs.General;

namespace Final_Business.Services.Implementations {
  public class SliderService(ISliderRepository sliderRepository, IMapper mapper, IWebHostEnvironment env)
    : ISliderService {
    public async Task<int> Create(SliderCreateDto createDto) {
      string? uploadedFilePath = null;

      if (await sliderRepository.ExistsAsync(x => x.Order == createDto.Order && !x.IsDeleted)) {
        throw new RestException(StatusCodes.Status400BadRequest, "Order already exists");
      }

      using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      try {

        var slider = mapper.Map<Slider>(createDto);

        uploadedFilePath = FileManager.Save(createDto.Image, env.WebRootPath, "images/sliders");
        slider.ImageLink = uploadedFilePath;

        await sliderRepository.AddAsync(slider);

        await sliderRepository.SaveAsync();

        scope.Complete();

        return slider.Id;
      }
      catch {

        if (!string.IsNullOrEmpty(uploadedFilePath)) {
          FileManager.Delete(env.WebRootPath, "images/sliders", uploadedFilePath);
        }

        throw;
      }
    }

    public async Task Delete(int id) {
      var slider = await sliderRepository.GetAsync( x => x.Id == id && !x.IsDeleted)
        ?? throw new RestException(StatusCodes.Status404NotFound, "Slider not found");

      slider.IsDeleted = true;
      slider.UpdatedAt = DateTime.Now;

      await sliderRepository.SaveAsync();
    }

    public async Task<PaginatedList<SliderGetAllDto>> GetPaginated(int pageNumber = 1, int pageSize = 1) {
      if (pageNumber <= 0 || pageSize <= 0) {
        throw new RestException(StatusCodes.Status400BadRequest, "Invalid parameters for paging");
      }

      var sliders = await sliderRepository.GetPaginatedAsync(x => !x.IsDeleted, pageNumber, pageSize);
      var paginated = PaginatedList<Slider>.Create(sliders, pageNumber, pageSize);
      return new PaginatedList<SliderGetAllDto>(mapper.Map<List<SliderGetAllDto>>(paginated.Items), paginated.TotalPages, pageNumber, pageSize);
    }

    public async Task<SliderGetOneDto> GetById(int id) {
      var slider = await sliderRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

      return slider == null
        ? throw new RestException(StatusCodes.Status404NotFound, "Slider not found")
        : mapper.Map<SliderGetOneDto>(slider);
    }

    public async Task<List<SliderGetAllDto>> GetAll() {
      var sliders = await sliderRepository.GetAllAsync(x => !x.IsDeleted);
      return mapper.Map<List<SliderGetAllDto>>(sliders);
    }

    public async Task Update(int id, SliderUpdateDto updateDto) {
      string? newUploadedFilePath = null;

      var slider = await sliderRepository.GetAsync(x => x.Id == id && !x.IsDeleted)
        ?? throw new RestException(StatusCodes.Status404NotFound, "Slider not found");

      if (await sliderRepository.ExistsAsync(x => x.Order == updateDto.Order && x.Id != id && !x.IsDeleted)) {
        throw new RestException(StatusCodes.Status400BadRequest, "Order already exists");
      }

      using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      try {

        mapper.Map(updateDto, slider);

        if (updateDto.Image != null) {
          newUploadedFilePath = FileManager.Save(updateDto.Image, env.WebRootPath, "images/sliders");

          var oldImagePath = slider.ImageLink;
          slider.ImageLink = newUploadedFilePath;
          slider.UpdatedAt = DateTime.Now;

          await sliderRepository.SaveAsync();

          if (!string.IsNullOrEmpty(oldImagePath)) {
            FileManager.Delete(env.WebRootPath, "images/sliders", oldImagePath);
          }
        }
        else {
          slider.UpdatedAt = DateTime.Now;
          slider.ImageLink = FileManager.DefaultImage;

          await sliderRepository.SaveAsync();
        }

        scope.Complete();
      }
      catch {

        if (!string.IsNullOrEmpty(newUploadedFilePath)) {
          FileManager.Delete(env.WebRootPath, "images/sliders", newUploadedFilePath);
        }

        throw;
      }
    }
  }
}
