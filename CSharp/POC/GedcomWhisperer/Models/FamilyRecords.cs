namespace GedcomWhisperer.Models;

public class FamilyRecords
{
    private string _tag = GedcomTags.FamilyTagFam;
    private string _level = "0";
    public List<Family> Families = new List<Family>();


    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        var famRecordObject = GedcomTags.GetSections(_level, _tag, dataLines);

        foreach (var tagObject in famRecordObject)
        {
            var newFamily = new Family(tagObject);    
            
            Families.Add(newFamily);
        }
        
    }
}