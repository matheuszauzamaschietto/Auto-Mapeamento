using LeitorExcelV3.Models;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Services.Discovery;

public class DiscoveryClientFieldsService: Discoverator
{
    private readonly string _deserializedRequest;

    public DiscoveryClientFieldsService(ExcelWorksheet worksheet, WorksheetService worksheetService, string deserializedRequest) : base(worksheet, worksheetService)
    {
        _deserializedRequest = deserializedRequest;
    }

    public override void Execute()
    {
        List<Dictionary<string, string>> fields = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "A", 7);
        List<string> clientFields = GetClientFields();

        clientFields = clientFields.Except(fields.Select(p => p["name"])).ToList();
        WorksheetService.WriteAdditionalFields(Worksheet, "A", 7, clientFields);
    }

    private List<string> GetClientFields()
    {
        List<string> fields = new List<string>();
        JObject jsonFields = JObject.Parse(_deserializedRequest);
        // jsonField.Value.Type // Adicionar pesquisa por baixa granularidade

            //fields.Add(jsonField.Key);
        fields.AddRange(GetFieldsPath("$", jsonFields, fields));
        

        return fields;
    }

    private List<string> GetFieldsPath(string jsonPath, JObject jsonObject, List<string> fields)
    {
        foreach(var field in jsonObject)
        {
            if(field.Value.Type != JTokenType.Object && field.Value.Type != JTokenType.Array)
            {
                fields.Add(MountJsonPath(jsonPath, field.Key));
            }
            else if (field.Value.Type == JTokenType.Object)
            {
                GetFieldsPath(MountJsonPath(jsonPath, field.Key), (JObject)field.Value, fields);
            }
        }
        return fields;
    }

    private string MountJsonPath(string jsonPath, string newJsonExtension)
    {
        return jsonPath + "." + newJsonExtension;
    }


}
