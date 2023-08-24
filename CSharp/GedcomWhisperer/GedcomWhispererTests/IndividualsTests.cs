using GedcomWhisperer;

namespace ReadFileTests;

public class IndividualsTests
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
        Assert.NotEmpty(gedcomDoc._Individuals.IndividualsList);
    }
    
    [Fact]
    public void LoadsIds()
    {
        // setup
        var gedcomDoc = new GedcomDocument();
        var testFile = "gedcomSampleFile.GED";

        var expectedIndividuals = 3;
        
        // act
        gedcomDoc.LoadGedcomFile(testFile);
        
        // assert
        Assert.Equal(expectedIndividuals, gedcomDoc._Individuals.IndividualsList.Count);
        Assert.All(gedcomDoc._Individuals.IndividualsList, obj => Assert.NotNull(obj.Id));
    }
}