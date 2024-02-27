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
        SetHeaders(connectionInfo.PloomesConnection.Headers, httpMessage.Headers);
        httpMessage.RequestUri = new Uri(connectionInfo.PloomesConnection.Url);
        return httpMessage;
    }


    private HttpRequestMessage CreateClientMessage(ConnectionInfos connectionInfo)
    {
        HttpRequestMessage httpMessage = new();
        httpMessage.Method = connectionInfo.ClientConnection.HttpMethod;
        httpMessage.Content = new StringContent(connectionInfo.ClientConnection.Body ?? "");
        SetHeaders(connectionInfo.ClientConnection.Headers, httpMessage.Headers);
        httpMessage.RequestUri = new Uri(connectionInfo.ClientConnection.Url);
        return httpMessage;
    }

    private void SetHeaders(Dictionary<string, string> headers, HttpRequestHeaders httpHeaders)
    {
        foreach (var header in headers)
            httpHeaders.Add(header.Key, header.Value);
    }
}
