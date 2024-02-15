using LeitorExcelV3.Models;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Services;

public class ValidatorClientService : Validator
{
    private readonly string _deserializedRequest;
    public ValidatorClientService(ExcelWorksheet worksheet, WorksheetService worksheetService, string deserializedRequest) : base(worksheet, worksheetService)
    {
        _deserializedRequest = deserializedRequest;
    }

    public override void Execute()
    {
        List<Dictionary<string, string>> fields = WorksheetService.GetWorksheetClientFields(Worksheet);
        foreach(var field in fields)
        {
            ExcelRange cell = Worksheet.Cells[field["cordenate"]];

            if (Regex.IsMatch(_deserializedRequest, field["name"]))
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
