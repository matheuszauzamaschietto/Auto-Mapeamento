using LeitorExcelV3.Models;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Services;

public class ValidatorPloomesFieldTypeService : Validator
{
    private readonly List<PlooFieldsModel> _plooFields;
    public ValidatorPloomesFieldTypeService(ExcelWorksheet worksheet ,WorksheetService worksheetService, List<PlooFieldsModel> plooFields) : base(worksheet, worksheetService)
    {
        _plooFields = plooFields;
    }

    public override void Execute()
    {
        List<Dictionary<string, string>> fieldsType = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "C", 7);
        List<Dictionary<string, string>> fieldsName = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "F", 7);
        for (int i = 0; i < fieldsType.Count; i++) 
        {
            var fieldName = fieldsName[i];
            var fieldType = fieldsType[i];
            int fieldTypeID = GetCellTypeId(fieldType["value"]);

            ExcelRange cell = Worksheet.Cells[fieldType["cordenate"]];

            if (_plooFields.FirstOrDefault(f => f.Name == fieldName["name"] && f.TypeId == fieldTypeID) is not null)
            {
                WorksheetService.SetCellAsFind(cell);
            }
            else
            {
                WorksheetService.SetCellAsNotFind(cell);
            }
            NextValidator?.Execute();
        }
    }

    private int GetCellTypeId(string text)
    {
        return int.Parse(Regex.Match(text, @"\[\d{1,2}\]").Value);
    }
}
