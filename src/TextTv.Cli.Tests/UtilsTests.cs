using TextTv.Cli.Helpers;

namespace TextTv.Cli.Tests;

[TestClass]
public class UtilsTests
{
    [TestMethod]
    public void GetTextTvApiUrl_GeneratesCorrectUrl()
    {
        // Arrange
        int pageNumber = 100;
        
        // Act
        string url = Utils.GetTextTvApiUrl(pageNumber);
        
        // Assert
        Assert.AreEqual("https://texttv.nu/api/get/100?includePlainTextContent=1", url);
    }
    
    [TestMethod]
    public void ExtractPageNumbers_FindsAllPageNumbers()
    {
        // Arrange
        string text = "See pages 100, 101 and page 700 for more info";
        
        // Act
        var pageNumbers = Utils.ExtractPageNumbers(text).ToList();
        
        // Assert
        Assert.AreEqual(3, pageNumbers.Count);
        CollectionAssert.Contains(pageNumbers, 100);
        CollectionAssert.Contains(pageNumbers, 101);
        CollectionAssert.Contains(pageNumbers, 700);
    }
    
    [TestMethod]
    public void ExtractPageNumbers_HandlesEmptyString()
    {
        // Arrange
        string text = "";
        
        // Act
        var pageNumbers = Utils.ExtractPageNumbers(text).ToList();
        
        // Assert
        Assert.AreEqual(0, pageNumbers.Count);
    }
    
    [TestMethod]
    public void ExtractPageNumbers_HandlesNullString()
    {
        // Arrange
        string? text = null;
        
        // Act
        var pageNumbers = Utils.ExtractPageNumbers(text!).ToList();
        
        // Assert
        Assert.AreEqual(0, pageNumbers.Count);
    }
    
    [TestMethod]
    public void ExtractPageNumbers_IdentifiesOnlyValidPageNumbers()
    {
        // Arrange
        string text = "The number 12 is not a page, but 100 is a valid page number";
        
        // Act
        var pageNumbers = Utils.ExtractPageNumbers(text).ToList();
        
        // Assert
        Assert.AreEqual(1, pageNumbers.Count);
        CollectionAssert.Contains(pageNumbers, 100);
    }
}
