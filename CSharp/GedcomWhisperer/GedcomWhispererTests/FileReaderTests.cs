using GedcomWhisperer;

namespace ReadFileTests;

public class FileReaderTests
{
    [Fact]
    public void LoadsFileAsString()
    {
        // setup
        var gedcomDoc = new GedcomDocument();
        var testFile = "gedcomSampleFile.GED";
        
        // act
        var fileString ="";
        gedcomDoc.ParseFileStringEvent += (sender, args) =>
        {
            fileString = args.FileString;
            // assert
            Assert.NotEmpty(fileString);
        };
        gedcomDoc.LoadGedcomFile(testFile);
        
        
        
    }
}