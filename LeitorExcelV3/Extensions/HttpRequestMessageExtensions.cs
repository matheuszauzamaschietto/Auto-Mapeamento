namespace LeitorExcelV3.Extensions;

public static class HttpRequestMessageExtensions
{
    public static HttpRequestMessage Clone(this HttpRequestMessage httpRequestMessage)
    {
        HttpRequestMessage requestMessageCopy = new HttpRequestMessage();
        requestMessageCopy.RequestUri = httpRequestMessage.RequestUri;
        requestMessageCopy.Content = httpRequestMessage.Content;
        SetHeaders(requestMessageCopy, httpRequestMessage);
        requestMessageCopy.Method = httpRequestMessage.Method;
        return requestMessageCopy;
    }

    private static void SetHeaders(HttpRequestMessage requestMessageCopy, HttpRequestMessage requestMessage)
    {
        foreach (var header in requestMessage.Headers)
        {
            requestMessageCopy.Headers.Add(header.Key, header.Value);
        }
    }
}
