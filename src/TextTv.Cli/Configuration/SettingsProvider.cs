using System.Text.Json;

namespace TextTv.Cli.Configuration;

/// <summary>
/// Provider for application settings
/// </summary>
public class SettingsProvider
{
    private readonly string _settingsFilePath;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Creates a new settings provider with the default settings file path
    /// </summary>
    public SettingsProvider() : this(GetDefaultSettingsPath()) { }

    /// <summary>
    /// Creates a new settings provider with a custom settings file path
    /// </summary>
    /// <param name="settingsFilePath">Path to the settings file</param>
    public SettingsProvider(string settingsFilePath)
    {
        _settingsFilePath = settingsFilePath;
    }

    /// <summary>
    /// Gets the default settings file path
    /// </summary>
    /// <returns>The default settings file path</returns>
    private static string GetDefaultSettingsPath()
    {
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        return Path.Combine(appDirectory, "appsettings.json");
    }

    /// <summary>
    /// Loads settings from the settings file
    /// </summary>
    /// <returns>The loaded settings</returns>
    public Settings LoadSettings()
    {
        EnsureSettingsFileExists();

        try
        {
            string json = File.ReadAllText(_settingsFilePath);
            var settings = JsonSerializer.Deserialize<Settings>(json, _jsonOptions);
            return settings ?? new Settings();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading settings: {ex.Message}");
            return new Settings();
        }
    }

    /// <summary>
    /// Ensures that the settings file exists
    /// </summary>
    public void EnsureSettingsFileExists()
    {
        if (!File.Exists(_settingsFilePath))
        {
            // We don't create the settings file automatically - it should be provided by the user
            Console.Error.WriteLine($"Settings file not found at {_settingsFilePath}");
            Console.Error.WriteLine("Please ensure the appsettings.json file exists with your OpenAI API key.");
        }
    }
}
