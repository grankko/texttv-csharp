using TextTv.Cli.Configuration;

namespace TextTv.Cli.Tests;

[TestClass]
public class CommandLineOptionsTests
{
    [TestMethod]
    public void Parse_SingleArgument_SetsPageNumber()
    {
        // Arrange
        string[] args = new[] { "100" };
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.AreEqual(100, options.PageNumber);
        Assert.IsNull(options.Url);
        Assert.AreEqual(AppMode.PageNumber, options.Mode);
    }
    
    [TestMethod]
    public void Parse_PageNumberFlag_SetsPageNumber()
    {
        // Arrange
        string[] args = new[] { "--pagenumber", "200" };
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.AreEqual(200, options.PageNumber);
        Assert.IsNull(options.Url);
        Assert.AreEqual(AppMode.PageNumber, options.Mode);
    }
    
    [TestMethod]
    public void Parse_ShortPageNumberFlag_SetsPageNumber()
    {
        // Arrange
        string[] args = new[] { "-p", "300" };
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.AreEqual(300, options.PageNumber);
        Assert.IsNull(options.Url);
        Assert.AreEqual(AppMode.PageNumber, options.Mode);
    }
    
    [TestMethod]
    public void Parse_UrlFlag_SetsUrl()
    {
        // Arrange
        string[] args = new[] { "--url", "https://example.com" };
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.IsNull(options.PageNumber);
        Assert.AreEqual("https://example.com", options.Url);
        Assert.AreEqual(AppMode.Url, options.Mode);
    }
    
    [TestMethod]
    public void Parse_ShortUrlFlag_SetsUrl()
    {
        // Arrange
        string[] args = new[] { "-u", "https://example.com" };
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.IsNull(options.PageNumber);
        Assert.AreEqual("https://example.com", options.Url);
        Assert.AreEqual(AppMode.Url, options.Mode);
    }
    
    [TestMethod]
    public void Parse_NoArgs_ReturnsUnknownMode()
    {
        // Arrange
        string[] args = Array.Empty<string>();
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.IsNull(options.PageNumber);
        Assert.IsNull(options.Url);
        Assert.AreEqual(AppMode.Unknown, options.Mode);
    }
    
    [TestMethod]
    public void Parse_InvalidPageNumber_ReturnsUnknownMode()
    {
        // Arrange
        string[] args = new[] { "not-a-number" };
        
        // Act
        var options = CommandLineOptions.Parse(args);
        
        // Assert
        Assert.IsNull(options.PageNumber);
        Assert.IsNull(options.Url);
        Assert.AreEqual(AppMode.Unknown, options.Mode);
    }
}
