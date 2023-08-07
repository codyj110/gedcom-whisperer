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
            var expectedCount = 233;
            
            // Act
            gedcom.LoadString(fileContent);
            var individualRecords = gedcom.IndividualRecords;
            // Assert
            Assert.Equal(expectedCount, individualRecords.IndividualRecordList.Count);
        }
        
    }
}