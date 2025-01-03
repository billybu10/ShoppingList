using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Xml.Linq;

namespace ShoppingList.Models;

public class Category

{
    public string Name { get; set; }

    public Category()
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
        string categoriesFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "categories.xml");
        if (File.Exists(categoriesFilePath))
        {
            XDocument categoriesXMLDocument = XDocument.Load(categoriesFilePath);
            IEnumerable<XElement> categoryNodes = categoriesXMLDocument.Element("Categories").Descendants();
            if (categoryNodes.Any(p => p.Attribute("Name").Value.ToLower() == Name.ToLower()))
            {
                //Popup
                return;
            }
            else
            {
                XElement newCategory = new XElement("Category",
                     new XAttribute("Name", Name)
                );

                categoriesXMLDocument.Element("Categories").Add(newCategory);

                categoriesXMLDocument.Save(categoriesFilePath);
            }
        }
        else
        {
            XDocument categoriesXMLDocument = new XDocument(
                new XElement("Categories",
                    new XElement("Category",
                        new XAttribute("Name", Name)
                    )
                )
            );
            categoriesXMLDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");

            categoriesXMLDocument.Save(categoriesFilePath);
        }
    }


    public void Delete()
    {
        string categoriesFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "categories.xml");
        XDocument categoriesXMLDocument = XDocument.Load(categoriesFilePath);
        foreach (XElement element in categoriesXMLDocument.Element("Categories").Elements())
        {
            if (element.Attribute("Name").Value == Uri.UnescapeDataString(Name))
            {
                element.Remove();
            }
        }
        categoriesXMLDocument.Save(categoriesFilePath);
    }

    public static Category Load(string Name)
    {
        string categoriesFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "categories.xml");
        XDocument categoriesXMLDocument = XDocument.Load(categoriesFilePath);
        XElement categoryToRetrieve = categoriesXMLDocument
                  .Element("Categories")
                  .Elements("Category")
                  .Where(e => e.Attribute("Name").Value == Uri.UnescapeDataString(Name))
                  .Single();
        return
            new()
            {
                Name = categoryToRetrieve.Attribute("Name").Value
            };
    }

    public static IEnumerable<Category> LoadAll()
    {

        string categoriesFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "categories.xml");

        if (!File.Exists(categoriesFilePath))
        {
            XDocument categoriesXMLNewDocument = new XDocument(new XElement("Categories"));
            categoriesXMLNewDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            categoriesXMLNewDocument.Save(categoriesFilePath);
        }
        XDocument categoriesXMLDocument = XDocument.Load(categoriesFilePath);
        if (categoriesXMLDocument.Element("Categories").Descendants() != null)
        {
            return categoriesXMLDocument.Element("Categories").Descendants().AsEnumerable().Select<XElement, Category>(element => new()
            {
                Name = element.Attribute("Name").Value
            }
            );
        }
        else
        {
            return Enumerable.Empty<Category>();
        }
    }
}