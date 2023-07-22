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
            var newIndividualRecordObject = new IndividualRecord(tagObject);

            IndividualRecordList.Add(newIndividualRecordObject);
        }
        
    }
}