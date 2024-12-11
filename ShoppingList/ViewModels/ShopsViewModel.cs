using CommunityToolkit.Mvvm.Input;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using ShoppingList.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

internal class ShopsViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.ShopViewModel> AllShops { get; }

    private ICommand _newCommand { set; get; }
    public ICommand NewCommand => _newCommand ?? (_newCommand = new Command<string>(AddNewShop));

    public ShopsViewModel()
    {
        AllShops = new ObservableCollection<ViewModels.ShopViewModel>(Models.Shop.LoadAll().Select(n => new ShopViewModel(n)));
    }

    private void AddNewShop(string name)
    {
        Shop newShop = new Shop();
        newShop.Name = name;
        newShop.Save();
        ShopViewModel shopVM = new ShopViewModel(newShop);
        AllShops.Add(shopVM);
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