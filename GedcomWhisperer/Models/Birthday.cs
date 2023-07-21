namespace GedcomWhisperer.Models;

public class Birthday
{
    public DateOnly Date { get; set; }
    public string Place { get; set; }
    public List<string> Sources { get; set; }
    
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