namespace GedcomWhisperer.Models;

public class LoadFileEventArgs
{
    public string FilePath { get; set; }

    public LoadFileEventArgs(string filePath)
    {
        FilePath = filePath;
    }
    
}