namespace GedcomWhisperer.Models;

public class Individual
{
// 0 @I1@ INDI 
// 1 NAME Robert Eugene /Williams/
//     2 SURN Williams
// 2 GIVN Robert Eugene
// 1 SEX M
// 1 BIRT
// 2 DATE 2 Oct 1822
// 2 PLAC Weston, Madison, Connecticut, United States of America
// 2 SOUR @S1@
// 3 PAGE Sec. 2, p. 45
// 1 DEAT
// 2 DATE 14 Apr 1905
// 2 PLAC Stamford, Fairfield, Connecticut, United States of America
// 1 BURI
// 2 PLAC Spring Hill Cemetery, Stamford, Fairfield, Connecticut, United States of America
// 1 FAMS @F1@
// 1 FAMS @F2@
// 1 RESI
// 2 DATE from 1900 to 1905
    public string Id { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public List<string> Notes { get; private set; } = new List<string>();
    
    // ... Add other attributes like events, family links, etc.

    // Example method to parse from a GEDCOM string representation
    public Individual(List<string> lines)
    {
        foreach (var line in lines)
        {
            var tokens = line.Split(new char[] { ' ' }, 3);

            if (tokens.Length > 2)
            {
                switch (tokens[2])
                {
                    case "INDI":
                        Id = tokens[1].Trim('@');
                        break;
                }
            }

            if (tokens.Length < 2) continue;
            
            switch (tokens[1])
            {
                case "NAME":
                    Name = tokens[2];
                    break;
                case "SEX":
                    Gender = tokens[2];
                    break;
                case "BIRT":
                    // Birth event; following line might contain date
                    var nextLine = lines[lines.IndexOf(line) + 1];
                    if (nextLine.Contains("DATE"))
                    {
                        var dateTokens = nextLine.Split(new char[] { ' ' }, 3);
                        BirthDate = DateTime.Parse(dateTokens[2]);
                    }
                    break;
                case "DEAT":
                    // Death event; following line might contain date
                    var nextLineDeath = lines[lines.IndexOf(line) + 1];
                    if (nextLineDeath.Contains("DATE"))
                    {
                        var dateTokensDeath = nextLineDeath.Split(new char[] { ' ' }, 3);
                        DeathDate = DateTime.Parse(dateTokensDeath[2]);
                    }
                    break;
                case "NOTE":
                    Notes.Add(tokens[2]);
                    break;
                // ... Handle other GEDCOM tags as needed
            }
        }
    }
}
