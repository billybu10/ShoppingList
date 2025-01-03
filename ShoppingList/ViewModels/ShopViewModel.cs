using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class ShopViewModel : ObservableObject, IQueryAttributable
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
                CheckData();
            }
        }
    }

    private string InternalIncorrectDataString = "Input data";
    public string IncorrectDataString
    {
        get => InternalIncorrectDataString;
        set
        {
            if (InternalIncorrectDataString != value)
            {
                InternalIncorrectDataString = value;
                OnPropertyChanged();
            }
        }
    }
    private bool InternalIsAllOk = false;
    public bool IsAllOk
    {
        get => InternalIsAllOk;
        set
        {
            if (InternalIsAllOk != value)
            {
                InternalIsAllOk = value;
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

    private void CheckData()
    {
        IncorrectDataString = "";
        bool IsWrong = false;
        if (String.IsNullOrWhiteSpace(_shop.Name))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Name \n";
        }

        if (!IsWrong)
        {
            IsAllOk = true;
        }
        else
        {
            IsAllOk = false;
        }
    }

    private async Task Save()
    {
        _shop.Save();
        await Shell.Current.GoToAsync($"..?saved={_shop.Name}");

    }

    private async Task Delete()
    {
        _shop.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_shop.Name}");
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