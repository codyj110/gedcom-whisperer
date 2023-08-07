namespace GedcomWhisperer.Models;

public class MultimediaObject
{
    public string Format { get; set; }
    public string Title { get; set; }
    public string File { get; set; }
    public string Date { get; set; }
    public string Type { get; set; }
    public string Note { get; set; }
    public List<string> CustomTags { get; set; }

    public MultimediaObject(TagObject multimediaTagObject)
    {
        Format = GedcomTags.GetSection("2", GedcomTags.ObjeTagForm, multimediaTagObject.InnerTags).Value;
        Title = GedcomTags.GetSection("2", GedcomTags.ObjeTagTitl, multimediaTagObject.InnerTags).Value;
        Type = GedcomTags.GetSection("3", GedcomTags.MultimediaTagType, multimediaTagObject.InnerTags).Value;
        File = multimediaTagObject.Value;
        Date = GedcomTags.GetSection("2", GedcomTags.DateTag, multimediaTagObject.InnerTags).Value;
        Note = GedcomTags.GetSection("2", GedcomTags.ObjeTagNote, multimediaTagObject.InnerTags).Value;
        CustomTags = GedcomTags.GetCustomSections("1", multimediaTagObject.InnerTags).Select(x => x).ToList();
    }
}
