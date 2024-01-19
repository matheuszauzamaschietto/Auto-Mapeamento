using Microsoft.AspNetCore.Http;

namespace LeitorExcelV3.Services;

public class DirectoryService
{
    private string _directoryPath;
    private string _filePath;
    public DirectoryService()
    {

    }
    

    private (string tmpDirectory, string file) GetDirectoryAndFilePath(IFormFile formFile)
    {
        string directoryPath = $"{Directory.GetCurrentDirectory()}/excel/worksheets/{Guid.NewGuid()}/";
        string filePath = directoryPath + formFile.Name + " - validated";
        return (directoryPath, filePath);
    }

    public string CreateDiretory(IFormFile formFile)
    {
        var path = GetDirectoryAndFilePath(formFile);
        _directoryPath = path.tmpDirectory;
        _filePath = path.file;

        Directory.CreateDirectory(_directoryPath);
        return _filePath;
    }

    public void DeleteDirectory()
    {
        File.Delete(_filePath);
        Directory.Delete(_directoryPath);
    }
}
