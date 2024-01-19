namespace LeitorExcelV3.Repositories;

public class PloomesRepository
{
    private readonly HttpClient _ploomesHttpClient;
    public PloomesRepository(HttpClient ploomesHttpClient)
    {
        _ploomesHttpClient = ploomesHttpClient;
    }


}
