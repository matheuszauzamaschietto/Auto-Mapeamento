namespace LeitorExcelV3.Interfaces;

public interface IChainOfResponsability
{
    void SetNext(IChainOfResponsability validator);
    void Execute();
}
