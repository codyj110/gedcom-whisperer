using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;

namespace GedcomWhisperer.Models;

public class IndividualRecord
{
    public Name Name { get; set; }
    public string Sex { get; set; }
    public List<string> Families { get; set; }
    public Birthday Birthday { get; set; }
    public Residence Residence { get; set; }
    
    public string   ChildFamilyId { get; set; }

    public IndividualRecord(TagObject individualRecordObject)
    {
        extractResidenceObject(individualRecordObject);
        extractFamilies(individualRecordObject);
        Name = new Name(individualRecordObject);
        Birthday = new Birthday(individualRecordObject);
        Sex = GedcomTags.GetSection("1", GedcomTags.IndividualTagSex, individualRecordObject.InnerTags).Value;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is IndividualRecord other)
        {
            return Name.Equals(other.Name) &&
                   Sex == other.Sex &&
                   Families.SequenceEqual(other.Families) &&
                   Birthday.Equals(other.Birthday) &&
                   Residence.Equals(other.Residence);
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Name.GetHashCode();
            hashCode = (hashCode * 397) ^ (Sex != null ? Sex.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Families != null ? Families.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Birthday.GetHashCode();
            hashCode = (hashCode * 397) ^ Residence.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(IndividualRecord record1, IndividualRecord record2)
    {
        return record1.Equals(record2);
    }

    public static bool operator !=(IndividualRecord record1, IndividualRecord record2)
    {
        return !record1.Equals(record2);
    }
    
    private void extractResidenceObject(TagObject tagObject)
    {
        Residence = new Residence(tagObject);
    }
    
    private  void extractFamilies(TagObject tagObject)
    {
        Families = GedcomTags.GetSections("1", GedcomTags.IndividualTagFams, tagObject.InnerTags)
            .Select(x => x.Value).ToList();
        ChildFamilyId = GedcomTags.GetSection("1", GedcomTags.IndividualTagFamc, tagObject.InnerTags).Value;
    }
    
}