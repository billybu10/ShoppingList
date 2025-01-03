using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class CategoryViewModel : ObservableObject, IQueryAttributable
{
    private Models.Category _category;

    public string Text
    {
        get => _category.Name;
        set
        {
            if (_category.Name != value)
            {
                _category.Name = value;
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


    public CategoryViewModel()
    {
        _category = new Models.Category();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public CategoryViewModel(Models.Category category)
    {
        _category = category;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private void CheckData()
    {
        IncorrectDataString = "";
        bool IsWrong = false;
        if (String.IsNullOrWhiteSpace(_category.Name))
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
        _category.Save();
        await Shell.Current.GoToAsync($"..?saved={_category.Name}");

    }

    private async Task Delete()
    {
        _category.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_category.Name}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _category = Models.Category.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _category = Models.Category.Load(_category.Name);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
    }
}