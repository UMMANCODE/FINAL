namespace Final_Business.Services.Implementations;

public class AdminHouseService(
  IHouseRepository houseRepository,
  IFeatureRepository featureRepository,
  IHouseFeatureRepository houseFeatureRepository,
  IMapper mapper,
  IWebHostEnvironment env,
  IHouseImageRepository houseImageRepository)
  : IAdminHouseService {

  public async Task<BaseResponse> Create(AdminHouseCreateDto createDto) {
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
          house.HouseImages.Add(houseImage);
        }
      }
      else {
        house.HouseImages.Add(new HouseImage { ImageLink = FileManager.DefaultImage, IsMain = true });
      }

      if (createDto.SelectedFeatures.Count > 0) {
        foreach (var featureId in createDto.SelectedFeatures) {
          var feature = await featureRepository.GetAsync(f => f.Id == featureId);
          if (feature != null) {
            house.Features.Add(new HouseFeature { Feature = feature });
          }
        }
      }

      await houseRepository.AddAsync(house);
      await houseRepository.SaveAsync();

      scope.Complete();

      return new BaseResponse(201, "Created successfully!", mapper.Map<AdminHouseGetOneDto>(house), []);
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

    return new BaseResponse(204, "Deleted successfully!", null, []);
  }

  public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    if (pageNumber <= 0 || pageSize <= 0) {
      throw new RestException(StatusCodes.Status400BadRequest, "Invalid parameters for paging");
    }

    var houses = await houseRepository.GetPaginatedAsync(x => !x.IsDeleted, pageNumber, pageSize, "HouseImages");
    var paginated = PaginatedList<House>.Create(houses, pageNumber, pageSize);
    var data = new PaginatedList<AdminHouseGetAllDto>(mapper.Map<List<AdminHouseGetAllDto>>(paginated.Items), paginated.TotalPages, pageNumber, pageSize);

    return new BaseResponse(200, "Success", data, []);
  }

  public async Task<BaseResponse> GetById(int id) {
    var house = await houseRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "HouseImages", "Discounts", "Comments", "Bids", "Orders", "Features");

    if (house != null) {
      return new BaseResponse(200, "Success", mapper.Map<AdminHouseGetOneDto>(house), []);
    }

    throw new RestException(StatusCodes.Status404NotFound, "House not found");
  }

  public async Task<BaseResponse> GetAll() {
    var houses = await houseRepository.GetAllAsync(x => !x.IsDeleted, "Discounts");
    return new BaseResponse(200, "Success", mapper.Map<List<AdminHouseGetAllDto>>(houses), []);
  }

  public async Task<BaseResponse> Update(int id, AdminHouseUpdateDto updateDto) {
    var uploadedNewFiles = new List<string>();

    var house = await houseRepository.GetAsync(x => x.Id == id && !x.IsDeleted, "HouseImages", "Features")
        ?? throw new RestException(StatusCodes.Status404NotFound, "House not found");

    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

    try {
      mapper.Map(updateDto, house);

      if (updateDto.IdsToDelete is null)
        updateDto = updateDto with { IdsToDelete = [] };

      var newImages = new List<HouseImage>();
      var allImages = house.HouseImages.Where(image => !updateDto.IdsToDelete.Contains(image.Id)).ToList();
      var deletedImages = house.HouseImages.Where(image => updateDto.IdsToDelete.Contains(image.Id)).ToList();

      if (updateDto.Images != null && updateDto.Images.Count != 0) {
        foreach (var image in updateDto.Images) {
          var filePath = FileManager.Save(image, env.WebRootPath, "images/houses");
          uploadedNewFiles.Add(filePath);

          var houseImage = new HouseImage { ImageLink = filePath };
          if (image == updateDto.Images.First())
            houseImage.IsMain = true;

          newImages.Add(houseImage);
        }

        var defaultImage = allImages.FirstOrDefault(img => img.ImageLink == FileManager.DefaultImage);
        if (defaultImage != null)
          allImages.Remove(defaultImage);

        allImages.AddRange(newImages);

        // Set the first image as main if there are no images
        if (allImages.Count == 0) {
          allImages.Add(new HouseImage { ImageLink = FileManager.DefaultImage, IsMain = true });
        }
        else {
          // Ensure one image is marked as main
          if (!allImages.Any(img => img.IsMain)) {
            allImages.First().IsMain = true;
          }
        }

        house.HouseImages = allImages;
      }
      else {
        // Handle case where no new images are provided
        if (allImages.Count == 0) {
          allImages.Add(new HouseImage { ImageLink = FileManager.DefaultImage, IsMain = true });
        }
        else {
          // Ensure one image is marked as main
          if (!allImages.Any(img => img.IsMain)) {
            allImages.First().IsMain = true;
          }
        }

        house.HouseImages = allImages;
      }

      // Handling feature updates
      var selectedFeatureIds = updateDto.SelectedFeatures;
      var allFeatureIds = house.Features.Select(f => f.FeatureId).ToList();

      // Features to remove
      var featuresToRemove = house.Features
        .Where(f => !selectedFeatureIds.Contains(f.FeatureId))
        .ToList();

      // Features to add
      var featuresToAdd = selectedFeatureIds
        .Where(item => !allFeatureIds.Contains(item))
        .Select(featureId => new HouseFeature { FeatureId = featureId })
        .ToList();

      // Remove features
      foreach (var featureToRemove in featuresToRemove) {
        house.Features.Remove(featureToRemove);
        await houseFeatureRepository.DeleteAsync(featureToRemove);
      }

      // Add features
      foreach (var featureToAdd in featuresToAdd) {
        house.Features.Add(featureToAdd);
        await houseFeatureRepository.AddAsync(featureToAdd);
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

      return new BaseResponse(204, "Updated successfully!", null, []);
    }
    catch {
      foreach (var filePath in uploadedNewFiles) {
        FileManager.Delete(env.WebRootPath, "images/houses", filePath);
      }

      throw;
    }
  }
}