namespace GedcomWhisperer.Models;

public class Submitter
{
    private string _tag = GedcomTags.SubmitTag;
    private string _level = "0";
    public string Name { get; set; }
    
    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        var submitObject = GedcomTags.GetSection(_level, _tag, dataLines);
        Name = GedcomTags.GetSection("1", GedcomTags.IndividualTagName, submitObject.InnerTags).Value;
    }
}