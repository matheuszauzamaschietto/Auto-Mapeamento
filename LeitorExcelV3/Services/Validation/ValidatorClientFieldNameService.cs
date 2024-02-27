using LeitorExcelV3.Models;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Drawing;
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

            string[] fieldPath = field["name"].Replace("$.", "").Split(".");

            bool fieldIsValid = validateField(fieldPath.ToList(), JObject.Parse(_deserializedRequest));

            if (fieldIsValid)
            {
                WorksheetService.SetCellColor(cell, Color.Green);
            }
            else if (Regex.IsMatch(_deserializedRequest, $"\"{fieldPath.Last()}\""))
            {
                WorksheetService.SetCellColor(cell, Color.Yellow);
            }
            else
            {
                WorksheetService.SetCellAsNotFind(cell);
            }
        }
        NextValidator?.Execute();
    }

    private bool validateField(List<string> fieldPathSplited, JObject webServiceJson)
    {
        try
        {
            JToken pathGet = webServiceJson[fieldPathSplited.First()];

            if(pathGet?.Type == JTokenType.Object)
            {
                return validateField(fieldPathSplited[1..], (JObject)pathGet);
            }
            else if(pathGet?.Type == JTokenType.Array)
            {
                return validateField(fieldPathSplited[2..], (JObject)pathGet.FirstOrDefault());
            }
            else if (pathGet is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch 
        {
            return false;
        }
    }
}
