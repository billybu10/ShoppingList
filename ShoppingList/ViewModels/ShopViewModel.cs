using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

internal class ShopViewModel : ObservableObject, IQueryAttributable
{
    private Models.Shop _shop;

    public string Text
    {
        get => _shop.Name;
        set
        {
            if (_shop.Name != value)
            {
                _shop.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }


    public ShopViewModel()
    {
        _shop = new Models.Shop();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public ShopViewModel(Models.Shop shop)
    {
        _shop = shop;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _shop.Save();
        //await Shell.Current.GoToAsync(nameof(Views.AllShopsPage));
    }

    private async Task Delete()
    {
        _shop.Delete();
        //await Shell.Current.GoToAsync(nameof(Views.AllShopsPage));
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _shop = Models.Shop.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _shop = Models.Shop.Load(_shop.Name);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
    }
}