using CommunityToolkit.Mvvm.DependencyInjection;
using System.Linq;
using System.Net.Security;
using System.Xml.Linq;

namespace ShoppingList.Models;

public class Product

{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Shop { get; set; }
    public string Unit { get; set; }
    public int Amount { get; set; }
    public bool Checked { get; set; }
    public bool Optional { get; set; }

    public Product()
    {
        ID = Guid.NewGuid().ToString("N");
        Amount = 0;
        Name = "";
        Shop = "";
        Unit = "";
        Category = "";
        Checked = false;
        Optional = false;
    }

    public void Save()
    {
        if (Name == "" || Name == String.Empty || String.IsNullOrWhiteSpace(Name))
        {
            //Popup
            Name = "Default Name";
        }
        string productsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "products.xml");
        if (File.Exists(productsFilePath)){
            XDocument productsXMLDocument = XDocument.Load(productsFilePath);
            IEnumerable<XElement> productNodes = productsXMLDocument.Element("Products").Descendants();
            if (productNodes.Any(p => p.Attribute("ID").Value == ID))
            {
                XElement productToEdit = productsXMLDocument
                  .Element("Products")
                  .Elements("Product")
                  .Where(e => e.Attribute("ID").Value == ID)
                  .Single();
                productToEdit.Attribute("Name").Value = Name;
                productToEdit.Attribute("Amount").Value = Amount.ToString();
                productToEdit.Attribute("Checked").Value = Checked.ToString();
                productToEdit.Attribute("Optional").Value = Optional.ToString();
                productToEdit.Attribute("Category").Value = Category;
                productToEdit.Attribute("Shop").Value = Shop;
                productToEdit.Attribute("Unit").Value = Unit;

                productsXMLDocument.Save(productsFilePath);
            }
            else
            {
                XElement newProduct = new XElement("Product",
                     new XAttribute("ID", ID),
                     new XAttribute("Name", Name),
                     new XAttribute("Amount", Amount.ToString()),
                     new XAttribute("Checked", Checked.ToString()),
                     new XAttribute("Optional", Optional.ToString()),
                     new XAttribute("Category", Category),
                     new XAttribute("Shop", Shop),
                     new XAttribute("Unit", Unit)
                );

                productsXMLDocument.Element("Products").Add(newProduct);

                productsXMLDocument.Save(productsFilePath);
            }
        }
        else
        {
            XDocument productsXMLDocument = new XDocument(
                new XElement("Products",
                    new XElement("Product",
                        new XAttribute("ID", ID),
                        new XAttribute("Name", Name),
                        new XAttribute("Amount", Amount.ToString()),
                        new XAttribute("Checked", Checked.ToString()),
                        new XAttribute("Optional", Optional.ToString()),
                        new XAttribute("Category", Category),
                        new XAttribute("Shop", Shop),
                        new XAttribute("Unit", Unit)
                    )
                )
            );
            productsXMLDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");

            productsXMLDocument.Save(productsFilePath);
        }
    }


    public void Delete() 
    {
        string productsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "products.xml");
        XDocument productsXMLDocument = XDocument.Load(productsFilePath);
        foreach (XElement element in productsXMLDocument.Element("Products").Elements())
        {
            if (element.Attribute("ID").Value == ID)
            {
                element.Remove();
            }
        }
        productsXMLDocument.Save(productsFilePath);
    }

    public static Product Load(string ID)
    {
        string productsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "products.xml");
        XDocument productsXMLDocument = XDocument.Load(productsFilePath);
        XElement productToRetrieve = productsXMLDocument
                  .Element("Products")
                  .Elements("Product")
                  .Where(e => e.Attribute("ID").Value == ID)
                  .Single();

        return
            new()
            {
                ID = productToRetrieve.Attribute("ID").Value,
                Name = productToRetrieve.Attribute("Name").Value,
                Amount = Int32.Parse(productToRetrieve.Attribute("Amount").Value),
                Checked = Boolean.Parse(productToRetrieve.Attribute("Checked").Value),
                Optional = Boolean.Parse(productToRetrieve.Attribute("Optional").Value),
                Category = productToRetrieve.Attribute("Category").Value,
                Shop = productToRetrieve.Attribute("Shop").Value,
                Unit = productToRetrieve.Attribute("Unit").Value
            };
    }

    public static IEnumerable<Product> LoadAll()
    {

        string productsFilePath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "products.xml");
        
        if (!File.Exists(productsFilePath))
        {
            XDocument productsXMLNewDocument = new XDocument(new XElement("Products"));
            productsXMLNewDocument.Declaration = new XDeclaration("1.0", "utf-8", "true");
            productsXMLNewDocument.Save(productsFilePath);
        }
        XDocument productsXMLDocument = XDocument.Load(productsFilePath);
        if (productsXMLDocument.Element("Products").Descendants() != null) 
        { 
            return productsXMLDocument.Element("Products").Descendants().AsEnumerable().Select<XElement, Product>(element => new()
                    {
                        ID = element.Attribute("ID").Value,
                        Name = element.Attribute("Name").Value,
                        Amount = Int32.Parse(element.Attribute("Amount").Value),
                        Checked = Boolean.Parse(element.Attribute("Checked").Value),
                        Optional = Boolean.Parse(element.Attribute("Optional").Value),
                        Category = element.Attribute("Category").Value,
                        Shop = element.Attribute("Shop").Value,
                        Unit = element.Attribute("Unit").Value
            }
            );
        }
        else
        {
            return Enumerable.Empty<Product>();
        }
    }
}