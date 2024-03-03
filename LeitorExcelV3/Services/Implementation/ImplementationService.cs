using LeitorExcelV3.Enums;
using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Models;
using OfficeOpenXml;

namespace LeitorExcelV3.Services.Implementation;

public class ImplementationService : IPrimaryService
{
    private readonly ExcelWorksheet _worksheet;
    private readonly WorksheetService _worksheetService;
    private readonly List<PlooFieldsModel>? _plooFields;
    private readonly RequestService _requestService;
    private readonly ConnectionInfos _connectionInfos;
    public ImplementationService(ExcelWorksheet worksheet, WorksheetService worksheetService, List<PlooFieldsModel>? plooFields, RequestService requestService, ConnectionInfos connectionInfos)
    {
        _connectionInfos = connectionInfos;
        _worksheet = worksheet;
        _worksheetService = worksheetService;
        _requestService = requestService;
        _plooFields = plooFields;
    }
    public void Execute()
    {
        IChainOfResponsability implentationFields = new ImplementatePloomesFields(_worksheet, _worksheetService, _requestService, _plooFields, _connectionInfos);
        implentationFields.Execute();
    }

}
