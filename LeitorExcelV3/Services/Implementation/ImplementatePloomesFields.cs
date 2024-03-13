using LeitorExcelV3.Enums;
using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Models;
using OfficeOpenXml;

namespace LeitorExcelV3.Services.Implementation;

public class ImplementatePloomesFields : Implementator
{
    private readonly RequestService _requestService;
    private readonly ConnectionInfos _connectionInfos;
    private readonly List<PlooFieldsModel> _plooExistFields;

    public ImplementatePloomesFields(ExcelWorksheet worksheet, WorksheetService worksheetService, RequestService requestService, List<PlooFieldsModel> plooExistFields, ConnectionInfos connectionInfos) : base(worksheet, worksheetService)
    {
        _requestService = requestService;
        _plooExistFields = plooExistFields;
        _connectionInfos = connectionInfos;
    }

    public async override void Execute()
    {
        var plooFieldsLists = GetFieldsCreateModels();
        for (int i = 0; i < plooFieldsLists.fieldsToCreate.Count; i++)
        {
            var allowCreationCell = Worksheet.Cells[plooFieldsLists.fieldsWhoAllowCreationCordenates[i]];
            var field = plooFieldsLists.fieldsToCreate[i];
            if (_plooExistFields.FirstOrDefault(p => p.Name == field.Name) is null)
            {
                await CreateFieldAtPloomes(field, allowCreationCell);
            }
            else
            {
                WorksheetService.SetCellAsFind(allowCreationCell);
            }
        }
    }

    private async Task CreateFieldAtPloomes(PlooFieldsModel field, ExcelRange allowCreationCell)
    {
        HttpResponseMessage responseMessage = _requestService.SendPloomesField(_connectionInfos, field).Result;
        if (responseMessage.IsSuccessStatusCode)
        {
            WorksheetService.SetCellAsFind(allowCreationCell);
        }
        else
        {
            WorksheetService.SetCellAsNotFind(allowCreationCell);
        }
    }
    private (List<PlooFieldsModel> fieldsToCreate, List<string> fieldsWhoAllowCreationCordenates) GetFieldsCreateModels()
    {
        var ploomesFieldNames = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "F", 7);
        var ploomesFieldTypes = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "C", 7);
        var ploomesFieldReceiveKeys = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "A", 7);
        var allowCreationFields = WorksheetService.GetWorksheetCellsStillBeEmpty(Worksheet, "I", 7);
        string entityField = Worksheet.Cells[CellDefaultPositions.ENTITY_CELL].GetValue<string>();

        List<PlooFieldsModel> plooFieldsModels = new List<PlooFieldsModel>();
        List<string> plooFieldsWhoAllowCreation = new List<string>();

        for (int i = 0; i < ploomesFieldNames.Count; i++)
        {
            if (allowCreationFields[i]["name"] == "Sim")
            {
                plooFieldsModels.Add(new PlooFieldsModel(ploomesFieldNames[i]["name"],
                                                         WorksheetService.GetCellTypeId(ploomesFieldTypes[i]["name"]),
                                                         WorksheetService.GetCellTypeId(entityField),
                                                         ploomesFieldReceiveKeys[i]["name"]));
                plooFieldsWhoAllowCreation.Add(allowCreationFields[i]["cordenate"]);
            }
        }
        return (plooFieldsModels, plooFieldsWhoAllowCreation);
    }

}
