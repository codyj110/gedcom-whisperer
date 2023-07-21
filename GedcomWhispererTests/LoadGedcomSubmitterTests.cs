using System.Globalization;
using GedcomWhisperer;

namespace GedcomWhispererTests;

public class LoadGedcomSubmitterTests
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
            var submitter = gedcom.Submitter;
            // Assert
            Assert.Equal(expectedName, submitter.Name);
        }
        
    }
}