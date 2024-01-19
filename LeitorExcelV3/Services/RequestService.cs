using LeitorExcelV3.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace LeitorExcelV3.Services;

public class RequestService
{
    private readonly HttpClient _httpClient;
    public RequestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> Send(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = MountClientHttpRequestMessage(connectionInfo);
        HttpResponseMessage response = await _httpClient.SendAsync(httpMessage);
        return await response.Content.ReadAsStringAsync();
    }

    private HttpRequestMessage MountClientHttpRequestMessage(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = new();
        httpMessage.Method = connectionInfo.HttpMethod;
        httpMessage.Content = new StringContent(connectionInfo.Body ?? "");
        SetHeaders(connectionInfo.ClientHeaders, httpMessage.Headers);
        httpMessage.RequestUri = new Uri(connectionInfo.Url);
        return httpMessage;
    }

    private void SetHeaders(Dictionary<string, string> headers, HttpRequestHeaders httpHeaders)
    {
        foreach (var header in headers)
            httpHeaders.Add(header.Key, header.Value);
    }
}
