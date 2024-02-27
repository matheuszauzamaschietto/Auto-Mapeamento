using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Models;
using LeitorExcelV3.Services.Validatiion;
using OfficeOpenXml;

namespace LeitorExcelV3.Services;

public class ValidationService : IPrimaryService
{
    private readonly ExcelWorksheet _worksheet;
    private readonly WorksheetService _worksheetService;
    private readonly string _clientFields;
    private readonly List<PlooFieldsModel>? _plooFields;

    public ValidationService(ExcelWorksheet worksheet, WorksheetService worksheetService, string clientFields, List<PlooFieldsModel>? plooFieldsModels)
    {
        _worksheet = worksheet;
        _worksheetService = worksheetService;
        _clientFields = clientFields;
        _plooFields = plooFieldsModels;
    }

    public void Execute()
    {
        IChainOfResponsability clientFieldNameValidator = new ValidatorClientFieldNameService(_worksheet, _worksheetService, _clientFields);
        IChainOfResponsability ploomesFieldNameValidator = new ValidatorPloomesFieldNameService(_worksheet, _worksheetService, _plooFields);
        IChainOfResponsability ploomesFieldTypeValidator = new ValidatorPloomesFieldTypeService(_worksheet, _worksheetService, _plooFields);
        clientFieldNameValidator.SetNext(ploomesFieldNameValidator);
        ploomesFieldNameValidator.SetNext(ploomesFieldTypeValidator);
        clientFieldNameValidator.Execute();
    }
}
