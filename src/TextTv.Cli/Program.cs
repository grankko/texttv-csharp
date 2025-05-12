using TextTv.Cli.Services;
using TextTv.Cli.Rendering;

namespace TextTv.Cli;

/// <summary>
/// Main program class for the Text TV application
/// </summary>
class Program
{
    /// <summary>
    /// Entry-point. Expects a single argument: the page number to fetch.
    /// </summary>
    /// <example>
    /// dotnet run -- 100
    /// </example>
    static async Task<int> Main(string[] args)
    {
        if (args.Length != 1 || !int.TryParse(args[0], out var pageNumber))
        {
            Console.Error.WriteLine("Usage: texttv <page-number>");
            return 1;
        }

        try
        {
            // Set up services
            var httpClient = new HttpClient();
            var textTvService = new TextTvService(httpClient);
            var renderer = new TextTvRenderer();
            
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
