using LeitorExcelV3.Enums;
using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public class ConnectionInfos
{
    public PloomesConnection PloomesConnection { get; private set; }
    public ClientConnection ClientConnection { get; set; }

    public ConnectionInfos(ExcelWorksheets worksheets)
    {
        ExcelWorksheet deafultConnectionWorkSheet = worksheets.FirstOrDefault(wk => wk.Name == WorksheetDefaultName.Autentication);
        PloomesConnection = new PloomesConnection(deafultConnectionWorkSheet);
        ClientConnection = new ClientConnection(deafultConnectionWorkSheet);
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
}
