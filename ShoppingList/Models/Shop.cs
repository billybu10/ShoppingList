using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Xml.Linq;

namespace ShoppingList.Models;

public class Shop

{
    public string Name { get; set; }

    public Shop()
    {
        Name = "";
    }

    public void Save()
    {
        if (Name == "" || Name == String.Empty || String.IsNullOrWhiteSpace(Name))
        {
            Name = "Default Name";
            //Popup
        }
        string shopsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "shops.xml");
        if (File.Exists(shopsFilePath)){
            XDocument shopsXMLDocument = XDocument.Load(shopsFilePath);
            IEnumerable<XElement> shopNodes = shopsXMLDocument.Element("Shops").Descendants();
            if (shopNodes.Any(p => p.Attribute("Name").Value.ToLower() == Name.ToLower()))
            {
                //Popup
                return;
            }
            else
            {
                XElement newShop = new XElement("Shop",
                     new XAttribute("Name", Name)
                );

                shopsXMLDocument.Element("Shops").Add(newShop);

                shopsXMLDocument.Save(shopsFilePath);
            }
        }
        else
        {
            XDocument shopsXMLDocument = new XDocument(
                new XElement("Shops",
                    new XElement("Shop",
                        new XAttribute("Name", Name)
                    )
                )
            );
            shopsXMLDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");

            shopsXMLDocument.Save(shopsFilePath);
        }
    }


    public void Delete() 
    {
        string shopsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "shops.xml");
        XDocument shopsXMLDocument = XDocument.Load(shopsFilePath);
        foreach (XElement element in shopsXMLDocument.Element("Shops").Elements())
        {
            if (element.Attribute("Name").Value == Uri.UnescapeDataString(Name))
            {
                element.Remove();
            }
        }
        shopsXMLDocument.Save(shopsFilePath);
    }

    public static Shop Load(string Name)
    {
        string shopsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "shops.xml");
        XDocument shopsXMLDocument = XDocument.Load(shopsFilePath);
        XElement shopToRetrieve = shopsXMLDocument
                  .Element("Shops")
                  .Elements("Shop")
                  .Where(e => e.Attribute("Name").Value == Uri.UnescapeDataString(Name))
                  .Single();
        return
            new()
            {
                Name = shopToRetrieve.Attribute("Name").Value
            };
    }

    public static IEnumerable<Shop> LoadAll()
    {

        string shopsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "shops.xml");
        
        if (!File.Exists(shopsFilePath))
        {
            XDocument shopsXMLNewDocument = new XDocument(new XElement("Shops"));
            shopsXMLNewDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            shopsXMLNewDocument.Save(shopsFilePath);
        }
        XDocument shopsXMLDocument = XDocument.Load(shopsFilePath);
        if (shopsXMLDocument.Element("Shops").Descendants() != null) 
        { 
            return shopsXMLDocument.Element("Shops").Descendants().AsEnumerable().Select<XElement, Shop>(element => new()
                    {
                        Name = element.Attribute("Name").Value
                    }
            );
        }
        else
        {
            return Enumerable.Empty<Shop>();
        }
    }
}