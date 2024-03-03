using LeitorExcelV3.Enums;
using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Models;
using LeitorExcelV3.Services;
using LeitorExcelV3.Services.Discovery;
using LeitorExcelV3.Services.Implementation;
using LeitorExcelV3.Services.Validatiion;
using LeitorExcelV3.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
namespace LeitorExcelV3.Controllers;

[Route("excel")]
[ApiController]
public class ExcelController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WorksheetService _worksheetService;
    private readonly DirectoryService _directoryService;
    public ExcelController(IHttpClientFactory httpClientFactory, WorksheetService worksheetService, DirectoryService directoryService)
    {
        _httpClientFactory = httpClientFactory;
        _worksheetService = worksheetService;
        _directoryService = directoryService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> ValidateExcel(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            return BadRequest("Arquivo não enviado");
        } 

        string filePath = _directoryService.CreateDiretory(formFile);
        using (var stream = new MemoryStream())
        {
            await formFile.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            ConnectionInfos connectionInfos = new(package.Workbook.Worksheets);

            foreach (var worksheet in package.Workbook.Worksheets)
            {
                connectionInfos.ClientConnection.SetConnectionInfo(worksheet);
                connectionInfos.PloomesConnection.SetEntity(worksheet);

                if (connectionInfos.ClientConnection.Url is null || connectionInfos.ClientConnection.HttpMethod is null || connectionInfos.PloomesConnection.EntityId is null)
                    continue;

                PhasesModel phases = new PhasesModel(worksheet);
                RequestService requestService = new RequestService(_httpClientFactory.CreateClient("client"), new Factories.HttpRequestMessageFactory());

                List<PlooFieldsModel>? plooFields = await requestService.SendPloomes<PlooFieldsModel>(connectionInfos);
                var clientResponse = await requestService.SendClient(connectionInfos);

                ExcelLogger excelLogger = new ExcelLogger();
                excelLogger.SetClientResponse(worksheet, clientResponse.serializedBody, clientResponse.responseCode);


                if (phases.DoDiscovery)
                {
                    IPrimaryService discovery = new DiscoveryService(worksheet, clientResponse.serializedBody, _worksheetService);
                    discovery.Execute();
                }

                if (phases.DoValidation)
                {
                    IPrimaryService Validation = new ValidationService(worksheet, _worksheetService, clientResponse.serializedBody, plooFields);
                    Validation.Execute();
                }

                if (phases.DoImplementation)
                {
                    IPrimaryService implementation = new ImplementationService(worksheet, _worksheetService, plooFields, requestService, connectionInfos);
                    implementation.Execute();
                }
            }

            //package.Save();
            package.SaveAs(new FileInfo(filePath));
        }
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);

        _directoryService.DeleteDirectory();

        return File(bytes, formFile.ContentType, formFile.Name + " - validated");
    }
}
