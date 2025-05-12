using HtmlAgilityPack;
using System.Text;

namespace TextTv.Cli.Services.Web;

/// <summary>
/// Service for retrieving and processing web content
/// </summary>
public class WebContentService
{
    private readonly HttpClient _httpClient;
    
    /// <summary>
    /// Creates a new WebContentService with the provided HttpClient
    /// </summary>
    public WebContentService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    /// <summary>
    /// Fetches and extracts content from a web page
    /// </summary>
    /// <param name="url">The URL to fetch content from</param>
    /// <returns>The extracted relevant content from the web page</returns>
    public async Task<string> FetchWebContentAsync(string url)
    {
        try
        {
            // Validate URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Invalid URL format", nameof(url));
            }
            
            // Fetch the web page content
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            
            // Parse and extract relevant content
            return ExtractRelevantContent(content);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching web page: {ex.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Extracts relevant content from HTML
    /// </summary>
    /// <param name="htmlContent">The raw HTML content</param>
    /// <returns>Extracted text content</returns>
    private string ExtractRelevantContent(string htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);
        
        var contentBuilder = new StringBuilder();
        
        // Extract title
        var titleNode = doc.DocumentNode.SelectSingleNode("//title");
        if (titleNode != null)
        {
            contentBuilder.AppendLine($"TITLE: {titleNode.InnerText.Trim()}");
            contentBuilder.AppendLine();
        }
        
        // Extract meta description
        var metaDescription = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
        if (metaDescription != null)
        {
            var content = metaDescription.GetAttributeValue("content", "");
            if (!string.IsNullOrWhiteSpace(content))
            {
                contentBuilder.AppendLine($"DESCRIPTION: {content.Trim()}");
                contentBuilder.AppendLine();
            }
        }
        
        // Extract main content
        // First try to get content from main, article, or specific content containers
        var mainContent = doc.DocumentNode.SelectSingleNode("//main") ?? 
                         doc.DocumentNode.SelectSingleNode("//article") ?? 
                         doc.DocumentNode.SelectSingleNode("//*[@id='content']") ??
                         doc.DocumentNode.SelectSingleNode("//*[@class='content']");
        
        if (mainContent != null)
        {
            ExtractTextFromNode(mainContent, contentBuilder);
        }
        else
        {
            // Fallback to body if no specific content container found
            var body = doc.DocumentNode.SelectSingleNode("//body");
            if (body != null)
            {
                // Remove script, style, and other non-content elements
                var scriptsAndStyles = body.SelectNodes("//script|//style|//nav|//footer|//header");
                if (scriptsAndStyles != null)
                {
                    foreach (var node in scriptsAndStyles)
                    {
                        node.Remove();
                    }
                }
                
                ExtractTextFromNode(body, contentBuilder);
            }
        }
        
        return contentBuilder.ToString();
    }
    
    /// <summary>
    /// Extracts text content from a node and its children
    /// </summary>
    private void ExtractTextFromNode(HtmlNode node, StringBuilder builder)
    {
        // Extract headers
        var headers = node.SelectNodes(".//h1|.//h2|.//h3|.//h4");
        if (headers != null)
        {
            foreach (var header in headers)
            {
                var headerText = header.InnerText.Trim();
                if (!string.IsNullOrWhiteSpace(headerText))
                {
                    builder.AppendLine($"HEADER: {headerText}");
                }
            }
            builder.AppendLine();
        }
        
        // Extract paragraphs
        var paragraphs = node.SelectNodes(".//p");
        if (paragraphs != null)
        {
            foreach (var paragraph in paragraphs)
            {
                var paragraphText = paragraph.InnerText.Trim();
                if (!string.IsNullOrWhiteSpace(paragraphText))
                {
                    builder.AppendLine(paragraphText);
                    builder.AppendLine();
                }
            }
        }
        
        // If no structured content found, just get all text
        if ((headers == null || headers.Count == 0) && (paragraphs == null || paragraphs.Count == 0))
        {
            var text = node.InnerText;
            if (!string.IsNullOrWhiteSpace(text))
            {
                // Clean up the text (remove excessive whitespace, etc.)
                text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();
                builder.AppendLine(text);
            }
        }
    }
}
