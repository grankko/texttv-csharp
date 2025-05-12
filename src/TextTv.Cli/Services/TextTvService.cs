using System.Text.Json;
using TextTv.Cli.Models;
using TextTv.Cli.Helpers;

namespace TextTv.Cli.Services;

/// <summary>
/// Service for interacting with the Text TV API
/// </summary>
public class TextTvService
{
    private readonly HttpClient _httpClient;
    
    /// <summary>
    /// Creates a new TextTvService with the provided HttpClient
    /// </summary>
    public TextTvService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    /// <summary>
    /// Fetches a Text TV page from the API
    /// </summary>
    /// <param name="pageNumber">The page number to fetch</param>
    /// <returns>A TextTvPage object if successful, null otherwise</returns>
    public async Task<TextTvPage?> FetchPageAsync(int pageNumber)
    {
        try
        {
            var url = Utils.GetTextTvApiUrl(pageNumber);
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API request failed with status code: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var pages = JsonSerializer.Deserialize<TextTvPage[]>(content, Utils.DefaultJsonSerializerOptions);
            
            if (pages == null || pages.Length == 0)
            {
                Console.WriteLine("No pages found in response");
                return null;
            }

            return pages[0];
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching page: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return null;
        }
    }
}
