using HtmlAgilityPack;
using TextTv.Cli.Helpers;

namespace TextTv.Cli.Tests;

[TestClass]
public class TextParsingHelperTests
{
    [TestMethod]
    public void DetermineTextColor_YellowForHeadlines()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<div class=\"DH\">Headline Text</div>");
        var node = doc.DocumentNode.SelectSingleNode("//div");
        
        // Act
        var color = TextParsingHelper.DetermineTextColor(node!);
        
        // Assert
        Assert.AreEqual(ConsoleColor.Yellow, color);
    }
    
    [TestMethod]
    public void DetermineTextColor_CyanForSecondaryContent()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<div style=\"color: Cyan\">Secondary Text</div>");
        var node = doc.DocumentNode.SelectSingleNode("//div");
        
        // Act
        var color = TextParsingHelper.DetermineTextColor(node!);
        
        // Assert
        Assert.AreEqual(ConsoleColor.Cyan, color);
    }

    [TestMethod]
    public void DetermineTextColor_CyanForClassAttribute()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<div class='C'>Secondary Text</div>");
        var node = doc.DocumentNode.SelectSingleNode("//div");

        // Act
        var color = TextParsingHelper.DetermineTextColor(node!);

        // Assert
        Assert.AreEqual(ConsoleColor.Cyan, color);
    }
    
    [TestMethod]
    public void DetermineTextColor_WhiteForRegularText()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<div>Regular Text</div>");
        var node = doc.DocumentNode.SelectSingleNode("//div");
        
        // Act
        var color = TextParsingHelper.DetermineTextColor(node!);
        
        // Assert
        Assert.AreEqual(ConsoleColor.White, color);
    }
    
    [TestMethod]
    public void DetermineTextColor_WhiteForNullNode()
    {
        // Act
        // The null! operator tells the compiler we know this is null and to suppress warnings
        HtmlNode? nullNode = null;
        var color = TextParsingHelper.DetermineTextColor(nullNode!);
        
        // Assert
        Assert.AreEqual(ConsoleColor.White, color);
    }
    
    [TestMethod]
    public void ExtractTextContent_ExtractsPlainText()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<div>Plain <b>Text</b> Content</div>");
        var node = doc.DocumentNode.SelectSingleNode("//div");
        
        // Act
        var text = TextParsingHelper.ExtractTextContent(node!);
        
        // Assert
        Assert.AreEqual("Plain Text Content", text);
    }
    
    [TestMethod]
    public void ExtractTextContent_PreservesPageNumbers()
    {
        // Arrange
        var doc = new HtmlDocument();
        doc.LoadHtml("<div>Go to page 100 for more</div>");
        var node = doc.DocumentNode.SelectSingleNode("//div");
        
        // Act
        var text = TextParsingHelper.ExtractTextContent(node!);
        
        // Assert
        Assert.AreEqual("Go to page 100 for more", text);
    }
    
    [TestMethod]
    public void ExtractTextContent_HandlesNullNode()
    {
        // Act
        // The null! operator tells the compiler we know this is null and to suppress warnings
        HtmlNode? nullNode = null;
        var text = TextParsingHelper.ExtractTextContent(nullNode!);
        
        // Assert
        Assert.AreEqual(string.Empty, text);
    }
    
    [TestMethod]
    public void ExtractContentFromHtml_HandlesCompleteHtml()
    {
        // Arrange
        string html = "<div><span style=\"color:Yellow\">Important Update</span></div>";
        
        // Act
        var text = TextParsingHelper.ExtractContentFromHtml(html);
        
        // Assert
        Assert.AreEqual("Important Update", text);
    }
    
    [TestMethod]
    public void ExtractContentFromHtml_ReturnsEmptyForEmptyDiv()
    {
        // Arrange
        string html = "<div></div>";
        
        // Act
        var text = TextParsingHelper.ExtractContentFromHtml(html);
        
        // Assert
        Assert.AreEqual(string.Empty, text);
    }
}
