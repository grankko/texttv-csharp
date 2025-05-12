namespace TextTv.Cli.Models;

/// <summary>
/// Represents a Text TV page with its content and navigation information
/// </summary>
public sealed record TextTvPage(
    string Num,
    string[] Content, 
    string NextPage,
    string PrevPage
);
