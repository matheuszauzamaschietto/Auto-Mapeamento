using LeitorExcelV3.Enums;
using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public class ClientConnection
{
    public string Url { get; set; }
    public HttpMethod HttpMethod { get; set; }
    public string Body { get; set; }
    public Dictionary<string, string> Headers { get; }
    public ClientConnection(ExcelWorksheet deafultConnectionWorkSheet)
    {
        Headers = GetHeaders(deafultConnectionWorkSheet);
    }

    private Dictionary<string, string> GetHeaders(ExcelWorksheet deafultConnectionWorkSheet)
    {
        int cont = 8;
        Dictionary<string, string> headers = new();
        while (deafultConnectionWorkSheet.Cells["B" + (cont + 1)].GetValue<string>() is not null || deafultConnectionWorkSheet.Cells["D" + (cont + 1)].GetValue<string>() is not null)
        {
            cont++;
            headers.Add(deafultConnectionWorkSheet.Cells["B" + cont].GetValue<string>(),
                        deafultConnectionWorkSheet.Cells["D" + cont].GetValue<string>());
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
        return connectionWorkSheet.Cells[CellDefaultPositions.BODY_CELL].GetValue<string>();
    }

    private HttpMethod? GetHttpMethod(ExcelWorksheet connectionWorkSheet)
    {
        string sheetMethod = connectionWorkSheet.Cells[CellDefaultPositions.METHOD_CELL].GetValue<string>();
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
        return connectionWorkSheet.Cells[CellDefaultPositions.URL_CELL].GetValue<string>();
    }
}
