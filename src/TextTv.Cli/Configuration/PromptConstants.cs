namespace TextTv.Cli.Configuration;

/// <summary>
/// Contains constants for prompts used in the OpenAI API requests
/// </summary>
public static class PromptConstants
{
    /// <summary>
    /// System prompt that describes how TextTV looks and how to format the response
    /// </summary>
    public const string TextTvFormatSystemPrompt = @"
You are a Swedish Text-TV (TextTV) page generator. Your task is to convert web content into the format of a Swedish Text-TV page.

TextTV has these key characteristics:
1. Strict space limitation - 40 characters wide and EXACTLY 22-23 lines maximum for content
2. No images, only text
3. Simple color coding:
   - Yellow for headlines (class='Y')
   - Cyan for subheadings or important secondary text (class='C')
   - White for regular body text (no class)

Your output MUST be a JSON object that exactly matches this structure:
{
  ""Num"": ""URL"",
  ""Content"": [
    ""<div class='Y'>HEADLINE TEXT HERE</div>"",
    ""<div class='C'>SUBHEADING TEXT HERE</div>"",
    ""<div>REGULAR TEXT HERE</div>"",
    ... more content lines ...
  ],
  ""NextPage"": """",
  ""PrevPage"": """"
}

IMPORTANT INSTRUCTIONS:

1. STRICTLY LIMIT to 22-23 total lines in the Content array (including blank lines)
2. DO NOT reference page numbers for navigation - we cannot navigate to other pages in URL mode
3. Always leave NextPage and PrevPage as empty strings
4. Make efficient use of available space - don't waste lines
5. Focus on the most important information from the website
6. Use blank lines (empty strings) sparingly - only for separating major sections
7. Count your lines carefully - content must fit without scrolling

TextTV STYLING GUIDELINES:
- Main headline should be in yellow and centered
- Subheadings should be in cyan and usually left-aligned
- Regular text should be in white and left-aligned
- Be extremely concise but informative
- Use short, simple sentences
";

    /// <summary>
    /// User prompt template for converting a URL to a TextTV page
    /// </summary>
    public const string TextTvFromUrlPromptTemplate = @"
Convert the following web page content into a Swedish Text-TV page:

URL: {0}

CONTENT:
{1}

Here are examples of good TextTV pages in the expected output format:

EXAMPLE 1 - News website:
{{
  ""Num"": ""URL"",
  ""Content"": [
    ""<div class='Y'>DAGENS NYHETER</div>"",
    """",
    ""<div class='C'>HUVUDNYHETER</div>"",
    ""<div>* Regeringen presenterar ny budget med</div>"",
    ""<div>  fokus på klimatsatsningar</div>"",
    """",
    ""<div>* EU-kommissionen föreslår nya regler</div>"",
    ""<div>  för techföretag om datasäkerhet</div>"",
    """",
    ""<div class='C'>EKONOMI</div>"",
    ""<div>* Börsen stiger efter positiva USA-</div>"",
    ""<div>  siffror - OMX upp 2,1 procent</div>"",
    """",
    ""<div>* Riksbanken behåller räntan oförändrad</div>"",
    ""<div>  men signalerar höjning i september</div>"",
    """",
    ""<div class='C'>Sverige</div>"",
    ""<div>* Översvämningsrisk i södra Sverige</div>"",
    ""<div>  efter kraftiga regn senaste veckan</div>"",
    """",
    ""<div>* Nya regler för elsparkcyklar träder</div>"",
    ""<div>  i kraft från 1 augusti</div>""
  ],
  ""NextPage"": """",
  ""PrevPage"": """"
}}

EXAMPLE 2 - Corporate website:
{{
  ""Num"": ""URL"",
  ""Content"": [
    ""<div class='Y'>FÖRETAGSNAMN AB</div>"",
    """",
    ""<div class='C'>OM FÖRETAGET</div>"",
    ""<div>Grundat 1985 i Stockholm. Ledande</div>"",
    ""<div>inom mjukvaruutveckling med fokus på</div>"",
    ""<div>AI och maskininlärningslösningar.</div>"",
    """",
    ""<div class='C'>PRODUKTER & TJÄNSTER</div>"",
    ""<div>* AI-verktyg för dataanalys</div>"",
    ""<div>* Molnbaserade lagringslösningar</div>"",
    ""<div>* Konsulttjänster inom IT-säkerhet</div>"",
    """",
    ""<div class='C'>SENASTE NYTT</div>"",
    ""<div>Företaget expanderar till nya marknader</div>"",
    ""<div>i Asien med kontor i Singapore från</div>"",
    ""<div>och med september 2025.</div>"",
    """",
    ""<div class='C'>KONTAKT</div>"",
    ""<div>info@företag.se</div>"",
    ""<div>08-123 45 67</div>""
  ],
  ""NextPage"": """",
  ""PrevPage"": """"
}}

Now create a similar TextTV page for the URL I provided. Make sure to:
1. Include as much relevant content as possible
2. Fill at least 15-20 lines with useful information
3. DO NOT reference page numbers that don't exist
4. Return ONLY the JSON object with the TextTV content
";
}
