using System.Net;
using Moq;
using Moq.Protected;

namespace TextTv.Cli.Tests;

/// <summary>
/// Helper class for mocking HttpClient
/// </summary>
public partial class HttpClientMock
{
    private readonly Mock<HttpMessageHandler> _handlerMock;
    
    public HttpClientMock()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
    }
    
    /// <summary>
    /// Sets up a successful HTTP response with the specified content
    /// </summary>
    public void SetupResponse(string requestUrl, string content)
    {
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString() == requestUrl),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            });
    }
    
    /// <summary>
    /// Sets up a failed HTTP response with the specified status code
    /// </summary>
    public void SetupFailedResponse(string requestUrl, HttpStatusCode statusCode)
    {
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString() == requestUrl),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(string.Empty)
            });
    }
    
    /// <summary>
    /// Creates and returns a new HttpClient using the mocked handler
    /// </summary>
    public HttpClient CreateClient()
    {
        return new HttpClient(_handlerMock.Object);
    }
}
