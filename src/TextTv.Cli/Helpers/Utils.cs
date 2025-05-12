using System.Text.Json;
using System.Net.Http;

namespace TextTv.Cli.Helpers;

/// <summary>
/// Utility methods for the Text TV application
/// </summary>
public static class Utils 
{
    /// <summary>
    /// Default JSON serializer options for API requests
    /// </summary>
    public static JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions 
    {
        PropertyNameCaseInsensitive = true,
    };
    
    /// <summary>
    /// Builds the API URL for fetching a Text TV page
    /// </summary>
    /// <param name="pageNumber">The page number to fetch</param>
    /// <returns>The complete URL for the API request</returns>
    public static string GetTextTvApiUrl(int pageNumber)
    {
        return $"https://texttv.nu/api/get/{pageNumber}?includePlainTextContent=1";
    }
    
    /// <summary>
    /// Extracts page numbers from text (useful for links)
    /// </summary>
    /// <param name="text">Text that may contain page numbers</param>
    /// <returns>List of page numbers found in the text</returns>
    public static IEnumerable<int> ExtractPageNumbers(string text)
    {
        if (string.IsNullOrEmpty(text)) yield break;
        
        // Match 3-digit numbers that could be page numbers
        var matches = System.Text.RegularExpressions.Regex.Matches(text, @"\b(\d{3})\b");
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            if (int.TryParse(match.Value, out int pageNumber))
            {
                yield return pageNumber;
            }
        }
    }
}
