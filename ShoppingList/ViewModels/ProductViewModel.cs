using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace ShoppingList.ViewModels;

public class ProductViewModel : ObservableObject, IQueryAttributable
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
                CheckData();
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
                CheckData();
            }
        }
    }

    public bool Active
    {
        get => _product.Checked;
        set
        {
            if (_product.Checked != value)
            {
                _product.Checked = value;
                OnPropertyChanged();
            }
        }
    }

    public bool Optional
    {
        get => _product.Optional;
        set
        {
            if (_product.Optional != value)
            {
                _product.Optional = value;
                OnPropertyChanged();
            }
        }
    }

    public string Category
    {
        get => _product.Category;
        set
        {
            if (_product.Category != value)
            {
                _product.Category = value;
                OnPropertyChanged();
                CheckData();
            }
        }
    }

    public string Shop
    {
        get => _product.Shop;
        set
        {
            if (_product.Shop != value)
            {
                _product.Shop = value;
                OnPropertyChanged();
                CheckData();
            }
        }
    }
    
    public string Unit
    {
        get => _product.Unit;
        set
        {
            if (_product.Unit != value)
            {
                _product.Unit = value;
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

    public string Identifier => _product.ID;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    public ICommand ResetCommand { get; private set; }
    public ICommand IncrementCommand { get; private set; }
    public ICommand DecrementCommand { get; private set; }
    public ICommand UpdateActiveCommand { get; private set; }


    public ProductViewModel()
    {
        _product = new Models.Product();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        IncrementCommand = new AsyncRelayCommand(Increment);
        DecrementCommand = new AsyncRelayCommand(Decrement);
        UpdateActiveCommand = new AsyncRelayCommand(UpdateActive);
    }

    public ProductViewModel(Models.Product product)
    {
        _product = product;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
        IncrementCommand = new AsyncRelayCommand(Increment);
        DecrementCommand = new AsyncRelayCommand(Decrement);
        UpdateActiveCommand = new AsyncRelayCommand(UpdateActive);
    }

    private void CheckData()
    {
        IncorrectDataString = "";
        bool IsWrong = false;
        if (String.IsNullOrWhiteSpace(_product.Name))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Name \n";
        }
        if (String.IsNullOrWhiteSpace(_product.Category))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Category \n";
        }
        if (String.IsNullOrWhiteSpace(_product.Shop))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Shop \n";
        }
        if (String.IsNullOrWhiteSpace(_product.Unit))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Unit \n";
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
        _product.Save();
        await Shell.Current.GoToAsync($"..?saved={_product.ID}");
    }

    private async Task Delete()
    {
        _product.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_product.ID}");
    }

    private async Task UpdateActive()
    {
        _product.Checked  = !_product.Checked;
        _product.Save();
        await Shell.Current.GoToAsync($"..?saved={_product.ID}");
    }

    private async Task Increment()
    {
        _product.Amount++;
        _product.Save();
        await Shell.Current.GoToAsync($"..?saved={_product.ID}");
    }

    private async Task Decrement()
    {
        if (_product.Amount > 0) 
        {
            _product.Amount--;
            _product.Save(); 
        }
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
        OnPropertyChanged(nameof(Active));
        OnPropertyChanged(nameof(Optional));
        OnPropertyChanged(nameof(Category));
        OnPropertyChanged(nameof(Shop));
        OnPropertyChanged(nameof(Unit));
    }

}