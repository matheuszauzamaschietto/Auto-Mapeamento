using LeitorExcelV3.Factories;
using LeitorExcelV3.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.Net;
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

    public async Task<(string serializedBody, HttpStatusCode responseCode)> SendClient(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = _httpRequestMessageFactory.Create(Enums.HttpRequestMessageFactoryEnum.CLIENT, connectionInfo);
        HttpResponseMessage response = await Send(httpMessage);
        return (await response?.Content?.ReadAsStringAsync() ?? "", response.StatusCode);
    }

    public async Task<List<T>?> SendPloomes<T>(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = _httpRequestMessageFactory.Create(Enums.HttpRequestMessageFactoryEnum.PLOOMES, connectionInfo);
        return (await Send<PlooBaseModel<T>?>(httpMessage)).Value;
    }

    private async Task<HttpResponseMessage> Send(HttpRequestMessage httpMessage)
    {
        HttpResponseMessage response = null;
        try
        {
            response = await _httpClient.SendAsync(httpMessage);
        }
        catch (Exception)
        {

        }
        return response;
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
