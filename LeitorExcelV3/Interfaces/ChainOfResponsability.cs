namespace LeitorExcelV3.Interfaces;

public interface ChainOfResponsability
{
    void SetNext(ChainOfResponsability validator);
    void Execute();
}
