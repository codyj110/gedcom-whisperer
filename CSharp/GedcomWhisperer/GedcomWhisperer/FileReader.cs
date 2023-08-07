using GedcomWhisperer.Models;

namespace GedcomWhisperer;

public delegate void FileStringLoadedEventHandler(object sender, FileStringLoadedArgs args);

internal class FileReader
{
    private string _rawData;
    private LoadFileEventHandler _loadFileEventHandler;
    internal event FileStringLoadedEventHandler FileStringLoadedEvent;

    public FileReader()
    {
    }
    
    internal void LoadFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            _rawData = reader.ReadToEnd();
            
            LoadString(_rawData);
        }
    }

    private void LoadString(string rawData)
    {
        FileStringLoadedEvent.Invoke(this, new FileStringLoadedArgs(rawData));
    }

}