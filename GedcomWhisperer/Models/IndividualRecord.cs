using System.Globalization;
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
    
    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        var individualRecordObject = GedcomTags.GetSection(_level, _tag, dataLines);
        var nameObject = GedcomTags.GetSection("1", "NAME", individualRecordObject.InnerTags);

        Name = new Name
        {
            Value = nameObject.Value,
            GivenName = GedcomTags.GetSection("2", "GIVN", nameObject.InnerTags).Value,
            Surname = GedcomTags.GetSection("2", "SURN", nameObject.InnerTags).Value,
            Source = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags).Select( x => x.Value).ToList()
        };
        
        Sex = GedcomTags.GetSection("1", "SEX", individualRecordObject.InnerTags).Value;
        Families = GedcomTags.GetSections("1", "FAMS", nameObject.InnerTags)
            .Select(x => x.Value).ToList();
        var birthdayObject = GedcomTags.GetSection("1", "BIRT", nameObject.InnerTags);
        
        string format = "d MMM yyyy";
        Birthday = new Birthday
        {
            Date = DateOnly.ParseExact(GedcomTags.GetSection("2", "DATE", birthdayObject.InnerTags).Value,
                format, CultureInfo.InvariantCulture),
            Place = GedcomTags.GetSection("2", "PLACE", birthdayObject.InnerTags).Value,
            Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags)
                .Select(x => x.Value).ToList()
        };
        
        var residenceObjct = GedcomTags.GetSection("1", "RESI", individualRecordObject.InnerTags);

        Residence = new Residence
        {
            Date = GedcomTags.GetSection("2", "DATE", residenceObjct.InnerTags).Value,
            Place = GedcomTags.GetSection("2", "PLACE", residenceObjct.InnerTags).Value,
            Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, residenceObjct.InnerTags)
                .Select(x => x.Value).ToList()
        };
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
}