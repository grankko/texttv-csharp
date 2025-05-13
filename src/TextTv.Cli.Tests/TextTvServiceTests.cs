using System.Net;
using TextTv.Cli.Services;

namespace TextTv.Cli.Tests;

[TestClass]
public class TextTvServiceTests
{
    [TestMethod]
    public async Task FetchPageAsync_ReturnsValidPage_WhenPageExists()
    {
        // Arrange
        var mockHttp = new HttpClientMock();
        mockHttp.SetupResponse(
            "https://texttv.nu/api/get/100?includePlainTextContent=1",
            @"[{""Num"":""100"",""Content"":[""<div>Test content</div>""],""NextPage"":""101"",""PrevPage"":""99""}]"
        );
        var httpClient = mockHttp.CreateClient();
        var service = new TextTvService(httpClient);
        
        // Act
        var page = await service.FetchPageAsync(100);
        
        // Assert
        Assert.IsNotNull(page);
        Assert.AreEqual("100", page.Num);
        Assert.AreEqual("101", page.NextPage);
        Assert.AreEqual("99", page.PrevPage);
        Assert.AreEqual(1, page.Content.Length);
        Assert.AreEqual("<div>Test content</div>", page.Content[0]);
    }
    
    [TestMethod]
    public async Task FetchPageAsync_ReturnsNull_WhenApiReturnsError()
    {
        // Arrange
        var mockHttp = new HttpClientMock();
        mockHttp.SetupFailedResponse(
            "https://texttv.nu/api/get/999?includePlainTextContent=1",
            HttpStatusCode.NotFound
        );
        var httpClient = mockHttp.CreateClient();
        var service = new TextTvService(httpClient);
        
        // Act
        var page = await service.FetchPageAsync(999);
        
        // Assert
        Assert.IsNull(page);
    }
    
    [TestMethod]
    public async Task FetchPageAsync_ReturnsNull_WhenDeserializationFails()
    {
        // Arrange
        var mockHttp = new HttpClientMock();
        mockHttp.SetupResponse(
            "https://texttv.nu/api/get/100?includePlainTextContent=1",
            "invalid json"
        );
        var httpClient = mockHttp.CreateClient();
        var service = new TextTvService(httpClient);
        
        // Act
        var page = await service.FetchPageAsync(100);
        
        // Assert
        Assert.IsNull(page);
    }
    
    [TestMethod]
    public async Task FetchPageAsync_ReturnsNull_WhenEmptyArrayReturned()
    {
        // Arrange
        var mockHttp = new HttpClientMock();
        mockHttp.SetupResponse(
            "https://texttv.nu/api/get/100?includePlainTextContent=1",
            "[]"
        );
        var httpClient = mockHttp.CreateClient();
        var service = new TextTvService(httpClient);
        
        // Act
        var page = await service.FetchPageAsync(100);
        
        // Assert
        Assert.IsNull(page);
    }
}
