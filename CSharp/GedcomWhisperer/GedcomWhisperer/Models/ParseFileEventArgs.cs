namespace GedcomWhisperer.Models;

public class ParseFileEventArgs
{
    public string FileString { get; set; }

    public ParseFileEventArgs(string fileString)
    {
        FileString = fileString;
    }
}