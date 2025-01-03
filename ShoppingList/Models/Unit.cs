using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Xml.Linq;

namespace ShoppingList.Models;

public class Unit

{
    public string Abbreviation { get; set; }
    public string Name { get; set; }

    public Unit()
    {
        Abbreviation = "";
        Name = "";
    }

    public void Save()
    {
        if (Abbreviation == "" || Abbreviation == String.Empty || String.IsNullOrWhiteSpace(Abbreviation))
        {
            Abbreviation = "DA.";
            //Popup
        }
        if (Name == "" || Name == String.Empty || String.IsNullOrWhiteSpace(Name))
        {
            Name = "Default Name";
            //Popup
        }
        string unitsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "units.xml");
        if (File.Exists(unitsFilePath))
        {
            XDocument unitsXMLDocument = XDocument.Load(unitsFilePath);
            IEnumerable<XElement> unitNodes = unitsXMLDocument.Element("Units").Descendants();
            if (unitNodes.Any(p => p.Attribute("Abbreviation").Value.ToLower() == Abbreviation.ToLower()))
            {
                //Popup
                return;
            }
            else
            {
                XElement newUnit = new XElement("Unit",
                     new XAttribute("Abbreviation", Abbreviation),
                     new XAttribute("Name", Name)
                );

                unitsXMLDocument.Element("Units").Add(newUnit);

                unitsXMLDocument.Save(unitsFilePath);
            }
        }
        else
        {
            XDocument unitsXMLDocument = new XDocument(
                new XElement("Units",
                    new XElement("Unit",
                        new XAttribute("Abbreviation", Abbreviation),
                        new XAttribute("Name", Name)
                    )
                )
            );
            unitsXMLDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");

            unitsXMLDocument.Save(unitsFilePath);
        }
    }


    public void Delete()
    {
        string unitsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "units.xml");
        XDocument unitsXMLDocument = XDocument.Load(unitsFilePath);
        foreach (XElement element in unitsXMLDocument.Element("Units").Elements())
        {
            if (element.Attribute("Abbreviation").Value == Uri.UnescapeDataString(Abbreviation))
            {
                element.Remove();
            }
        }
        unitsXMLDocument.Save(unitsFilePath);
    }

    public static Unit Load(string Abbreviation)
    {
        string unitsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "units.xml");
        XDocument unitsXMLDocument = XDocument.Load(unitsFilePath);
        XElement unitToRetrieve = unitsXMLDocument
                  .Element("Units")
                  .Elements("Unit")
                  .Where(e => e.Attribute("Abbreviation").Value == Uri.UnescapeDataString(Abbreviation))
                  .Single();
        return
            new()
            {
                Abbreviation = unitToRetrieve.Attribute("Abbreviation").Value,
                Name = unitToRetrieve.Attribute("Name").Value
            };
    }

    public static IEnumerable<Unit> LoadAll()
    {

        string unitsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "units.xml");

        if (!File.Exists(unitsFilePath))
        {
            XDocument unitsXMLNewDocument = new XDocument(new XElement("Units"));
            unitsXMLNewDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            unitsXMLNewDocument.Save(unitsFilePath);
        }
        XDocument unitsXMLDocument = XDocument.Load(unitsFilePath);
        if (unitsXMLDocument.Element("Units").Descendants() != null)
        {
            return unitsXMLDocument.Element("Units").Descendants().AsEnumerable().Select<XElement, Unit>(element => new()
            {
                Abbreviation = element.Attribute("Abbreviation").Value,
                Name = element.Attribute("Name").Value
            }
            );
        }
        else
        {
            return Enumerable.Empty<Unit>();
        }
    }
}