using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace GedcomWhisperer.Models;

public class Header
{
    private string _tag = GedcomTags.HeaderTag;
    private string _level = "0";
    public string Submitter;
    public HeaderSource HeaderSource;
    public DateTime Date;
    public GedcomVersion GedcomVersion;
    public string CharSet;
    

    private List<string> _innerTags = new List<string>();

    public Header()
    {
        
    }
    
    
    public void HandleLoadStringEvent(object sender, LoadEventArgs e)
    {
        var dataLines = e.RawDataString.Split("\r\n").ToList();

        LoadInnerTags(dataLines);
    }

    private void LoadInnerTags(List<string> dataLines)
    {
        var headerObject = GedcomTags.GetSection(_level, _tag, dataLines);

        _innerTags = headerObject.InnerTags;

        Submitter = GedcomTags.GetSection("1", GedcomTags.SubmitTag, dataLines).Value;

        ExtractSourceData(dataLines);
        ExtractDate(dataLines);
        ExtractGedcomVersion(dataLines);
        CharSet = GedcomTags.GetSection("1", GedcomTags.CharsetTag, dataLines).Value;
    }

    private void ExtractGedcomVersion(List<string> dataLines)
    {
        var gedcomVersionSection = GedcomTags.GetSection("1", GedcomTags.GedcomTag, dataLines);
        GedcomVersion = new GedcomVersion
        {
            Version = GedcomTags.GetSection("2", "VERS", gedcomVersionSection.InnerTags).Value,
            Format = GedcomTags.GetSection("2", "FORM", gedcomVersionSection.InnerTags).Value
        };
    }

    private void ExtractDate(List<string> dataLines)
    {
        string format = "d MMM yyyy hh:mm:ss";
        var dateTimeSection = GedcomTags.GetSection("1", GedcomTags.DateTag, dataLines);
        var date = dateTimeSection.Value;
        var time = GedcomTags.GetSection("2", "TIME", dateTimeSection.InnerTags).Value;
        var dateTimeValue = $"{date} {time}";
        Date = DateTime.ParseExact(dateTimeValue, format,
            CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    private void ExtractSourceData(List<string> dataLines)
    {
        var sourceSection = GedcomTags.GetSection("1", GedcomTags.SourceTag, dataLines);
        HeaderSource = new HeaderSource();
        HeaderSource.Value = sourceSection.Value;
        HeaderSource.Name = GedcomTags.GetSection("2", "NAME", sourceSection.InnerTags).Value;
        HeaderSource.Version = GedcomTags.GetSection("2", "VERS", sourceSection.InnerTags).Value;
        HeaderSource.CustomTags = GedcomTags.GetCustomSections("2", sourceSection.InnerTags);

        var corpSection = GedcomTags.GetSection("2", "CORP", sourceSection.InnerTags);
        var Corporate = new Corporate();
        Corporate.Value = corpSection.Value;
        Corporate.Phone = GedcomTags.GetSection("3", "PHON", corpSection.InnerTags).Value;
        Corporate.Website = GedcomTags.GetSection("3", "WWW", corpSection.InnerTags).Value;

        Address address = new Address();
        var addressSection = GedcomTags.GetSection("3", "ADDR", corpSection.InnerTags);
        address.Value = addressSection.Value;
        address.ContinuedTags = addressSection.InnerTags;
        Corporate.Address = address;

        HeaderSource.Corporate = Corporate;
    }
}

public struct HeaderSource
{
    public string Value { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public List<string> CustomTags { get; set; }

    public Corporate Corporate { get; set; }
    public override bool Equals(object obj)
    {
        if (!(obj is HeaderSource))
        {
            return false;
        }

        HeaderSource other = (HeaderSource) obj;
        return Value == other.Value && Name == other.Name && Version == other.Version &&
               CustomTags.SequenceEqual(other.CustomTags) && Corporate == other.Corporate;
    }
}

public struct Corporate
{
    public string Value { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public Address Address { get; set; }
    public string CharacterSet { get; set; }

    public override bool Equals(object obj)
    {
        if (!(obj is Corporate))
        {
            return false;
        }

        var other = (Corporate) obj;

        return Value == other.Value && Phone == other.Phone && Website == other.Website && Address == other.Address;
    }

    public bool Equals(Corporate other)
    {
        return Value == other.Value && Phone == other.Phone && Website == other.Website && Address.Equals(other.Address);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Phone, Website, Address);
    }

    public static bool operator ==(Corporate left, Corporate right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Corporate left, Corporate right)
    {
        return !Equals(left, right);;
    }
}

public struct Address
{
    public string Value { get; set; }
    public List<string> ContinuedTags { get; set; }

    public override bool Equals(object obj)
    {
        if (!(obj is Address))
        {
            return false;
        }

        var other = (Address) obj;

        return Value == other.Value && ContinuedTags.SequenceEqual(other.ContinuedTags);
    }

    public bool Equals(Address other)
    {
        return Value == other.Value && ContinuedTags.Equals(other.ContinuedTags);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, ContinuedTags);
    }

    public static bool operator ==(Address left, Address right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Address left, Address right)
    {
        return !Equals(left, right);
    }
}

public struct GedcomVersion
{
    public string Version { get; set; }
    public string Format { get; set; }

    public override bool Equals(object obj)
    {
        if (!(obj is GedcomVersion))
        {
            return false;
        }

        var other = (GedcomVersion) obj;
        return Version == other.Version && Format == other.Format;
    }

    public static bool operator ==(GedcomVersion left, GedcomVersion right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(GedcomVersion left, GedcomVersion right)
    {
        return !Equals(left, right);
    }
}
