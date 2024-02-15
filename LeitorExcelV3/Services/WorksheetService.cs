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
    /* PENSA EM UM MODO DE REFATORAR OS DOIS METODOS A BAIXO PARA QUE FIQUEM GENERICOS */
    public List<Dictionary<string, string>> GetWorksheetClientFields(ExcelWorksheet worksheet)
    {
        const string column = "A";
        List<Dictionary<string, string>> sheetFields = new();
        int row = 7;
        string? cellValue = string.Empty;
        do
        {
            cellValue = worksheet.Cells[column + row].GetValue<string>() ?? string.Empty;
            if (cellValue != string.Empty && cellValue != null)
                sheetFields.Add(new Dictionary<string, string>() { { "name", cellValue }, { "cordenate", column + row } });
            row++;
        } while (cellValue != string.Empty);
        return sheetFields;
    }

    public List<Dictionary<string, string>> GetWorksheetPloomesFields(ExcelWorksheet worksheet)
    {
        const string column = "F";
        List<Dictionary<string, string>> sheetFields = new();
        int row = 7;
        string? cellValue = string.Empty;
        do
        {
            cellValue = worksheet.Cells[column + row].GetValue<string>() ?? string.Empty;
            if (cellValue != string.Empty && cellValue != null)
                sheetFields.Add(new Dictionary<string, string>() { { "name", $"\"{cellValue}\"" }, { "cordenate", column + row } });
            row++;
        } while (cellValue != string.Empty);
        return sheetFields;
    }
}
