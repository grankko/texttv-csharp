namespace TextTv.Cli.Models;

/// <summary>
/// Represents a Text TV page with its content and navigation information
/// </summary>
public sealed record TextTvPage(
    string Num,
    string[] Content, 
    string? NextPage = null,
    string? PrevPage = null
)
{
    /// <summary>
    /// Ensures that Content array is never null
    /// </summary>
    public TextTvPage() : this(string.Empty, Array.Empty<string>(), null, null)
    {
    }

    /// <summary>
    /// Gets the page number or URL
    /// </summary>
    public string Num { get; init; } = Num ?? string.Empty;

    /// <summary>
    /// Gets the content of the page as an array of strings
    /// </summary>
    public string[] Content { get; init; } = Content ?? Array.Empty<string>();

    /// <summary>
    /// Gets the next page number or URL, if available
    /// </summary>
    public string? NextPage { get; init; } = NextPage;

    /// <summary>
    /// Gets the previous page number or URL, if available
    /// </summary>
    public string? PrevPage { get; init; } = PrevPage;
};
