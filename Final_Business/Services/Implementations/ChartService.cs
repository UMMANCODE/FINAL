using Microsoft.EntityFrameworkCore;

namespace Final_Business.Services.Implementations;
public class ChartService(IHouseRepository houseRepository, IOrderRepository orderRepository, ICommentRepository commentRepository) : IChartService {
  public async Task<BaseResponse> GetDoughnutChart() {
    var houses = await houseRepository.GetAllAsync(x => true);
    var result = new Dictionary<string, int> {
      { PropertyStatus.ForSale.ToString(), houses.Count(h => h.Status == PropertyStatus.ForSale) },
      { PropertyStatus.ForAuction.ToString(), houses.Count(h => h.Status == PropertyStatus.ForAuction) },
      { PropertyStatus.Sold.ToString(), houses.Count(h => h.Status == PropertyStatus.Sold) }
    };

    return new BaseResponse(200, "Success", result, []);
  }

  public async Task<BaseResponse> GetLineChart() {
    var data = await orderRepository.GetAllAsync(x => !x.IsDeleted);
    var orders = await data.ToListAsync();
    var result = orders.GroupBy(o => o.CreatedAt.Date.ToString("MMM"))
      .Select(g => new { Month = g.Key, Count = g.Count() })
      .OrderBy(g => DateTime.ParseExact(g.Month, "MMM", null).Month)
      .ToDictionary(g => g.Month, g => g.Count);

    return new BaseResponse(200, "Success", result, []);
  }

  public async Task<BaseResponse> GetBarChart() {
    var result = new Dictionary<string, int> {
      { "Spring", 2 },
      { "Summer", 4 },
      { "Autumn", 6 },
      { "Winter", 2 },
    };

    return await Task.FromResult(new BaseResponse(200, "Success", result, []));
  }

  public async Task<BaseResponse> GetPieChart1() {
    var orders = await orderRepository.GetAllAsync(x => !x.IsDeleted);
    var result = new Dictionary<string, int> {
      { OrderStatus.Rejected.ToString(), orders.Count(o => o.Status == OrderStatus.Rejected) },
      { OrderStatus.Accepted.ToString(), orders.Count(o => o.Status == OrderStatus.Accepted) },
      { OrderStatus.Pending.ToString(), orders.Count(o => o.Status == OrderStatus.Pending) }
    };

    return new BaseResponse(200, "Success", result, []);
  }

  public async Task<BaseResponse> GetPieChart2() {
    var comments = await commentRepository.GetAllAsync(x => true);
    var result = new Dictionary<string, int> {
      { CommentStatus.Rejected.ToString(), comments.Count(c => c.Status == CommentStatus.Rejected) },
      { CommentStatus.Approved.ToString(), comments.Count(c => c.Status == CommentStatus.Approved) },
      { CommentStatus.Pending.ToString(), comments.Count(c => c.Status == CommentStatus.Pending) }
    };

    return new BaseResponse(200, "Success", result, []);
  }
}
