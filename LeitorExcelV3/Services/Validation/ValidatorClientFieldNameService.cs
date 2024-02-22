using LeitorExcelV3.Models;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Services.Validatiion;

public class ValidatorClientFieldNameService : Validator
{
    private readonly string _deserializedRequest;
    public ValidatorClientFieldNameService(ExcelWorksheet worksheet, WorksheetService worksheetService, string deserializedRequest) : base(worksheet, worksheetService)
    {
        _deserializedRequest = deserializedRequest;
    }

    public override void Execute()
    {
        List<Dictionary<string, string>> fields = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "A", 7);
        foreach (var field in fields)
        {
            ExcelRange cell = Worksheet.Cells[field["cordenate"]];

            if (Regex.IsMatch(_deserializedRequest, $"\"{field["name"]}\""))
            {
                WorksheetService.SetCellAsFind(cell);
            }
            else
            {
                WorksheetService.SetCellAsNotFind(cell);
            }
        }
        NextValidator?.Execute();
    }
}
