using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GedcomWhisperer.Models;

public static class GedcomTags
{
    public static readonly string SubmitTag = "SUBM";
    public static readonly string CharsetTag = "CHAR";
    public static readonly string GedcomTag = "GEDC";
    public static readonly string DateTag = "DATE";
    public static readonly string HeaderTag = "HEAD";
    public static readonly string SourceTag = "SOUR";
    
    public static readonly string IndividualTagIndi = "INDI";
    public static readonly string IndividualTagName = "NAME";
    public static readonly string IndividualTagSex = "SEX";
    public static readonly string IndividualTagBirt = "BIRT";
    public static readonly string IndividualTagDeat = "DEAT";
    public static readonly string IndividualTagFamc = "FAMC";
    public static readonly string IndividualTagFams = "FAMS";
    public static readonly string IndividualTagObje = "OBJE";
    public static readonly string IndividualTagPlace = "PLAC";
    
    public static readonly string FamilyTagFam = "FAM";
    public static readonly string FamilyTagHusb = "HUSB";
    public static readonly string FamilyTagWife = "WIFE";
    public static readonly string FamilyTagChil = "CHIL";
    public static readonly string FamilyTagMarr = "MARR";
    public static readonly string FamilyTagDiv = "DIV";
    public static readonly string FamilyTagObje = "OBJE";
    public static readonly string GivenTagGvn = "GIVN";
    public static readonly string SurnameTagSurn = "SURN";
    public static readonly string ResidenceTagResi = "RESI";
    
    public static readonly string ObjeTagObj = "OBJE";
    public static readonly string ObjeTagFile = "FILE";
    public static readonly string ObjeTagForm = "FORM";
    public static readonly string ObjeTagType = "TYPE";
    public static readonly string ObjeTagTitl = "TITL";
    public static readonly string ObjeTagRin = "RIN"; 
    public static readonly string ObjeTagNote = "NOTE";
    
    public static readonly string MultimediaTagForm = "FORM";
    public static readonly string MultimediaTagTitl = "TITL";
    public static readonly string MultimediaTagFile = "FILE";
    public static readonly string MultimediaTagType = "TYPE";
    public static readonly string MultimediaTagRin = "RIN";

    public static TagObject GetSection(string level, string tag, List<string> dataLines)
    {
        string exactPattern = $@"({level}) ";

        if ((tag == IndividualTagIndi || tag == SubmitTag) && level == "0")
        {
            exactPattern += $"(.+) ({tag})";
        }
        else
        {
            exactPattern += $"({tag})(?: (.+))?$";
        }
        
        string genericPattern = @"(\d+) (.+) ?(.+)";

        var result = new TagObject();

        var readSubProperties = false;
        foreach (var dataLine in dataLines)
        {
            Match exactMatch = Regex.Match(dataLine, exactPattern);
            Match genericMatch = Regex.Match(dataLine, genericPattern);

            if (exactMatch.Success)
            {
                readSubProperties = true;
                if (exactMatch.Groups.Count == 4)
                {
                    if ((tag == IndividualTagIndi || tag == SubmitTag) && level == "0")
                    {
                        result.Value = exactMatch.Groups[2].Value;
                    }
                    else
                    {
                        result.Value = exactMatch.Groups[3].Value;
                    }
                }
            }
            else if (genericMatch.Success && readSubProperties &&
                int.Parse(genericMatch.Groups[1].Value) > int.Parse(level))
            {
                result.InnerTags.Add(dataLine);
            }
            else if(readSubProperties)
            {
                break;
            }
            
        }

        return result;
    }
    
    public static List<TagObject> GetSections(string level, string tag, List<string> dataLines)
    {
        string exactPattern = $@"({level}) ";

        if ((tag == IndividualTagIndi || tag == SubmitTag || tag == FamilyTagFam || tag == IndividualTagObje) && level == "0")
        {
            exactPattern += $"(.+) ({tag})";
        }
        else
        {
            exactPattern += $"({tag})(?: (.+))?$";
        }
        
        string genericPattern = @"(\d+) (.+) ?(.+)";

        var currentResult = new TagObject();
        var result = new List<TagObject>();

        var readSubProperties = false;
        foreach (var dataLine in dataLines)
        {
            Match exactMatch = Regex.Match(dataLine, exactPattern);
            Match genericMatch = Regex.Match(dataLine, genericPattern);

            if (exactMatch.Success && readSubProperties)
            {
                result.Add(currentResult);
                currentResult = new TagObject();
                if (exactMatch.Groups.Count == 4)
                {
                    if ((tag == IndividualTagIndi || tag == SubmitTag || tag == FamilyTagFam || tag == IndividualTagObje) && level == "0")
                    {
                        currentResult.Value = exactMatch.Groups[2].Value;
                    }
                    else
                    {
                        currentResult.Value = exactMatch.Groups[3].Value;
                    }
                }
            }
            else if (exactMatch.Success)
            {
                readSubProperties = true;
                if (exactMatch.Groups.Count == 4)
                {
                    if ((tag == IndividualTagIndi || tag == SubmitTag || tag == FamilyTagFam || tag == IndividualTagObje) && level == "0")
                    {
                        currentResult.Value = exactMatch.Groups[2].Value;
                    }
                    else
                    {
                        currentResult.Value = exactMatch.Groups[3].Value;
                    }
                }
            }
            else if (genericMatch.Success && readSubProperties &&
                     int.Parse(genericMatch.Groups[1].Value) > int.Parse(level))
            {
                currentResult.InnerTags.Add(dataLine);
            }
            else if(readSubProperties)
            {
                readSubProperties = false;
                result.Add(currentResult);
                currentResult = new TagObject();
            }
            
        }

        return result;
    }
    
    public static List<string> GetCustomSections(string level, List<string> dataLines)
    {
        string exactPattern = $@"({level}) (_.+)(?: (.+))?$";
        string genericPattern = @"(\d+) (.+) ?(.+)";

        var result = new List<string>();

        var readSubProperties = false;
        foreach (var dataLine in dataLines)
        {
            Match exactMatch = Regex.Match(dataLine, exactPattern);
            Match genericMatch = Regex.Match(dataLine, genericPattern);

            if (exactMatch.Success)
            {
                readSubProperties = true;
                if (exactMatch.Groups.Count == 4)
                {
                    result.Add(dataLine);
                }
            }
            else if (genericMatch.Success && readSubProperties &&
                     int.Parse(genericMatch.Groups[1].Value) > int.Parse(level))
            {
                result.Add(dataLine);
            }
            else if(readSubProperties)
            {
                break;
            }
            
        }

        return result;
    }
}