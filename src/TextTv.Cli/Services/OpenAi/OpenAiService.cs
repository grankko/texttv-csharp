using Azure.AI.OpenAI;
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
    private readonly OpenAIClient _client;
    
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
        
        _client = new OpenAIClient(_settings.OpenAiApiKey);
    }
    
    /// <summary>
    /// Converts web content to TextTV format using OpenAI
    /// </summary>
    /// <param name="url">The URL that was fetched</param>
    /// <param name="webContent">The content from the web page</param>
    /// <returns>A TextTvPage object with the converted content</returns>
    public async Task<TextTvPage?> ConvertWebToTextTvAsync(string url, string webContent)
    {
        try
        {
            // Prepare the chat completion options
            var chatCompletionOptions = new ChatCompletionsOptions
            {
                DeploymentName = _settings.OpenAiModel,
                Temperature = 0.2f, // Low temperature for consistent, formatted output
                MaxTokens = 1000   // Limit the response size
            };
            
            // Add the system message
            chatCompletionOptions.Messages.Add(new ChatRequestSystemMessage(PromptConstants.TextTvFormatSystemPrompt));
            
            // Add the user message with the web content
            var prompt = string.Format(PromptConstants.TextTvFromUrlPromptTemplate, url, webContent);
            chatCompletionOptions.Messages.Add(new ChatRequestUserMessage(prompt));
            
            // Call the OpenAI API
            var response = await _client.GetChatCompletionsAsync(chatCompletionOptions);
            if (response.Value.Choices.Count == 0)
            {
                Console.WriteLine("OpenAI returned an empty response.");
                return null;
            }
            
            var responseContent = response.Value.Choices[0].Message.Content;
            
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
    private string CleanJsonResponse(string response)
    {
        // Remove markdown code blocks if present
        if (response.StartsWith("```json"))
        {
            response = response.Replace("```json", "").Replace("```", "").Trim();
        }
        else if (response.StartsWith("```"))
        {
            response = response.Replace("```", "").Trim();
        }
        
        return response;
    }
}
