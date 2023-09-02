using System.Text.RegularExpressions;

namespace GedcomWhisperer.Models;

public class Individuals
{
    public List<Individual> IndividualsList { get; set; } = new();
    private string _sectionHeaderPattern = "INDI";
    
    public void ParseFileStringEventHandler(object sender, ParseFileEventArgs parseFileEventArgs)
    {
        var fileLines = parseFileEventArgs.FileString.Split("\r\n");

        var individualLines = new List<string>();
        var inIndividual = false;
        foreach (var fileLine in fileLines)
        {

            if (!inIndividual)
            {
                if (IsLevelZero(fileLine))
                {
                    if (IsIndividualSection(fileLine))
                    {
                        inIndividual = true;
                        individualLines.Add(fileLine);
                    }
                }
            }
            else
            {
                if (IsLevelZero(fileLine))
                {
                    var newIndividual = new Individual(individualLines);
                    IndividualsList.Add(newIndividual);
                    inIndividual = false;
                    
                    if (IsIndividualSection(fileLine))
                    {
                        inIndividual = true;
                        individualLines.Add(fileLine);
                    }
                }
                else
                {
                    individualLines.Add(fileLine);
                }
            }
        }

        

    }
    
    private bool IsLevelZero(string line)
    {
        Match match = Regex.Match(line, "^0 ");

        return match.Success;
    }

    private bool IsIndividualSection(string line)
    {
        Match match = Regex.Match(line, _sectionHeaderPattern);

        return match.Success;
    }
}