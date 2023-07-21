namespace GedcomWhisperer.Models;

public struct Name
{
    public string Value { get; set; }
    public string GivenName { get; set; }
    public string Surname { get; set; }
    public List<string> Source { get; set; }
    
    public override bool Equals(object obj)
    {
        if (obj is Name other)
        {
            return Value == other.Value &&
                   GivenName == other.GivenName &&
                   Surname == other.Surname &&
                   Source.SequenceEqual(other.Source);
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Value != null ? Value.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (GivenName != null ? GivenName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Surname != null ? Surname.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Source != null ? Source.GetHashCode() : 0);
            return hashCode;
        }
    }

    public static bool operator ==(Name name1, Name name2)
    {
        return name1.Equals(name2);
    }

    public static bool operator !=(Name name1, Name name2)
    {
        return !name1.Equals(name2);
    }
}