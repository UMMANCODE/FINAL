namespace Final_UI.Services.Interfaces;

public interface IExcelReportService {
  public byte[] GenerateReport(IEnumerable<(object data, string sheetName)> datasets);
}
