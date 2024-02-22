﻿using LeitorExcelV3.Interfaces;
using LeitorExcelV3.Services;
using OfficeOpenXml;

namespace LeitorExcelV3.Models;

public abstract class Discoverator : ChainOfResponsability
{
    protected WorksheetService WorksheetService { get; }
    protected ChainOfResponsability NextValidator { get; set; }
    protected ExcelWorksheet Worksheet { get; set; }
    public Discoverator(ExcelWorksheet worksheet, WorksheetService worksheetService)
    {
        WorksheetService = worksheetService;
        Worksheet = worksheet;
    } 

    public void SetNext(ChainOfResponsability validator)
    {
        NextValidator = validator;
    }

    public abstract void Execute();
}

