namespace GedcomWhisperer.Models;

public class Family
{
    public string Id { get; set; }
    public string HusbandId { get; set; }
    public string WifeId { get; set; }
    public List<string> ChildrenIds { get; set; }
    public MarriageDetails Marriage { get; set; }

    public Family()
    {
        ChildrenIds = new List<string>();
    }

    public Family(TagObject familyObject)
    {
        var marriageDetails = GedcomTags.GetSection("1", "MARR", familyObject.InnerTags);
        Id = familyObject.Value;
        HusbandId = GedcomTags.GetSection("1", "HUSB", familyObject.InnerTags).Value;
        WifeId = GedcomTags.GetSection("1", "WIFE", familyObject.InnerTags).Value;
        ChildrenIds = GedcomTags.GetSections("1", "CHIL", familyObject.InnerTags).Select(x => x.Value).ToList();
        Marriage = new MarriageDetails(marriageDetails);
    }
}