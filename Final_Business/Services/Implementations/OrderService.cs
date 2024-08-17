using AutoMapper;
using Final_Business.DTOs;
using Final_Business.DTOs.General;
using Final_Business.Exceptions;
using Final_Business.Helpers;
using Final_Business.Services.Interfaces;
using Final_Core.Entities;
using Final_Core.Enums;
using Final_Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Final_Business.Services.Implementations;
public class OrderService(IOrderRepository orderRepository, IMapper mapper)
  : IOrderService {
  public async Task<BaseResponse> Create(OrderCreateDto createDto) {
    var order = mapper.Map<Order>(createDto);
    await orderRepository.AddAsync(order);
    await orderRepository.SaveAsync();

    return new BaseResponse(201, "Created successfully!", mapper.Map<OrderGetDto>(order), []);
  }

  public async Task<BaseResponse> GetPaginated(int pageNumber = 1, int pageSize = 1) {
    var orders = await orderRepository.GetPaginatedAsync(x => true, pageNumber, pageSize, "AppUser", "House");
    var paginated = PaginatedList<Order>.Create(orders, pageNumber, pageSize);

    var data = new PaginatedList<OrderGetDto>(
      mapper.Map<List<OrderGetDto>>(paginated.Items),
      paginated.TotalPages,
      pageNumber,
      pageSize
    );

    return new BaseResponse(200, "Success", data, []);
  }

  public async Task<BaseResponse> GetAll() {
    var orders = await orderRepository.GetAllAsync(x => true, "AppUser", "House");

    return new BaseResponse(200, "Success", mapper.Map<List<OrderGetDto>>(orders), []);
  }

  public async Task<BaseResponse> GetById(int id) {
    var order = await orderRepository.GetAsync(x => x.Id == id, "AppUser", "House");

    return order == null
      ? throw new RestException(StatusCodes.Status404NotFound, "Order not found")
      : new BaseResponse(200, "Success", mapper.Map<OrderGetDto>(order), []);
  }

  public async Task<BaseResponse> UpdateStatus(int id, OrderStatus status) {
    var order = await orderRepository.GetAsync(x => x.Id == id)
      ?? throw new RestException(StatusCodes.Status404NotFound, "Order not found");

    if (order.Status != OrderStatus.Pending)
      return new BaseResponse(400, "Order status can only be updated if it's pending", null, []);

    order.Status = status;
    await orderRepository.SaveAsync();

    return new BaseResponse(204, "Updated successfully!", null, []);
  }
}