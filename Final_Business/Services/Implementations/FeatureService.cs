using AutoMapper;
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
  public class FeatureService(IFeatureRepository featureRepository, IMapper mapper, IWebHostEnvironment env)
    : IFeatureService {
    public async Task<BaseResponse> Create(FeatureCreateDto createDto) {
      string? uploadedFilePath = null;

      using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      try {

        var feature = mapper.Map<Feature>(createDto);

        uploadedFilePath = FileManager.Save(createDto.Icon, env.WebRootPath, "images/features");
        feature.IconLink = uploadedFilePath;

        await featureRepository.AddAsync(feature);

        await featureRepository.SaveAsync();

        scope.Complete();

        return new BaseResponse(201, "Created successfully", mapper.Map<FeatureGetOneDto>(feature), []);
      }
      catch {

        if (!string.IsNullOrEmpty(uploadedFilePath)) {
          FileManager.Delete(env.WebRootPath, "images/features", uploadedFilePath);
        }

        throw;
      }
    }

    public async Task<BaseResponse> Delete(int id) {
      var feature = await featureRepository.GetAsync(x => x.Id == id && !x.IsDeleted)
        ?? throw new RestException(StatusCodes.Status404NotFound, "Feature not found");

      feature.IsDeleted = true;
      feature.UpdatedAt = DateTime.Now;

      await featureRepository.SaveAsync();

      return new BaseResponse(204, "Deleted successfully", null, []);
    }

    public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
      if (pageNumber <= 0 || pageSize <= 0) {
        throw new RestException(StatusCodes.Status400BadRequest, "Invalid parameters for paging");
      }

      var features = await featureRepository.GetPaginatedAsync(x => !x.IsDeleted, pageNumber, pageSize);
      var paginated = PaginatedList<Feature>.Create(features, pageNumber, pageSize);
      var data =  new PaginatedList<FeatureGetAllDto>(mapper.Map<List<FeatureGetAllDto>>(paginated.Items), paginated.TotalPages, pageNumber, pageSize);

      return new BaseResponse(200, "Success", data, []);
    }

    public async Task<BaseResponse> GetById(int id) {
      var feature = await featureRepository.GetAsync(x => x.Id == id && !x.IsDeleted);

      return feature == null
        ? throw new RestException(StatusCodes.Status404NotFound, "Feature not found")
        : new BaseResponse(200, "Success", mapper.Map<FeatureGetOneDto>(feature), []);
    }

    public async Task<BaseResponse> GetAll() {
      var features = await featureRepository.GetAllAsync(x => !x.IsDeleted);

      return new BaseResponse(200, "Success", mapper.Map<List<FeatureGetAllDto>>(features), []);
    }

    public async Task<BaseResponse> Update(int id, FeatureUpdateDto updateDto) {
      string? newUploadedFilePath = null;

      var feature = await featureRepository.GetAsync(x => x.Id == id && !x.IsDeleted)
        ?? throw new RestException(StatusCodes.Status404NotFound, "Feature not found");

      using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
      try {

        mapper.Map(updateDto, feature);

        if (updateDto.Icon != null) {
          newUploadedFilePath = FileManager.Save(updateDto.Icon, env.WebRootPath, "images/features");

          var oldIconPath = feature.IconLink;
          feature.IconLink = newUploadedFilePath;
          feature.UpdatedAt = DateTime.Now;

          await featureRepository.SaveAsync();

          if (!string.IsNullOrEmpty(oldIconPath)) {
            FileManager.Delete(env.WebRootPath, "images/features", oldIconPath);
          }
        }
        else {
          feature.UpdatedAt = DateTime.Now;
          feature.IconLink = FileManager.DefaultImage;

          await featureRepository.SaveAsync();
        }

        scope.Complete();

        return new BaseResponse(204, "Updated successfully", null, []);
      }
      catch {

        if (!string.IsNullOrEmpty(newUploadedFilePath)) {
          FileManager.Delete(env.WebRootPath, "images/features", newUploadedFilePath);
        }

        throw;
      }
    }
  }
}
