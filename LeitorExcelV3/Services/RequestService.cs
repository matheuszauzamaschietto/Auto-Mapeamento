using LeitorExcelV3.Extensions;
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
        HttpResponseMessage response = await SendRequest(httpMessage);
        return (await response?.Content?.ReadAsStringAsync() ?? "", response.StatusCode);
    }

    public async Task<HttpResponseMessage> SendPloomesField(ConnectionInfos connectionInfo, PlooFieldsModel field)
    {
        HttpRequestMessage httpMessage = _httpRequestMessageFactory.Create(Enums.HttpRequestMessageFactoryEnum.PLOOMES_FIELD, connectionInfo, field);
        return await Send(httpMessage);
    }

    public async Task<List<T>?> SendPloomes<T>(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = _httpRequestMessageFactory.Create(Enums.HttpRequestMessageFactoryEnum.PLOOMES, connectionInfo);
        return (await SendRequest<PlooBaseModel<T>?>(httpMessage)).Value;
    }

    private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage httpMessage)
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

    private async Task<T?> SendRequest<T>(HttpRequestMessage httpMessage)
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

    private async Task<HttpResponseMessage> Send(HttpRequestMessage requestMessage)
    {
        HttpStatusCode? statusCode = null;
        HttpResponseMessage response;
        do
        {
            HttpRequestMessage requestMessageCopy = requestMessage.Clone();
            response = await _httpClient.SendAsync(requestMessageCopy);
            statusCode = response.StatusCode;
        } while (statusCode == HttpStatusCode.TooManyRequests);
        return response;
    }

}
