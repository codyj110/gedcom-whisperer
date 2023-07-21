using System.Diagnostics;
using System.Globalization;
using GedcomWhisperer;
using GedcomWhisperer.Models;

namespace GedcomWhispererTests;

public class LoadGedcomHeaderTest
{
    [Fact]
    public void LoadsString()
    {
        using (StreamReader reader = new StreamReader("Sample.ged"))
        {
            // Setup
            string fileContent = reader.ReadToEnd();
            var gedcom = new Gedcom();

            var expectedSubmitter = "@SUBM1@";

            var expectedSource = new HeaderSource
            {
                Value = "novariseindustries.com Family Trees",
                Name = "novariseindustries.com Member Trees",
                Version = "2022.08",
                CustomTags = new List<string>() {"2 _TREE Everly Family Tree", "3 RIN 163715145", "3 _ENV prd"}
            };

            var expectedCorporate = new Corporate
            {
                Value = "novariseindustries.com",
                Phone = "555-123-4567",
                Website = "www.novariseindustries.com",
                Address = new Address
                {
                    Value = "123 Elm Street",
                    ContinuedTags = new List<string>() {"4 CONT Anytown, USA  12345", "4 CONT USA"}
                }
            };

            expectedSource.Corporate = expectedCorporate;

            var expectedChartSet = "UTF-8";
            var expectedDate = DateTime.Parse("4 Jul 2023 04:33:28");
            var expectedGedcom = new GedcomVersion
            {
                Version = "5.5.1",
                Format = "LINEAGE-LINKED"
            };

            // Act
            gedcom.LoadString(fileContent);
            var header = gedcom.Header;
            // Assert
            Assert.Equal(expectedSubmitter, header.Submitter);
            Assert.Equal(expectedSource, header.HeaderSource);
            Assert.Equal(expectedChartSet, header.CharSet);
            Assert.Equal(expectedDate.ToString(CultureInfo.InvariantCulture), header.Date.ToString(CultureInfo.InvariantCulture));
            Assert.Equal(expectedGedcom, header.GedcomVersion);
        }
        
    }
}