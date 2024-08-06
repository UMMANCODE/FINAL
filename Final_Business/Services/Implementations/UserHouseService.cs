using AutoMapper;
using Final_Business.DTOs.User;
using Final_Business.DTOs;
using Final_Business.Exceptions;
using Final_Business.Helpers;
using Final_Core.Entities;
using Final_Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Transactions;
using Final_Business.Services.Interfaces;

namespace Final_Business.Services.Implementations {
  public class UserHouseService(
    IHouseRepository houseRepository,
    IMapper mapper,
    IWebHostEnvironment env,
    IHouseImageRepository houseImageRepository)
    : IUserHouseService {

    public async Task<BaseResponse> Create(UserHouseCreateDto createDto) {
      var uploadedFiles = new List<string>();

      using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

      try {

        var house = mapper.Map<House>(createDto);

        if (createDto.Images.Count != 0) {
          foreach (var image in createDto.Images) {
            var filePath = FileManager.Save(image, env.WebRootPath, "images/houses");
            uploadedFiles.Add(filePath);

            var houseImage = new HouseImage { ImageLink = filePath };
            if (image == createDto.Images.First()) {
              houseImage.IsMain = true;
            }
            house.Images.Add(houseImage);
          }
        }
        else {
          house.Images.Add(new HouseImage { ImageLink = FileManager.DefaultImage, IsMain = true });
        }

        await houseRepository.AddAsync(house);

        await houseRepository.SaveAsync();

        scope.Complete();

        return new BaseResponse(201, "Created successfully", mapper.Map<UserHouseGetOneDto>(house), []);
      }
      catch {

        foreach (var filePath in uploadedFiles) {
          FileManager.Delete(env.WebRootPath, "images/houses", filePath);
        }

        throw;
      }
    }

    public async Task<BaseResponse> Delete(int id) {
      var house = await houseRepository.GetAsync(x => x.Id == id && !x.IsDeleted)
        ?? throw new RestException(StatusCodes.Status404NotFound, "House not found");

      house.IsDeleted = true;
      house.UpdatedAt = DateTime.Now;

      await houseRepository.SaveAsync();

      return new BaseResponse(204, "Deleted successfully", null, []);
    }

    public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
      if (pageNumber <= 0 || pageSize <= 0) {
        throw new RestException(StatusCodes.Status400BadRequest, "Invalid parameters for paging");
      }

      var houses = await houseRepository.GetPaginatedAsync(x => !x.IsDeleted, pageNumber, pageSize, "Images");
      var paginated = PaginatedList<House>.Create(houses, pageNumber, pageSize);
      var data = new PaginatedList<UserHouseGetAllDto>(mapper.Map<List<UserHouseGetAllDto>>(paginated.Items), paginated.TotalPages, pageNumber, pageSize);

      return new BaseResponse(200, "Success", data, []);
    }

    public async Task<BaseResponse> GetById(int id) {
      var house = await houseRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "Images");

      return house == null
        ? throw new RestException(StatusCodes.Status404NotFound, "House not found")
        : new BaseResponse(200, "Success", mapper.Map<UserHouseGetOneDto>(house), []);
    }

    public async Task<BaseResponse> GetAll() {
      var houses = await houseRepository.GetAllAsync(x => !x.IsDeleted);
      return new BaseResponse(200, "Success", mapper.Map<List<UserHouseGetAllDto>>(houses), []);
    }

    public async Task<BaseResponse> Update(int id, UserHouseUpdateDto updateDto) {
      var uploadedNewFiles = new List<string>();

      var house = await houseRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "Images")
          ?? throw new RestException(StatusCodes.Status404NotFound, "House not found");

      using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

      try {
        mapper.Map(updateDto, house);

        var newImages = new List<HouseImage>();
        var allImages = new List<HouseImage>(house.Images.Where(image => updateDto.IdsToDelete != null && !updateDto.IdsToDelete.Contains(image.Id)));
        var deletedImages = house.Images.Where(image => updateDto.IdsToDelete != null && updateDto.IdsToDelete.Contains(image.Id)).ToList();

        if (updateDto.Images != null && updateDto.Images.Count != 0) {
          foreach (var image in updateDto.Images) {
            var filePath = FileManager.Save(image, env.WebRootPath, "images/houses");
            uploadedNewFiles.Add(filePath);

            var houseImage = new HouseImage { ImageLink = filePath };
            if (image == updateDto.Images.First())
              houseImage.IsMain = true;

            newImages.Add(houseImage);
          }

          allImages.AddRange(newImages);

          house.Images = allImages;
        }
        else {
          if (allImages.Count == 0)
            allImages = [new HouseImage { ImageLink = FileManager.DefaultImage, IsMain = true }];
          else
            allImages.First().IsMain = true;

          house.Images = allImages;
        }

        house.UpdatedAt = DateTime.Now;

        await houseRepository.SaveAsync();

        if (deletedImages.Count != 0) {
          foreach (var imageToDelete in deletedImages) {
            if (imageToDelete.ImageLink == null) continue;
            FileManager.Delete(env.WebRootPath, "images/houses", imageToDelete.ImageLink);
            await houseImageRepository.DeleteAsync(imageToDelete);
          }
        }

        scope.Complete();

        return new BaseResponse(204, "Updated successfully", null, []);
      }
      catch {
        foreach (var filePath in uploadedNewFiles) {
          FileManager.Delete(env.WebRootPath, "images/houses", filePath);
        }

        throw;
      }
    }
  }
}
