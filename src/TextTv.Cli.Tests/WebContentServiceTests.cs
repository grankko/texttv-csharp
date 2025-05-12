using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using TextTv.Cli.Services.Web;

namespace TextTv.Cli.Tests;

[TestClass]
public class WebContentServiceTests
{
    [TestMethod]
    [Ignore("Requires more setup with the mocking framework")]
    public async Task FetchWebContentAsync_ValidUrl_ExtractsContent()
    {
        // This test requires additional setup with the mocking framework
        // Will be implemented in a future update
    }
    
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
}
