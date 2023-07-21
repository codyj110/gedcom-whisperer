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

            newIndividualRecordObject.Name = new Name
            {
                Value = nameObject.Value,
                GivenName = GedcomTags.GetSection("2", "GIVN", nameObject.InnerTags).Value,
                Surname = GedcomTags.GetSection("2", "SURN", nameObject.InnerTags).Value,
                Source = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags).Select( x => x.Value).ToList()
            };
        
            newIndividualRecordObject.Sex = GedcomTags.GetSection("1", "SEX", tagObject.InnerTags).Value;
            newIndividualRecordObject.Families = GedcomTags.GetSections("1", "FAMS", tagObject.InnerTags)
                .Select(x => x.Value).ToList();
            var birthdayObject = GedcomTags.GetSection("1", "BIRT", tagObject.InnerTags);
        
            string format = "d MMM yyyy";
            newIndividualRecordObject.Birthday = new Birthday
            {
                Date = DateOnly.ParseExact(GedcomTags.GetSection("2", "DATE", birthdayObject.InnerTags).Value,
                    format, CultureInfo.InvariantCulture),
                Place = GedcomTags.GetSection("2", "PLACE", birthdayObject.InnerTags).Value,
                Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags)
                    .Select(x => x.Value).ToList()
            };
        
            var residenceObjct = GedcomTags.GetSection("1", "RESI", tagObject.InnerTags);
            newIndividualRecordObject.Residence = new Residence
            {
                Date = GedcomTags.GetSection("2", "DATE", residenceObjct.InnerTags).Value,
                Place = GedcomTags.GetSection("2", "PLACE", residenceObjct.InnerTags).Value,
                Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, residenceObjct.InnerTags)
                    .Select(x => x.Value).ToList()
            };
            
            IndividualRecordList.Add(newIndividualRecordObject);
        }
        
    }
}