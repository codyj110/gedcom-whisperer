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
    public static readonly string IndividualRecordTag = "INDI";
    public static readonly string FamilyRecordTag = "FAM";

    public static TagObject GetSection(string level, string tag, List<string> dataLines)
    {
        string exactPattern = $@"({level}) ";

        if ((tag == IndividualRecordTag || tag == SubmitTag) && level == "0")
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
                    if ((tag == IndividualRecordTag || tag == SubmitTag) && level == "0")
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

        if ((tag == IndividualRecordTag || tag == SubmitTag || tag == FamilyRecordTag) && level == "0")
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
                    if ((tag == IndividualRecordTag || tag == SubmitTag || tag == FamilyRecordTag) && level == "0")
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
                    if ((tag == IndividualRecordTag || tag == SubmitTag || tag == FamilyRecordTag) && level == "0")
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