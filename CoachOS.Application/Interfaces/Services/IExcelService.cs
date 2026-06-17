using System.Collections.Generic;
using System.IO;

namespace CoachOS.Application.Interfaces.Services
{
    public interface IExcelService
    {
        byte[] ExportToExcel<T>(IEnumerable<T> data, string sheetName);
        IEnumerable<T> ImportFromExcel<T>(Stream fileStream) where T : new();
    }
}
