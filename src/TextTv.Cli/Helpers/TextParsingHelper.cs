using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace TextTv.Cli.Helpers;

/// <summary>
/// Helper class for parsing HTML content from Text TV pages
/// </summary>
public static class TextParsingHelper
{
    /// <summary>
    /// Determines the color to use based on HTML content
    /// </summary>
    /// <param name="node">The HTML node to analyze</param>
    /// <returns>The appropriate console color for the content</returns>
    public static ConsoleColor DetermineTextColor(HtmlNode node)
    {
        if (node == null) return ConsoleColor.White;
        
        var classes = node.GetAttributeValue("class", "");
        bool isHeadline = classes.Contains("DH") || 
                          node.InnerHtml.Contains("transform:scaleY(2)") ||
                          node.InnerHtml.Contains("bgY");
                          
        bool isCyan = node.InnerHtml.Contains("bgC") || 
                      node.InnerHtml.Contains("Cyan") ||
                      node.GetAttributeValue("style", "").Contains("Cyan");
        
        // Set color based on content type
        if (isHeadline || classes.Contains("Y") || node.InnerHtml.Contains("Yellow"))
        {
            return ConsoleColor.Yellow;
        }
        else if (isCyan)
        {
            return ConsoleColor.Cyan;
        }
        
        return ConsoleColor.White;
    }
    
    /// <summary>
    /// Extracts plain text content from an HTML node
    /// </summary>
    /// <param name="node">The HTML node to process</param>
    /// <returns>The extracted text content</returns>
    public static string ExtractTextContent(HtmlNode node)
    {
        if (node == null) return string.Empty;
        
        // Extract plain text
        var plainText = node.InnerText?.Trim();
        
        // Handle any page number links specially
        plainText = Regex.Replace(plainText ?? "", @"(\d{3})", m =>
        {
            if (int.TryParse(m.Value, out _))
            {
                return m.Value; // Return page numbers as is
            }
            return m.Value;
        });
        
        return plainText ?? string.Empty;
    }
    
    /// <summary>
    /// Extracts content from HTML with associated color information
    /// </summary>
    /// <param name="html">The HTML string to parse</param>
    /// <returns>Extracted text content</returns>
    public static string ExtractContentFromHtml(string html)
    {
        if (string.IsNullOrWhiteSpace(html) || html == "<div></div>")
        {
            return string.Empty;
        }
                
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var rootNode = doc.DocumentNode.SelectSingleNode("//div");
        
        if (rootNode == null)
        {
            return string.Empty;
        }
        
        return ExtractTextContent(rootNode);
    }
}
