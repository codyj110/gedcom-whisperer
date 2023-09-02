using System.Globalization;
using System.Text.RegularExpressions;

namespace GedcomWhisperer.Models;

public class Header
{
    public List<string> HeaderLines { get; set; }
    public Gedc Gedc { get; set; } = new();
    
    private string _sectionHeaderPattern = "HEAD$";
    public void ParseFileStringEventHandler(object sender, ParseFileEventArgs parseFileEventArgs)
    {
        var fileLines = parseFileEventArgs.FileString.Split("\r\n");

        var headerLines = new List<string>();
        var inHeader = false;
        foreach (var fileLine in fileLines)
        {

            if (!inHeader)
            {
                if (IsLevelZero(fileLine))
                {
                    if (IsHeaderSection(fileLine))
                    {
                        inHeader = true;
                        headerLines.Add(fileLine);
                    }
                }
            }
            else
            {
                if (IsLevelZero(fileLine))
                {
                    break;
                }

                headerLines.Add(fileLine);
            }
        }

        HeaderLines = headerLines;

        DeserializeFrom(HeaderLines);

    }

    private bool IsLevelZero(string line)
    {
        Match match = Regex.Match(line, "^0 ");

        return match.Success;
    }

    private bool IsHeaderSection(string line)
    {
        Match match = Regex.Match(line, _sectionHeaderPattern);

        return match.Success;
    }

    public void DeserializeFrom(List<string> lines)
    {
        Gedc.Vers = lines.Find(l => l.StartsWith("2 VERS"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Form.Value = lines.Find(l => l.StartsWith("2 FORM"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Form.Vers = lines.Find(l => l.StartsWith("3 VERS"))?.Split(" ")[2] ?? string.Empty;
        
        Gedc.Encoding = lines.Find(l => l.StartsWith("1 CHAR"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Language = lines.Find(l => l.StartsWith("1 LANG"))?.Split(" ")[2] ?? string.Empty;
        Gedc.FileName = lines.Find(l => l.StartsWith("1 FILE"))?.Split(" ")[2] ?? string.Empty;
        
        Gedc.Source.Value = lines.Find(l => l.StartsWith("1 SOUR"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Source.Name = lines.Find(l => l.StartsWith("2 NAME"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Source.Version = lines.Find(l => l.StartsWith("2 VERS"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Source.Organization = lines.Find(l => l.StartsWith("2 CORP"))?.Split(" ")[2] ?? string.Empty;
        var address = lines.Find(l => l.StartsWith("3 ADDR"))?.Split(" ");
        Gedc.Source.Address = address.Length > 2 ? address[2] : string.Empty;
        Gedc.Source.City = lines.Find(l => l.StartsWith("4 CITY"))?.Split(" ")[2] ?? string.Empty;
        Gedc.Source.Website = lines.Find(l => l.StartsWith("3 WWW"))?.Split(" ")[2] ?? string.Empty;
        
        var date = lines.Find(l => l.StartsWith("1 DATE"))?.Replace("1 DATE ", "") ?? string.Empty;
        var time = lines.Find(l => l.StartsWith("2 TIME"))?.Replace("2 TIME ", "") ?? string.Empty;
        Gedc.Date = DateTime.ParseExact($"{date} {time}", "d MMM yyyy H:mm:ss", CultureInfo.InvariantCulture);
        
    }

    public List<string> SerializeTo()
    {
        throw new NotImplementedException();
    }
}

public class Gedc
{
    public string Vers { get; set; }
    public Form Form { get; set; } = new();
    public string Encoding  { get; set; }
    public Source Source { get; set; } = new();

    public DateTime Date { get; set; }

    public string FileName { get; set; }

    public string Language { get; set; }
}

public class Form
{
    public string Value { get; set; } = "LINEAGE-LINKED"; // default value
    public string Vers { get; set; }
}

public class Source
{
    public string Value { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string Organization { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Website { get; set; }
}

