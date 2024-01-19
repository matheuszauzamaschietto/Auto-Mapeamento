using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace LeitorExcelV3.Services;

public class WorksheetService
{
    public WorksheetService()
    {
        
    }

    public void SetCellAsNotFind(ExcelRange cell)
    {
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(color: System.Drawing.Color.Red);
    }

    public void SetCellAsFind(ExcelRange cell)
    {
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(color: System.Drawing.Color.Green);
    }

    public List<Dictionary<string, string>> GetWorksheetClientFields(ExcelWorksheet worksheet)
    {
        List<Dictionary<string, string>> sheetFields = new();
        int cont = 7;
        string? cellValue = string.Empty;
        do
        {
            cellValue = worksheet.Cells["A" + cont].GetValue<string>() ?? string.Empty;
            if (cellValue != string.Empty && cellValue != null)
                sheetFields.Add(new Dictionary<string, string>() { { "name", cellValue }, { "cordenate", "A" + cont } });
            cont++;
        } while (cellValue != string.Empty);
        return sheetFields;
    }
}
