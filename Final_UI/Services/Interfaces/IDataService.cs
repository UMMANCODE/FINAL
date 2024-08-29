using Final_UI.Helpers;

namespace Final_UI.Services.Interfaces;

public interface IDataService {
  Task<AppUserResponse?> GetProfile();
  Task<List<OrderResponse>?> GetOrders();
  Task<List<OrderStatistic>> GetOrderStatistics();
  Task<List<CommentResponse>?> GetComments();
  Task<bool> UpdateOrderStatus(int id, OrderStatus status);
  Task<bool> UpdateCommentStatus(int id, CommentStatus status);
}
