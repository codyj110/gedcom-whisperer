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
        var marriageDetails = GedcomTags.GetSection("1", GedcomTags.FamilyTagMarr, familyObject.InnerTags);
        Id = familyObject.Value;
        HusbandId = GedcomTags.GetSection("1", GedcomTags.FamilyTagHusb, familyObject.InnerTags).Value;
        WifeId = GedcomTags.GetSection("1", GedcomTags.FamilyTagWife, familyObject.InnerTags).Value;
        ChildrenIds = GedcomTags.GetSections("1", GedcomTags.FamilyTagChil, familyObject.InnerTags).Select(x => x.Value).ToList();
        Marriage = new MarriageDetails(marriageDetails);
    }
}