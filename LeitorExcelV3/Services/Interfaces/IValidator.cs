namespace LeitorExcelV3.Services.Interfaces;

public interface IValidator
{
    void SetNext(IValidator validator);
    void Execute();
}
