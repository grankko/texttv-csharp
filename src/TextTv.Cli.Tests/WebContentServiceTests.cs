using TextTv.Cli.Services.Web;

namespace TextTv.Cli.Tests;

[TestClass]
public class WebContentServiceTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task FetchWebContentAsync_InvalidUrl_ThrowsException()
    {
        // Arrange
        var httpClient = new HttpClient();
        var service = new WebContentService(httpClient);
        
        // Act & Assert - Exception expected
        await service.FetchWebContentAsync("not-a-valid-url");
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task FetchWebContentAsync_EmptyUrl_ThrowsException()
    {
        // Arrange
        var httpClient = new HttpClient();
        var service = new WebContentService(httpClient);
        
        // Act & Assert - Exception expected
        await service.FetchWebContentAsync(string.Empty);
    }
}
