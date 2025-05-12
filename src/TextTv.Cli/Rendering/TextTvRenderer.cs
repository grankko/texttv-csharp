using HtmlAgilityPack;
using TextTv.Cli.Models;
using TextTv.Cli.Helpers;

namespace TextTv.Cli.Rendering;

/// <summary>
/// Handles the rendering of Text TV pages to the console with proper formatting
/// </summary>
public class TextTvRenderer
{
    private readonly TextWriter _writer;
    
    /// <summary>
    /// Creates a new TextTvRenderer instance that writes to the Console
    /// </summary>
    public TextTvRenderer() : this(Console.Out) { }
    
    /// <summary>
    /// Creates a new TextTvRenderer instance that writes to the specified TextWriter
    /// </summary>
    /// <param name="writer">The TextWriter to use for output</param>
    public TextTvRenderer(TextWriter writer)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }
    
    /// <summary>
    /// Renders a Text TV page with appropriate colors and formatting
    /// </summary>
    public void Render(TextTvPage page)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Clear();
        Console.BackgroundColor = ConsoleColor.Black;
        

        
        // Choose the appropriate footer based on mode
        bool isUrlMode = page.Num.StartsWith("http", StringComparison.OrdinalIgnoreCase);
        if (isUrlMode)
        {
            RenderHeader(page.Num, true);
            RenderContent(page.Content);
            RenderUrlModeFooter();
        }
        else
        {
            RenderHeader(page.Num, false);
            RenderContent(page.Content);
            RenderPageNumberFooter();
        }
    }
    
    private void RenderHeader(string pageNumber, bool isUrlMode)
    {
        // Page header with page number and date
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{pageNumber} ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        if (!isUrlMode)
            Console.Write("SVT Text");
        else
            Console.Write("WEB TV");
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" {DateTime.Now:dddd d MMM yyyy}");
        
        // SVT Text logo in blue bar
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        
        if (!isUrlMode)
            Console.WriteLine("SVT TEXT".PadLeft(20).PadRight(40));
        else
            Console.WriteLine("WEB TV".PadLeft(20).PadRight(40));
        
        // Reset to black background
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine();
    }
    
    private void RenderContent(string[] content)
    {
        foreach (var line in content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(line) || line == "<div></div>")
                {
                    Console.WriteLine();
                    continue;
                }
                
                var doc = new HtmlDocument();
                doc.LoadHtml(line);
                var rootNode = doc.DocumentNode.SelectSingleNode("//div");
                
                if (rootNode == null)
                {
                    Console.WriteLine();
                    continue;
                }
                
                // Use TextParsingHelper to determine color and extract content
                ConsoleColor textColor = TextParsingHelper.DetermineTextColor(rootNode);
                string textContent = TextParsingHelper.ExtractTextContent(rootNode);
                
                if (!string.IsNullOrWhiteSpace(textContent))
                {
                    Console.ForegroundColor = textColor;
                    Console.WriteLine(textContent);
                }
                else
                {
                    Console.WriteLine();
                }
            }
            catch
            {
                // Fallback to plain output if HTML parsing fails
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(line);
            }
        }
    }
    
    /// <summary>
    /// Renders the footer for page number mode with navigation to other pages
    /// </summary>
    private void RenderPageNumberFooter()
    {
        // Bottom navigation bar
        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Inrikes 101 Utrikes 104 Innehåll 700".PadRight(40));
        
        // Reset colors
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }
    
    /// <summary>
    /// Renders the footer for URL mode without page number references
    /// </summary>
    private void RenderUrlModeFooter()
    {
        // Bottom navigation bar without page references
        Console.WriteLine();
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Web URL Mode".PadRight(40));
        
        // Reset colors
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
    }
}
