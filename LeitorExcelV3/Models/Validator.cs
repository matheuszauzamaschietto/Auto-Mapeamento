using LeitorExcelV3.Services;
using LeitorExcelV3.Services.Interfaces;
using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public abstract class Validator : IValidator
{
    protected WorksheetService WorksheetService { get; }
    protected IValidator NextValidator { get; set; }
    protected ExcelWorksheet Worksheet { get; set; }
    public Validator(ExcelWorksheet worksheet, WorksheetService worksheetService)
    {
        WorksheetService = worksheetService;
        Worksheet = worksheet;
    }

    public void SetNext(IValidator validator)
    {
        NextValidator = validator;
    }

    public abstract void Execute();
}

