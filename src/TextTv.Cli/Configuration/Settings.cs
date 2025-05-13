namespace TextTv.Cli.Configuration;

/// <summary>
/// Application settings
/// </summary>
public class Settings
{
    /// <summary>
    /// OpenAI API key used for the URL mode
    /// </summary>
    public string OpenAiApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// The OpenAI model to use for URL mode
    /// </summary>
    public string OpenAiModel { get; set; } = "gpt-4.1";
}
