using Final_UI.Helpers.Enums;
using Final_UI.Models.Responses;

namespace Final_UI.Services.Interfaces;

public interface IDataService {
  Task<List<OrderResponse>?> GetOrders();
  Task<List<CommentResponse>?> GetComments();
  Task<bool> UpdateOrderStatus(int id, OrderStatus status);
  Task<bool> UpdateCommentStatus(int id, CommentStatus status);
}
