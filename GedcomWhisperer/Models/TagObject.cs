namespace GedcomWhisperer.Models;

public class TagObject
{
    public string Value { get; set; }
    public List<string> InnerTags { get; set; } = new List<string>();
}