using ShoppingList.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ShoppingList.Views;

public partial class ProductPage : ContentPage
{
    public ShoppingList.ViewModels.ProductViewModel ProductViewModel { get; set; }
    public ShoppingList.ViewModels.CategoriesViewModel CategoriesViewModel { get; set; }
    public ShoppingList.ViewModels.ShopsViewModel ShopsViewModel { get; set; }
    public ShoppingList.ViewModels.UnitsViewModel UnitsViewModel { get; set; }
    public ObservableCollection<string> CategoryNames { get; set; } = [];
    public ObservableCollection<string> ShopNames { get; set; } = [];
    public ObservableCollection<string> Units { get; set; } = [];

    public void FillCategoryNames()
    {
        CategoryNames.Clear();
        CategoriesViewModel = new CategoriesViewModel();
        
        foreach (var category in CategoriesViewModel.AllCategories.OrderBy(c => c.Text))
        {
            CategoryNames.Add(category.Text);
        }
    }

    public void FillShopNames()
    {
        ShopNames.Clear();
        ShopsViewModel = new ShopsViewModel();

        foreach (var shop in ShopsViewModel.AllShops.OrderBy(s => s.Text))
        {
            ShopNames.Add(shop.Text);
        }
    }

    public void FillUnits()
    {
        Units.Clear();
        UnitsViewModel = new UnitsViewModel();

        foreach (var unit in UnitsViewModel.AllUnits.OrderBy(u => u.Shortened))
        {
            Units.Add(unit.Shortened);
        }
    }

    public ProductPage()
    {
        
        FillCategoryNames();
        FillShopNames();
        FillUnits();

        ProductViewModel = new ProductViewModel();


        BindingContext = this;
        InitializeComponent();

    }
}