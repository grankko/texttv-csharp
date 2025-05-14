# TextTV Viewer

A command-line application for viewing Text TV pages from the texttv.nu API and for converting web pages to TextTV format using OpenAI's GPT-4.1.

## Usage

The application supports automatic detection of input type:

```bash
# View TextTV page 100
./texttv 100

# Convert a web page to TextTV format
./texttv https://example.com
```

### Explicit Mode Options

You can also use explicit flags:

```bash
# TextTV mode (view page 100)
./texttv --pagenumber 100
./texttv -p 100

# URL mode (convert web page)
./texttv --url https://example.com
./texttv -u https://example.com

# Show help information
./texttv --help
./texttv -h
```

## Configuration

For URL mode, an OpenAI API key is required in the `appsettings.json` file. If this file doesn't exist, the application will create one based on the template, but you'll need to add your actual API key.

Example `appsettings.json`:
```json
{
  "OpenAiApiKey": "your-openai-api-key-here",
  "OpenAiModel": "gpt-4.1"
}
```

## Project Links

- Source Code: https://github.com/grankko/texttv-csharp
- Issues: https://github.com/grankko/texttv-csharp/issues
