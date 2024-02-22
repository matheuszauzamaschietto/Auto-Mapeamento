using LeitorExcelV3.Enums;
using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Models;
using LeitorExcelV3.Services;
using LeitorExcelV3.Services.Discovery;
using LeitorExcelV3.Services.Validatiion;
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
                connectionInfos.SetConnectionInfo(worksheet);
                if (connectionInfos.Url is null || connectionInfos.HttpMethod is null)
                    continue;

                RequestService requestService = new(_httpClientFactory.CreateClient("client"), new Factories.HttpRequestMessageFactory());

                List<PlooFieldsModel>? plooFields = await requestService.SendPloomes<PlooFieldsModel>(connectionInfos);
                string clientFields = await requestService.SendClient(connectionInfos);

                #region Discovery

                ChainOfResponsability clientFieldsDiscovery = new DiscoveryClientFieldsService(worksheet, _worksheetService, clientFields);
                clientFieldsDiscovery.Execute();

                #endregion

                #region Validation

                ChainOfResponsability clientFieldNameValidator = new ValidatorClientFieldNameService(worksheet, _worksheetService, clientFields);
                ChainOfResponsability ploomesFieldNameValidator = new ValidatorPloomesFieldNameService(worksheet, _worksheetService, plooFields);
                ChainOfResponsability ploomesFieldTypeValidator = new ValidatorPloomesFieldTypeService (worksheet, _worksheetService, plooFields);
                clientFieldNameValidator.SetNext(ploomesFieldNameValidator);
                ploomesFieldNameValidator.SetNext(ploomesFieldTypeValidator);
                clientFieldNameValidator.Execute();

                #endregion Validation
            }
            package.SaveAs(filePath);
        }
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);

        _directoryService.DeleteDirectory();

        return File(bytes, formFile.ContentType, formFile.Name + " - validated");
    }
}
