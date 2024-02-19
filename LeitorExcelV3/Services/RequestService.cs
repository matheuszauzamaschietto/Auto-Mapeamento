using LeitorExcelV3.Factories;
using LeitorExcelV3.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LeitorExcelV3.Services;

public class RequestService
{
    private readonly HttpClient _httpClient;
    private readonly HttpRequestMessageFactory _httpRequestMessageFactory;
    public RequestService(HttpClient httpClient, HttpRequestMessageFactory httpRequestMessageFactory)
    {
        _httpRequestMessageFactory = httpRequestMessageFactory;
        _httpClient = httpClient;
    }

    public async Task<string> SendClient(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = _httpRequestMessageFactory.Create(Enums.HttpRequestMessageFactoryEnum.CLIENT, (connectionInfo));
        return await Send(httpMessage);
    }

    public async Task<T?> SendPloomes<T>(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = _httpRequestMessageFactory.Create(Enums.HttpRequestMessageFactoryEnum.PLOOMES, (connectionInfo));
        return await Send<T>(httpMessage);
    }

    private async Task<string> Send(HttpRequestMessage httpMessage)
    {
        HttpResponseMessage response = null;
        try
        {
            response = await _httpClient.SendAsync(httpMessage);
        }
        catch (Exception)
        {

        }
        return await response?.Content?.ReadAsStringAsync() ?? "";
    }

    private async Task<T?> Send<T>(HttpRequestMessage httpMessage)
    {
        HttpResponseMessage response = null;
        try
        {
            response = await _httpClient.SendAsync(httpMessage);
        }
        catch (Exception)
        {

        }
        return JsonSerializer.Deserialize<T>(await response?.Content?.ReadAsStringAsync());
    }

}
