using System.Reflection;
using OfficeOpenXml;

namespace Final_UI.Services.Implementations {
  public class ExcelReportService : IExcelReportService {
    public byte[] GenerateReport(IEnumerable<(object data, string sheetName)> datasets) {
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

      using var package = new ExcelPackage();

      // Loop through each dataset and create a corresponding sheet
      foreach (var (data, sheetName) in datasets) {
        var worksheet = package.Workbook.Worksheets.Add(sheetName);
        PopulateWorksheet(worksheet, data);
      }

      return package.GetAsByteArray();
    }

    private void PopulateWorksheet(ExcelWorksheet worksheet, object data) {
      var dataType = data.GetType();
      var genericArgumentType = dataType.GetGenericArguments().FirstOrDefault();

      if (genericArgumentType == null) return;
      var properties = genericArgumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

      // Generate headers based on the properties of the data type
      for (var i = 0; i < properties.Length; i++) {
        worksheet.Cells[1, i + 1].Value = properties[i].Name;
        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        worksheet.Column(i + 1).AutoFit();
      }

      // Add data rows
      var row = 2;
      foreach (var item in (IEnumerable<object>)data) {
        for (var i = 0; i < properties.Length; i++) {
          var value = properties[i].GetValue(item);
          if (value is DateTime dateTimeValue) {
            worksheet.Cells[row, i + 1].Value = dateTimeValue.ToString("yyyy-MM-dd");
          }
          else if (value is decimal or double) {
            worksheet.Cells[row, i + 1].Style.Numberformat.Format = "#,##0.00";
            worksheet.Cells[row, i + 1].Value = value;
          }
          else {
            worksheet.Cells[row, i + 1].Value = value?.ToString();
          }
        }
        row++;
      }
    }
  }
}