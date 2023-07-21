namespace GedcomWhisperer.Models;

public class LoadEventArgs
{
    public string RawDataString { get; set; }

    public LoadEventArgs(string rawDataString)
    {
        RawDataString = rawDataString;
    }
}