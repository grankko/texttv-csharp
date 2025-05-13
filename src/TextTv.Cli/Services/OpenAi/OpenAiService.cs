using OpenAI.Chat;
using System.Text.Json;
using TextTv.Cli.Configuration;
using TextTv.Cli.Models;

namespace TextTv.Cli.Services.OpenAi;

/// <summary>
/// Service for interacting with the OpenAI API
/// </summary>
public class OpenAiService
{
    private readonly Settings _settings;
    private readonly ChatClient _chatClient;
    
    /// <summary>
    /// Creates a new OpenAiService with the provided settings
    /// </summary>
    public OpenAiService(Settings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        
        if (string.IsNullOrWhiteSpace(_settings.OpenAiApiKey))
        {
            throw new InvalidOperationException(
                "OpenAI API key is missing. Please update your appsettings.json file with a valid API key.");
        }
        
        // Create the OpenAI ChatClient with the model and API key
        _chatClient = new ChatClient(
            model: _settings.OpenAiModel,
            apiKey: _settings.OpenAiApiKey);
    }
    
    /// <summary>
    /// Converts web content to TextTV format using OpenAI
    /// </summary>
    /// <param name="url">The URL that was fetched</param>
    /// <param name="webContent">The content from the web page</param>
    /// <returns>A TextTvPage object with the converted content</returns>
    public async Task<TextTvPage?> ConvertWebToTextTvAsync(string url, string webContent)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty", nameof(url));
        }
        
        if (string.IsNullOrWhiteSpace(webContent))
        {
            throw new ArgumentException("Web content cannot be null or empty", nameof(webContent));
        }
        
        try
        {
            // Prepare the chat messages
            var messages = new List<ChatMessage>
            {
                // Add the system message
                new SystemChatMessage(PromptConstants.TextTvFormatSystemPrompt),
                
                // Add the user message with the web content
                new UserChatMessage(string.Format(PromptConstants.TextTvFromUrlPromptTemplate, url, webContent))
            };
            
            // Call the OpenAI API with chat completion options
            var options = new ChatCompletionOptions
            {
                Temperature = 0.2f // Low temperature for consistent, formatted output
            };
            
            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages, options);
            
            if (completion == null || completion.Content.Count == 0)
            {
                Console.WriteLine("OpenAI returned an empty response.");
                return null;
            }
            
            string? responseContent = completion.Content[0].Text;
            
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                Console.WriteLine("OpenAI returned an empty text response.");
                return null;
            }
            
            // Parse the JSON response
            try
            {
                // The response might have triple backticks or other markdown, so let's clean it
                responseContent = CleanJsonResponse(responseContent);
                
                // Deserialize to a TextTvPage
                var textTvPage = JsonSerializer.Deserialize<TextTvPage>(
                    responseContent, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
                
                if (textTvPage == null)
                {
                    Console.WriteLine("Failed to deserialize the response to a TextTvPage object.");
                    return null;
                }
                
                return textTvPage;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to parse OpenAI response as TextTvPage: {ex.Message}");
                Console.WriteLine($"Response: {responseContent}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling OpenAI: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// Cleans the JSON response from OpenAI by removing markdown formatting
    /// </summary>
    /// <param name="response">The response string to clean</param>
    /// <returns>The cleaned response string</returns>
    private string CleanJsonResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            return string.Empty;
        }
        
        // Remove markdown code blocks if present
        if (response.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
        {
            response = response.Replace("```json", "", StringComparison.OrdinalIgnoreCase)
                             .Replace("```", "")
                             .Trim();
        }
        else if (response.StartsWith("```"))
        {
            response = response.Replace("```", "")
                             .Trim();
        }
        
        return response;
    }
}
