using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public class PhasesModel
{
    public readonly bool DoDiscovery;
    public readonly bool DoValidation;
    public readonly bool DoImplementation;

    public PhasesModel(ExcelWorksheet worksheet)
    {
        DoDiscovery = worksheet.Cells["Z7"].GetValue<string>() == "Sim" ? true : false;
        DoValidation = worksheet.Cells["Z8"].GetValue<string>() == "Sim" ? true : false;
        DoImplementation = worksheet.Cells["Z9"].GetValue<string>() == "Sim" ? true : false;
    }
}
