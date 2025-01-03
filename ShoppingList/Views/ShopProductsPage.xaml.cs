using CommunityToolkit.Mvvm.Input;
using ShoppingList.ViewModels;
using System.Collections.ObjectModel;

namespace ShoppingList.Views;

public partial class ShopProductsPage : ContentPage, IQueryAttributable
{
    public ShopsViewModel ShopsViewModel { get; set; }
    public ObservableCollection<ShopProductsViewModel> ShopProductsViewModels { get; set; }
    public ObservableCollection<string> EmptyCollection { get; }

    public ShopProductsPage()
	{
        BindingContext = this;
        ShopsViewModel = new ShopsViewModel();
        ShopProductsViewModels = new ObservableCollection<ShopProductsViewModel>();
        foreach (var shop in ShopsViewModel.AllShops)
        {
            ShopProductsViewModels.Add(new ShopProductsViewModel(shop));
        }
        InitializeComponent();
        //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
        //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "Products");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            ShopsViewModel.Reload();
            ShopProductsViewModels.Clear();
            foreach (var shop in ShopsViewModel.AllShops)
            {
                ShopProductsViewModels.Add(new ShopProductsViewModel(shop));
            }
            InvalidateMeasure();
            //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
            //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "Products");

        }
        else if (query.ContainsKey("saved"))
        {
            ShopsViewModel.Reload();
            ShopProductsViewModels.Clear();
            foreach (var shop in ShopsViewModel.AllShops)
            {
                ShopProductsViewModels.Add(new ShopProductsViewModel(shop));
            }
            InvalidateMeasure();
            //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
            //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "Products");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ShopsViewModel.Reload();
        ShopProductsViewModels.Clear();
        foreach (var shop in ShopsViewModel.AllShops)
        {
            ShopProductsViewModels.Add(new ShopProductsViewModel(shop));
        }
        InvalidateMeasure();
        //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
        //shopsCollection.SetBinding(CollectionView.ItemsSourceProperty, "Products");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        //shopsCollection.SelectedItem = null;
    }
}