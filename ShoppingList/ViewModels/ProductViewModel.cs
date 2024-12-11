using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

internal class ProductViewModel : ObservableObject, IQueryAttributable
{
    private Models.Product _product;

    public string Text
    {
        get => _product.Name;
        set
        {
            if (_product.Name != value)
            {
                _product.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public int Value
    {
        get => _product.Amount;
        set
        {
            if (_product.Amount != value)
            {
                _product.Amount = value;
                OnPropertyChanged();
            }
        }
    }

    public string Identifier => _product.ID;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand ResetCommand { get; private set; }
    public ICommand IncrementCommand { get; private set; }
    public ICommand DecrementCommand { get; private set; }


    public ProductViewModel()
    {
        _product = new Models.Product();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        IncrementCommand = new AsyncRelayCommand(Increment);
        DecrementCommand = new AsyncRelayCommand(Decrement);
    }

    public ProductViewModel(Models.Product product)
    {
        _product = product;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        IncrementCommand = new AsyncRelayCommand(Increment);
        DecrementCommand = new AsyncRelayCommand(Decrement);
    }

    private async Task Save()
    {
        _product.Save();
        await Shell.Current.GoToAsync($"..?saved={_product.ID}");
    }

    private async Task Delete()
    {
        _product.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_product.ID}");
    }

    private async Task Increment()
    {
        _product.Amount++;
        _product.Save();
        await Shell.Current.GoToAsync($"..?saved={_product.ID}");
    }

    private async Task Decrement()
    {
        _product.Amount--;
        _product.Save();
        await Shell.Current.GoToAsync($"..?saved={_product.ID}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _product = Models.Product.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _product = Models.Product.Load(_product.ID);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Value));
    }
}