﻿using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using System.Text.RegularExpressions;

namespace LeitorExcelV3.Services;

public class WorksheetService
{
    public WorksheetService()
    {
        
    }

    public void SetCellAsNotFind(ExcelRange cell)
    {
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(color: System.Drawing.Color.Red);
    }

    public void SetCellColor(ExcelRange cell, Color cor)
    {
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(color: cor);
    }

    public void SetCellAsFind(ExcelRange cell)
    {
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(color: System.Drawing.Color.Green);
    }

    public List<Dictionary<string, string>> GetWorksheetCellsStillBeEmpty(ExcelWorksheet worksheet, string column, int row)
    {
        List<Dictionary<string, string>> sheetFields = new();
        string? cellValue = string.Empty;
        do
        {
            cellValue = worksheet.Cells[column + row].GetValue<string>() ?? string.Empty;
            if (cellValue != string.Empty && cellValue != null)
                sheetFields.Add(new Dictionary<string, string>() { { "name", $"{cellValue}" }, { "cordenate", column + row } });
            row++;
        } while (cellValue != string.Empty);
        return sheetFields;
    }

    public ExcelWorksheet WriteAdditionalFields(ExcelWorksheet worksheet, string column, int row, List<string> fieldsToWrite)
    {
        int fieldsWritedCount = 0;
        do
        {
            var cellValue = worksheet.Cells[column + row];
            
            if ((cellValue.GetValue<string>() ?? string.Empty) == string.Empty)
            {
                cellValue.Value = fieldsToWrite[fieldsWritedCount];
                fieldsWritedCount++;
            }
                
            row++;
        } while (fieldsWritedCount < fieldsToWrite.Count);
        return worksheet;
    }

    public int GetCellTypeId(string text)
    {
        int typeId = 0;
        int.TryParse(Regex.Match(text, @"(?<=\[)\d{1,2}(?=\])").Value, out typeId);
        return typeId;
    }
}
