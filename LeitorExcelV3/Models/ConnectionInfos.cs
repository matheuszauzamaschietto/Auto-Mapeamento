using LeitorExcelV3.Enums;
using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public class ConnectionInfos
{
    public Dictionary<string, string> ClientHeaders { get; }
    public HttpMethod HttpMethod { get; set; }
    public string PloomesUrl { get; }
    public string Url { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> PloomesHeader { get; set; }
    public ConnectionInfos(ExcelWorksheets worksheets)
    {
        PloomesUrl = @"https://api2.ploomes.com/Fields?$expand=OptionsTable($expand=Options),Type";
        ExcelWorksheet deafultConnectionWorkSheet = worksheets.FirstOrDefault(wk => wk.Name == WorksheetDefaultName.Autentication);
        ClientHeaders = GetHeaders(deafultConnectionWorkSheet);
        PloomesHeader = GetPloomesHeader(deafultConnectionWorkSheet);
    }

    private Dictionary<string, string> GetPloomesHeader(ExcelWorksheet deafultConnectionWorkSheet)
    {
        Dictionary<string, string> ploomesHeader = new Dictionary<string, string>() { { "User-Key", deafultConnectionWorkSheet.Cells[CellDefaultPositions.USER_KEY_CELL].GetCellValue<string>() } };
        return ploomesHeader;
    }

    private Dictionary<string, string> GetHeaders(ExcelWorksheet deafultConnectionWorkSheet)
    {
        int cont = 8;
        Dictionary<string, string> headers = new();
        while (deafultConnectionWorkSheet.Cells["B" + (cont + 1)].GetCellValue<string>() is not null || deafultConnectionWorkSheet.Cells["D" + (cont + 1)].GetCellValue<string>() is not null)
        {
            cont++;
            headers.Add(deafultConnectionWorkSheet.Cells["B" + cont].GetCellValue<string>(),
                        deafultConnectionWorkSheet.Cells["D" + cont].GetCellValue<string>());
        }
        return headers;
    }

    public void SetConnectionInfo(ExcelWorksheet connectionWorksheet)
    {
        Body = GetBody(connectionWorksheet);
        HttpMethod = GetHttpMethod(connectionWorksheet);
        Url = GetUrl(connectionWorksheet);
    }

    private string GetBody(ExcelWorksheet connectionWorkSheet)
    {
        return connectionWorkSheet.Cells[CellDefaultPositions.BODY_CELL].GetCellValue<string>();
    }

    private HttpMethod? GetHttpMethod(ExcelWorksheet connectionWorkSheet)
    {
        string sheetMethod = connectionWorkSheet.Cells[CellDefaultPositions.METHOD_CELL].GetCellValue<string>();
        switch (sheetMethod?.ToLower())
        {
            case ("post"): return HttpMethod.Post;
            case ("delete"): return HttpMethod.Delete;
            case ("get"): return HttpMethod.Get;
            case ("put"): return HttpMethod.Put;
            default: return null;
        }
    }

    public string GetUrl(ExcelWorksheet connectionWorkSheet)
    {
        return connectionWorkSheet.Cells[CellDefaultPositions.URL_CELL].GetCellValue<string>();
    }
}
