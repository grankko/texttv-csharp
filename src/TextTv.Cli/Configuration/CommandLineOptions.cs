namespace TextTv.Cli.Configuration;

/// <summary>
/// Represents command-line options for the application
/// </summary>
public class CommandLineOptions
{
    /// <summary>
    /// Gets or sets the page number (TextTV mode)
    /// </summary>
    public int? PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the URL (URL mode)
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Gets the application mode based on the provided options
    /// </summary>
    public AppMode Mode => 
        Url != null ? AppMode.Url : 
        PageNumber != null ? AppMode.PageNumber : 
        AppMode.Unknown;

    /// <summary>
    /// Parses command line arguments into options
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Parsed command line options</returns>
    public static CommandLineOptions Parse(string[] args)
    {
        var options = new CommandLineOptions();

        if (args.Length == 0)
        {
            return options;
        }

        // If only one argument is provided without flags, treat it as page number (backwards compatibility)
        if (args.Length == 1 && !args[0].StartsWith("--"))
        {
            if (int.TryParse(args[0], out var pageNumber))
            {
                options.PageNumber = pageNumber;
            }
            return options;
        }

        // Parse named arguments
        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];

            if (arg == "--pagenumber" || arg == "-p")
            {
                if (i + 1 < args.Length && int.TryParse(args[i + 1], out var pageNumber))
                {
                    options.PageNumber = pageNumber;
                    i++; // Skip the value in the next iteration
                }
            }
            else if (arg == "--url" || arg == "-u")
            {
                if (i + 1 < args.Length)
                {
                    options.Url = args[i + 1];
                    i++; // Skip the value in the next iteration
                }
            }
            else if (arg == "--help" || arg == "-h")
            {
                // Help mode will be handled separately
            }
        }

        return options;
    }

    /// <summary>
    /// Prints usage information to the console
    /// </summary>
    public static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  texttv <page-number>");
        Console.WriteLine("  texttv --pagenumber <page-number>");
        Console.WriteLine("  texttv --url <url>");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  <page-number>             Text TV page number (default mode)");
        Console.WriteLine("  --pagenumber, -p <number> Text TV page number");
        Console.WriteLine("  --url, -u <url>           URL to convert to Text TV format");
        Console.WriteLine("  --help, -h                Show help information");
    }
}

/// <summary>
/// Represents the application mode
/// </summary>
public enum AppMode
{
    /// <summary>
    /// Unknown/invalid mode
    /// </summary>
    Unknown,

    /// <summary>
    /// Page number mode (fetches TextTV content from the API)
    /// </summary>
    PageNumber,

    /// <summary>
    /// URL mode (converts web content to TextTV format)
    /// </summary>
    Url
}
