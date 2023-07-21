using GedcomWhisperer;

namespace GedcomWhispererTests;

public class LoadGedcomIndividualRecordsTests
{
    [Fact]
    public void LoadsString()
    {
        using (StreamReader reader = new StreamReader("Sample.ged"))
        {
            // Setup
            string fileContent = reader.ReadToEnd();
            var gedcom = new Gedcom();

            var expectedName = "novariseindustries.com Member Trees Submitter";
            // Act
            gedcom.LoadString(fileContent);
            var individualRecords = gedcom.IndividualRecords;
            // Assert
            Assert.Equal(10, individualRecords.IndividualRecordList.Count);
        }
        
    }
}