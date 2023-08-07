using GedcomWhisperer.Models;

namespace GedcomWhisperer;

public delegate void LoadFileEventHandler(object sender, LoadFileEventArgs args);
public delegate void ParseFileStringEventHandler(object sender, ParseFileEventArgs args);


public class GedcomDocument
{
    private readonly FileReader _fileReader;
    public event ParseFileStringEventHandler ParseFileStringEvent;
    
    public GedcomDocument()
    {
        _fileReader = new FileReader();
        _fileReader.FileStringLoadedEvent += FileStringLoadedEventHandler;
    }

    public void LoadGedcomFile(string filePath)
    {
        _fileReader.LoadFile(filePath);
    }

    private void FileStringLoadedEventHandler(object sender, FileStringLoadedArgs args)
    {
        ParseFileStringEvent?.Invoke(this, new ParseFileEventArgs(args.FileString));
    }
}