namespace GedcomWhisperer.Models;

public class MarriageDetails
{
    public string Date { get; set; }
    public string Place { get; set; }
    public string Source { get; set; }

    public MarriageDetails(TagObject marriageTagObject)
    {
        Date = GedcomTags.GetSection("2", GedcomTags.DateTag, marriageTagObject.InnerTags).Value;
        Place = GedcomTags.GetSection("2", GedcomTags.IndividualTagPlace, marriageTagObject.InnerTags).Value;
        Source = GedcomTags.GetSection("2", GedcomTags.SourceTag, marriageTagObject.InnerTags).Value;
    }
}