using GedcomWhisperer;

namespace GedcomWhispererTests;

public class LoadGedcomFamilyRecordsTests
{
    [Fact]
    public void LoadsString()
    {
        using (StreamReader reader = new StreamReader("Sample.ged"))
        {
            // Setup
            string fileContent = reader.ReadToEnd();
            var gedcom = new Gedcom();
            var expectedCount = 84;
            
            // Act
            gedcom.LoadString(fileContent);
            var familyRecords = gedcom.FamilyRecords;
            // Assert
            Assert.Equal(expectedCount, familyRecords.Families.Count);
        }
        
    }
}