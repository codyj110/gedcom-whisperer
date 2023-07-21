namespace GedcomWhisperer.Models;

public class FamilyRecords
{
    private string _tag = "FAM";
    private string _level = "0";
    public List<Family> Families = new List<Family>();


    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        var famRecordObject = GedcomTags.GetSections(_level, _tag, dataLines);

        foreach (var tagObject in famRecordObject)
        {
            var newFamily = new Family
            {
                Id = tagObject.Value,
                HusbandId = GedcomTags.GetSection("1", "HUSB", tagObject.InnerTags).Value,
                WifeId = GedcomTags.GetSection("1", "WIFE", tagObject.InnerTags).Value,
                ChildrenIds = GedcomTags.GetSections("1", "CHIL", tagObject.InnerTags).Select( x => x.Value).ToList()
            };
            
            this.Families.Add(newFamily);
        }
        
    }
}