namespace GedcomWhisperer.Models;

public class MultimediaObjectRecords
{
    private string _tag = GedcomTags.FamilyTagObje;
    private string _level = "0";
    public List<MultimediaObject> MultimediaObjects { get; set; } = new ();
    
    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        var multimediaObjectRecord = GedcomTags.GetSections(_level, _tag, dataLines);

        foreach (var tagObject in multimediaObjectRecord)
        {
            var newIndividualRecordObject = new MultimediaObject(tagObject);

            MultimediaObjects.Add(newIndividualRecordObject);
        }
        
    }
}