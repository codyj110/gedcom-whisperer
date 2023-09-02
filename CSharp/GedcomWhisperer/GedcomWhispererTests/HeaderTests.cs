using GedcomWhisperer;

namespace ReadFileTests;


public class HeaderTests
{
    [Fact]
    public void LoadsFileAsString()
    {
        // setup
        var gedcomDoc = new GedcomDocument();
        var testFile = "gedcomSampleFile.GED";
        
        // act
        gedcomDoc.LoadGedcomFile(testFile);
        
        // assert
        Assert.NotEmpty(gedcomDoc._Header.HeaderLines);
    }
}