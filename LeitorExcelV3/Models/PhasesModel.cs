using LeitorExcelV3.Enums;
using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public class PhasesModel
{
    public readonly bool DoDiscovery;
    public readonly bool DoValidation;
    public readonly bool DoImplementation;

    public PhasesModel(ExcelWorksheet worksheet)
    {
        DoDiscovery = worksheet.Cells[CellDefaultPositions.DISCOVERY_CELL].GetValue<string>() == "Sim" ? true : false;
        DoValidation = worksheet.Cells[CellDefaultPositions.VALIDATION_CELL].GetValue<string>() == "Sim" ? true : false;
        DoImplementation = worksheet.Cells[CellDefaultPositions.IMPLEMENTATION_CELL].GetValue<string>() == "Sim" ? true : false;
    }
}
