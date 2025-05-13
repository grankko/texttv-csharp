using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenAI.Chat;
using System.Text.Json;
using TextTv.Cli.Configuration;
using TextTv.Cli.Models;
using TextTv.Cli.Services.OpenAi;

namespace TextTv.Cli.Tests;

[TestClass]
public class OpenAiServiceTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_NullSettings_ThrowsArgumentNullException()
    {
        // Act - should throw ArgumentNullException
        _ = new OpenAiService(null!);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Constructor_EmptyApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var settings = new Settings
        {
            OpenAiApiKey = string.Empty
        };

        // Act - should throw InvalidOperationException
        _ = new OpenAiService(settings);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task ConvertWebToTextTvAsync_EmptyUrl_ThrowsArgumentException()
    {
        // Arrange
        var settings = new Settings
        {
            OpenAiApiKey = "test-api-key",
            OpenAiModel = "test-model"
        };
        var service = new OpenAiService(settings);

        // Act - Should throw ArgumentException
        await service.ConvertWebToTextTvAsync(string.Empty, "content");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task ConvertWebToTextTvAsync_EmptyContent_ThrowsArgumentException()
    {
        // Arrange
        var settings = new Settings
        {
            OpenAiApiKey = "test-api-key",
            OpenAiModel = "test-model"
        };
        var service = new OpenAiService(settings);

        // Act - Should throw ArgumentException
        await service.ConvertWebToTextTvAsync("https://example.com", string.Empty);
    }
}
