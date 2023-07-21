using GedcomWhisperer.Models;

namespace GedcomWhisperer;

public delegate void LoadStringEventHandler(object sender, LoadEventArgs e);

public class Gedcom
{
    public event LoadStringEventHandler LoadStringEvent;
    
    private string _rawData;

    public readonly Header Header = new Header();
    public readonly IndividualRecords IndividualRecords = new IndividualRecords();
    public readonly Submitter Submitter = new Submitter();

 
    public Gedcom()
    {
        LoadStringEvent += Header.HandleLoadStringEvent;
        LoadStringEvent += Submitter.HandleLoadStringEvent;
        LoadStringEvent += IndividualRecords.HandleLoadStringEvent;
    }

    public void LoadFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            _rawData = reader.ReadToEnd();
            
            LoadString(_rawData);
        }
    }

    public void LoadString(string rawData)
    {
        OnLoadStringEvent(rawData);
    }

    protected virtual void OnLoadStringEvent(string rawData)
    {
        LoadEventArgs args = new LoadEventArgs(rawData);

        LoadStringEvent?.Invoke(this, args);
    }
}