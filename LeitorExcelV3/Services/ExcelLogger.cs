using OfficeOpenXml;
using System.Net;

namespace LeitorExcelV3.Services;

public class ExcelLogger
{
    public void SetClientResponse(ExcelWorksheet worksheet, string body, HttpStatusCode httpCode)
    {
        worksheet.Cells["Z12"].Value = body;
        worksheet.Cells["Z13"].Value = httpCode;
    }
}
