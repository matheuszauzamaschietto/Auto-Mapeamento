using LeitorExcelV3.Enums;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Models;

public class PloomesConnection
{
    public string Url { get; private set; }
    public Dictionary<string, string> Headers { get; private set; }
    public int? EntityId { get; private set; }
    public PloomesConnection(ExcelWorksheet deafultConnectionWorkSheet)
    {
        Headers = new Dictionary<string, string>() { { "User-Key", deafultConnectionWorkSheet.Cells[CellDefaultPositions.USER_KEY_CELL].GetValue<string>() } };
    }

    public void SetEntity(ExcelWorksheet worksheet)
    {
        string entityFullName = worksheet.Cells["Z5"].GetValue<string>();
        EntityId = GetCellTypeId(entityFullName ?? string.Empty);
        Url = $"https://api2.ploomes.com/Fields?$expand=OptionsTable($expand=Options),Type&$filter=EntityId+eq+{EntityId}";
    }

    private int GetCellTypeId(string text)
    {
        int entityId = 0;
        int.TryParse(Regex.Match(text, @"(?<=\[)\d{1,}(?=\])").Value, out entityId);
        return entityId;
    }
}
