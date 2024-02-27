using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Services.Discovery;
using OfficeOpenXml;

namespace LeitorExcelV3.Services;

public class DiscoveryService : IPrimaryService
{
    private readonly ExcelWorksheet _worksheet;
    private readonly string _clientFields;
    private readonly WorksheetService _worksheetService;

    public DiscoveryService(ExcelWorksheet worksheet, string clientFields, WorksheetService worksheetService)
    {
        _worksheet = worksheet;
        _clientFields = clientFields;
        _worksheetService = worksheetService;
    }

    public void Execute()
    {
        IChainOfResponsability clientFieldsDiscovery = new DiscoveryClientFieldsService(_worksheet, _worksheetService, _clientFields);
        clientFieldsDiscovery.Execute();
    }
}
