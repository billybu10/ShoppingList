using CommunityToolkit.Mvvm.Input;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using ShoppingList.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class ShopsViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.ShopViewModel> AllShops { get; }
    public ICommand NewCommand { get; }

    public ShopsViewModel()
    {
        AllShops = new ObservableCollection<ViewModels.ShopViewModel>(Models.Shop.LoadAll().Select(n => new ShopViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewShopAsync);
    }

    public void Reload()
    {
        AllShops.Clear();
        foreach (var shopViewModel in Models.Shop.LoadAll().Select(n => new ShopViewModel(n))) AllShops.Add(shopViewModel);
    }

    private async Task NewShopAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ShopPage));
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string shopName = query["deleted"].ToString();
            ShopViewModel matchedShop = AllShops.Where((n) => n.Text == shopName).FirstOrDefault();

            if (matchedShop != null)
                AllShops.Remove(matchedShop);
        }
        else if (query.ContainsKey("saved"))
        {
            string shopName = query["saved"].ToString();
            ShopViewModel matchedShop = AllShops.Where((n) => n.Text == shopName).FirstOrDefault();


            if (matchedShop != null)
            {
                matchedShop.Reload();
            }

            else
                AllShops.Insert(0, new ShopViewModel(Models.Shop.Load(shopName)));
        }
    }
}