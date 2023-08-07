namespace GedcomWhisperer.Models;

public class FileStringLoadedArgs
{
    public string FileString { get; set; }
    
    public FileStringLoadedArgs(string fileString)
    {
        FileString = fileString;
    }
}