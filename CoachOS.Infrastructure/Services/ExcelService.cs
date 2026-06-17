using ClosedXML.Excel;
using CoachOS.Application.Interfaces.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoachOS.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            var dataList = data.ToList();
            for (int row = 0; row < dataList.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(dataList[row]);
                    worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? string.Empty;
                }
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public IEnumerable<T> ImportFromExcel<T>(Stream fileStream) where T : new()
        {
            var result = new List<T>();
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheets.First();

            var properties = typeof(T).GetProperties();
            var headerRow = worksheet.Row(1);

            var propertyMap = new Dictionary<int, System.Reflection.PropertyInfo>();
            for (int col = 1; col <= worksheet.LastCellUsed().Address.ColumnNumber; col++)
            {
                var headerValue = headerRow.Cell(col).GetString();
                var prop = properties.FirstOrDefault(p => p.Name.Equals(headerValue, System.StringComparison.OrdinalIgnoreCase));
                if (prop != null)
                {
                    propertyMap[col] = prop;
                }
            }

            for (int row = 2; row <= worksheet.LastRowUsed().RowNumber(); row++)
            {
                var item = new T();
                foreach (var map in propertyMap)
                {
                    var cellValue = worksheet.Cell(row, map.Key).GetString();
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        var targetType = map.Value.PropertyType;
                        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(System.Nullable<>))
                        {
                            targetType = System.Nullable.GetUnderlyingType(targetType);
                        }
                        
                        var convertedValue = System.Convert.ChangeType(cellValue, targetType);
                        map.Value.SetValue(item, convertedValue);
                    }
                }
                result.Add(item);
            }

            return result;
        }
    }
}
