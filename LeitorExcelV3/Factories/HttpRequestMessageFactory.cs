using LeitorExcelV3.Enums;
using LeitorExcelV3.Models;
using System.Net.Http.Headers;
using LeitorExcelV3.Models;

namespace LeitorExcelV3.Factories;

public class HttpRequestMessageFactory
{
    public HttpRequestMessageFactory()
    {
        
    }

    public HttpRequestMessage? Create(HttpRequestMessageFactoryEnum type, ConnectionInfos connectionInfo)
    {
        if(type == HttpRequestMessageFactoryEnum.CLIENT)
        {
            return CreateClientMessage(connectionInfo);
        }
        else if (type == HttpRequestMessageFactoryEnum.PLOOMES)
        {
            return CreatePloomesMessage(connectionInfo);
        }
        return null;
    }

    private HttpRequestMessage CreatePloomesMessage(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = new();
        httpMessage.Method = HttpMethod.Get;
        SetHeaders(connectionInfo.PloomesHeader, httpMessage.Headers);
        httpMessage.RequestUri = new Uri(connectionInfo.PloomesUrl);
        return httpMessage;
    }


    private HttpRequestMessage CreateClientMessage(ConnectionInfos connectionInfo)
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
