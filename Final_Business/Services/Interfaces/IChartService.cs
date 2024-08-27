namespace Final_Business.Services.Interfaces;
public interface IChartService {
  Task<BaseResponse> GetDoughnutChart();
  Task<BaseResponse> GetLineChart();
  Task<BaseResponse> GetBarChart();
}
