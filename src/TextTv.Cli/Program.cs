using TextTv.Cli.Services;
using TextTv.Cli.Rendering;
using TextTv.Cli.Configuration;
using TextTv.Cli.Services.Web;
using TextTv.Cli.Services.OpenAi;

namespace TextTv.Cli;

/// <summary>
/// Main program class for the Text TV application
/// </summary>
class Program
{
    /// <summary>
    /// Entry-point. Supports both page number and URL modes.
    /// </summary>
    /// <example>
    /// dotnet run -- 100
    /// dotnet run -- --pagenumber 100
    /// dotnet run -- --url https://example.com
    /// </example>
    static async Task<int> Main(string[] args)
    {
        // Parse command-line options
        var options = CommandLineOptions.Parse(args);
        
        // Show help if requested or if no valid options provided
        if (options.Mode == AppMode.Unknown)
        {
            CommandLineOptions.PrintUsage();
            return 1;
        }

        try
        {
            // Set up common services
            var httpClient = new HttpClient();
            var renderer = new TextTvRenderer();
            
            // Load settings (needed for URL mode)
            var settingsProvider = new SettingsProvider();
            settingsProvider.EnsureSettingsFileExists();
            var settings = settingsProvider.LoadSettings();
            
            // Execute the appropriate mode
            var result = await ExecuteRequestedMode(options, httpClient, renderer, settings);
            
            // Ensure console colors are properly reset before exit
            Console.ResetColor();
            return result;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
            return 1;
        }
    }
    
    /// <summary>
    /// Executes the requested application mode
    /// </summary>
    private static async Task<int> ExecuteRequestedMode(
        CommandLineOptions options, 
        HttpClient httpClient, 
        TextTvRenderer renderer,
        Settings settings)
    {
        switch (options.Mode)
        {
            case AppMode.PageNumber:
                return await ExecutePageNumberMode(options.PageNumber!.Value, httpClient, renderer);
            
            case AppMode.Url:
                if (string.IsNullOrEmpty(options.Url))
                {
                    Console.Error.WriteLine("URL is required for URL mode.");
                    return 1;
                }
                return await ExecuteUrlMode(options.Url, httpClient, renderer, settings);
                
            default:
                Console.Error.WriteLine("Invalid application mode.");
                return 1;
        }
    }
    
    /// <summary>
    /// Executes the page number mode (original functionality)
    /// </summary>
    private static async Task<int> ExecutePageNumberMode(
        int pageNumber, 
        HttpClient httpClient, 
        TextTvRenderer renderer)
    {
        var textTvService = new TextTvService(httpClient);
        
        // Fetch and render page
        var textTvPage = await textTvService.FetchPageAsync(pageNumber);
        if (textTvPage is null)
        {
            Console.WriteLine($"Page {pageNumber} not found.");
            return 1;
        }

        renderer.Render(textTvPage);
        return 0;
    }
    
    /// <summary>
    /// Executes the URL mode (new functionality)
    /// </summary>
    private static async Task<int> ExecuteUrlMode(
        string url, 
        HttpClient httpClient, 
        TextTvRenderer renderer,
        Settings settings)
    {
        try
        {
            // Check if API key is provided
            if (string.IsNullOrWhiteSpace(settings.OpenAiApiKey))
            {
                Console.Error.WriteLine("OpenAI API key is required for URL mode.");
                Console.Error.WriteLine("Please update the appsettings.json file with your API key.");
                return 1;
            }
            
            // Create required services
            var webContentService = new WebContentService(httpClient);
            var openAiService = new OpenAiService(settings);
            
            // Fetch web content
            var webContent = await webContentService.FetchWebContentAsync(url);
            if (string.IsNullOrWhiteSpace(webContent))
            {
                Console.WriteLine("Failed to extract content from the provided URL.");
                return 1;
            }
            
            // Process with OpenAI
            var textTvPage = await openAiService.ConvertWebToTextTvAsync(url, webContent);
            if (textTvPage is null)
            {
                Console.WriteLine("Failed to convert web content to TextTV format.");
                return 1;
            }
            
            // Render the TextTV page
            renderer.Render(textTvPage);
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in URL mode: {ex.Message}");
            return 1;
        }
    }
}
