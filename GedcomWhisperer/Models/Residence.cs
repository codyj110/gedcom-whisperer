namespace GedcomWhisperer.Models;

public class Residence
{
    public string Date { get; set; }
    public string Place { get; set; }
    public List<string> Sources { get; set; }

    public Residence(TagObject parentTagObject)
    {
        var residenceObjct = GedcomTags.GetSection("1", GedcomTags.ResidenceTagResi, parentTagObject.InnerTags);
        Date = GedcomTags.GetSection("2", GedcomTags.DateTag, residenceObjct.InnerTags).Value;
        Place = GedcomTags.GetSection("2", GedcomTags.IndividualTagPlace, residenceObjct.InnerTags).Value;
        Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, residenceObjct.InnerTags)
            .Select(x => x.Value).ToList();
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Residence other)
        {
            return Date.Equals(other.Date) &&
                   Place == other.Place &&
                   Sources.SequenceEqual(other.Sources);
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Date.GetHashCode();
            hashCode = (hashCode * 397) ^ (Place != null ? Place.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Sources != null ? Sources.GetHashCode() : 0);
            return hashCode;
        }
    }

    public static bool operator ==(Residence residence1, Residence residence2)
    {
        return residence1.Equals(residence2);
    }

    public static bool operator !=(Residence residence1, Residence residence2)
    {
        return !residence1.Equals(residence2);
    }
}