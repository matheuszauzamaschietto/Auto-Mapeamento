using LeitorExcelV3.Models;
using OfficeOpenXml;

namespace LeitorExcelV3.Services;

public class ValidatorPloomesService : Validator
{
    private readonly string _deserializedRequest;
    public ValidatorPloomesService(ExcelWorksheet worksheet ,WorksheetService worksheetService, string deserializedRequest) : base(worksheet, worksheetService)
    {
        _deserializedRequest = deserializedRequest;
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
