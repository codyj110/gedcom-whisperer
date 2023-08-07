using System.Globalization;

namespace GedcomWhisperer.Models;

public class Birthday
{
    public DateOnly Date { get; set; }
    public string Place { get; set; }
    public List<string> Sources { get; set; }

    public Birthday(TagObject parentObject)
    {
        var birthdayObject = GedcomTags.GetSection("1", GedcomTags.IndividualTagBirt, parentObject.InnerTags);
        var nameObject = GedcomTags.GetSection("1", GedcomTags.IndividualTagName, parentObject.InnerTags);
        
        if (birthdayObject.InnerTags.Count != 0)
        {
            try
            {
                string format = "d MMM yyyy";
                DateOnly.TryParseExact(
                    GedcomTags.GetSection("2", GedcomTags.DateTag, birthdayObject.InnerTags).Value,
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date);

                Date = date;
                Place = GedcomTags.GetSection("2", GedcomTags.IndividualTagPlace, birthdayObject.InnerTags).Value;
                Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags)
                    .Select(x => x.Value).ToList();

            }
            catch (FormatException exception)
            {
                string format = "MMM d yyyy";

                DateTime.TryParseExact(GedcomTags.GetSection("2", GedcomTags.DateTag, birthdayObject.InnerTags).Value, format, null,
                    DateTimeStyles.None,
                    out var dateTime);
                DateOnly date = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);

                Date = date;
                Place = GedcomTags.GetSection("2", GedcomTags.DateTag, birthdayObject.InnerTags).Value;
                Sources = GedcomTags.GetSections("2", GedcomTags.SourceTag, nameObject.InnerTags)
                    .Select(x => x.Value).ToList();
            }
        }
    }
    
    public override bool Equals(object obj)
    {
        if (obj is Birthday other)
        {
            return Date.Equals(other.Date) &&
                   Place == other.Place &&
                   Sources.SequenceEqual(other.Sources);
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Date.GetHashCode();
            hashCode = (hashCode * 397) ^ (Place != null ? Place.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Sources != null ? Sources.GetHashCode() : 0);
            return hashCode;
        }
    }

    public static bool operator ==(Birthday birthday1, Birthday birthday2)
    {
        return birthday1.Equals(birthday2);
    }

    public static bool operator !=(Birthday birthday1, Birthday birthday2)
    {
        return !birthday1.Equals(birthday2);
    }
}