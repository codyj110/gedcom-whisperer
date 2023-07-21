namespace GedcomWhisperer.Models;

public class Family
{
    public string Id { get; set; }
    public string HusbandId { get; set; }
    public string WifeId { get; set; }
    public List<string> ChildrenIds { get; set; }

    public Family()
    {
        ChildrenIds = new List<string>();
    }
}