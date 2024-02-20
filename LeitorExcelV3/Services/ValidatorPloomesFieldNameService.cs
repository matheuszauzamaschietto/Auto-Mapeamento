using LeitorExcelV3.Models;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Services;

public class ValidatorPloomesFieldNameService : Validator
{
    private readonly List<PlooFieldsModel> _plooFields;
    public ValidatorPloomesFieldNameService(ExcelWorksheet worksheet ,WorksheetService worksheetService, List<PlooFieldsModel> plooFields) : base(worksheet, worksheetService)
    {
        _plooFields = plooFields;
    }

    public override void Execute()
    {
        List<Dictionary<string, string>> fields = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "F", 7);
        foreach (var field in fields)
        {
            ExcelRange cell = Worksheet.Cells[field["cordenate"]];

            if(_plooFields.FirstOrDefault(f => f.Name == field["name"]) is not null)
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
