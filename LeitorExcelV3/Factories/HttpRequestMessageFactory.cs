using LeitorExcelV3.Enums;
using LeitorExcelV3.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace LeitorExcelV3.Factories;

public class HttpRequestMessageFactory
{
    public HttpRequestMessageFactory()
    {
        
    }

    public HttpRequestMessage? Create(HttpRequestMessageFactoryEnum type, ConnectionInfos connectionInfo, PlooFieldsModel? field = null)
    {
        if(type == HttpRequestMessageFactoryEnum.CLIENT)
        {
            return CreateClientMessage(connectionInfo);
        }
        else if (type == HttpRequestMessageFactoryEnum.PLOOMES)
        {
            return CreatePloomesMessage(connectionInfo);
        }
        else if (type == HttpRequestMessageFactoryEnum.PLOOMES_FIELD)
        {
            return CreatePloomesFieldCreationMessage(connectionInfo, field);
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

    private HttpRequestMessage CreatePloomesFieldCreationMessage(ConnectionInfos connectionInfo, PlooFieldsModel field)
    {
        HttpRequestMessage httpMessage = new();
        httpMessage.Method = HttpMethod.Post;

        httpMessage.Content = new StringContent(JsonSerializer.Serialize(field,  new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        }));

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
