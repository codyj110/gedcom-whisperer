using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;

namespace GedcomWhisperer.Models;

// 0 @I180122401009@ INDI
//  1 NAME Cody Ross /Johnson/
//   2 GIVN Cody Ross
//   2 SURN Johnson
//   2 SOUR @S425541580@
//    3 _APID 1,70802::750759
//   2 SOUR @S474978771@
//    3 _APID 1,62209::155258866
//  1 SEX M
//  1 FAMC @F5@
//  1 FAMS @F73@
//  1 BIRT
//   2 DATE 11 Jul 1985
//   2 PLAC New Albany, Floyd, Indiana, USA
//   2 SOUR @S474978771@
//    3 _APID 1,62209::155258866
//  1 RESI
//   2 DATE 2002-2020
//   2 PLAC Floyds Knobs, Indiana, USA
//   2 SOUR @S474978771@
//    3 _APID 1,62209::155258866

public class IndividualRecord
{
    private string _tag = GedcomTags.IndividualRecordTag;
    private string _level = "0";
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
        Sex = GedcomTags.GetSection("1", "SEX", individualRecordObject.InnerTags).Value;
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
        Families = GedcomTags.GetSections("1", "FAMS", tagObject.InnerTags)
            .Select(x => x.Value).ToList();
        ChildFamilyId = GedcomTags.GetSection("1", "FAMC", tagObject.InnerTags).Value;
    }
    
}