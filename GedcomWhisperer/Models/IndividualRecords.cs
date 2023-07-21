using System.Globalization;

namespace GedcomWhisperer.Models;

public class IndividualRecords
{
    private string _tag = GedcomTags.IndividualRecordTag;
    private string _level = "0";
    public List<IndividualRecord> IndividualRecordList { get; set; } = new List<IndividualRecord>();
    
    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        var individualRecordObject = GedcomTags.GetSections(_level, _tag, dataLines);

        foreach (var tagObject in individualRecordObject)
        {
            var newIndividualRecordObject = new IndividualRecord();
            
            var nameObject = GedcomTags.GetSection("1", "NAME", tagObject.InnerTags);
            extractName(newIndividualRecordObject, nameObject);
        
            extractSex(newIndividualRecordObject, tagObject);
            
            extractFamilies(newIndividualRecordObject, tagObject);
            
            extractBirthday(newIndividualRecordObject, tagObject, nameObject);

            extractResidenceObject(tagObject, newIndividualRecordObject);

            IndividualRecordList.Add(newIndividualRecordObject);
        }
        
    }

    private static void extractResidenceObject(TagObject tagObject, IndividualRecord newIndividualRecordObject)
    {
        var residenceObjct = GedcomTags.GetSection("1", "RESI", tagObject.InnerTags);
        newIndividualRecordObject.Residence = new Residence
        {
            Date = GedcomTags.GetSection("2", "DATE", residenceObjct.InnerTags).Value,
            Place = GedcomTags.GetSection("2", "PLACE", residenceObjct.InnerTags).Value,
            Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, residenceObjct.InnerTags)
                .Select(x => x.Value).ToList()
        };
    }

    private static void extractFamilies(IndividualRecord newIndividualRecordObject, TagObject tagObject)
    {
        newIndividualRecordObject.Families = GedcomTags.GetSections("1", "FAMS", tagObject.InnerTags)
            .Select(x => x.Value).ToList();
        newIndividualRecordObject.ChildFamilyId = GedcomTags.GetSection("1", "FAMC", tagObject.InnerTags).Value;
    }

    private static void extractSex(IndividualRecord newIndividualRecordObject, TagObject tagObject)
    {
        newIndividualRecordObject.Sex = GedcomTags.GetSection("1", "SEX", tagObject.InnerTags).Value;
    }

    private static void extractName(IndividualRecord newIndividualRecordObject, TagObject nameObject)
    {
        newIndividualRecordObject.Name = new Name
        {
            Value = nameObject.Value,
            GivenName = GedcomTags.GetSection("2", "GIVN", nameObject.InnerTags).Value,
            Surname = GedcomTags.GetSection("2", "SURN", nameObject.InnerTags).Value,
            Source = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags).Select(x => x.Value).ToList()
        };
    }
    
    private static void extractBirthday(IndividualRecord newIndividualRecordObject, TagObject tagObject, TagObject nameObject)
    {
        var birthdayObject = GedcomTags.GetSection("1", "BIRT", tagObject.InnerTags);

        if (birthdayObject.InnerTags.Count != 0)
        {
            try
            {
                string format = "d MMM yyyy";
                DateOnly.TryParseExact(
                    GedcomTags.GetSection("2", "DATE", birthdayObject.InnerTags).Value,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date);
                newIndividualRecordObject.Birthday = new Birthday
                {
                    Date = date,
                    Place = GedcomTags.GetSection("2", "PLACE", birthdayObject.InnerTags).Value,
                    Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags)
                        .Select(x => x.Value).ToList()
                };
            }
            catch (FormatException exception)
            {
                string format = "MMM d yyyy";

                DateTime.TryParseExact(GedcomTags.GetSection("2", "DATE", birthdayObject.InnerTags).Value, format, null,
                    DateTimeStyles.None,
                    out var dateTime);
                DateOnly date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                newIndividualRecordObject.Birthday = new Birthday
                {
                    Date = date,
                    Place = GedcomTags.GetSection("2", "PLACE", birthdayObject.InnerTags).Value,
                    Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags)
                        .Select(x => x.Value).ToList()
                };
            }
        }
    }
}