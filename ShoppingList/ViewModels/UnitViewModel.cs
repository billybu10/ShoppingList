using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class UnitViewModel : ObservableObject, IQueryAttributable
{
    private Models.Unit _unit;

    public string Text
    {
        get => _unit.Name;
        set
        {
            if (_unit.Name != value)
            {
                _unit.Name = value;
                CheckData();
                OnPropertyChanged();
            }
        }
    }

    public string Shortened
    {
        get => _unit.Abbreviation;
        set
        {
            if (_unit.Abbreviation != value)
            {
                _unit.Abbreviation = value;
                CheckData();
                OnPropertyChanged();
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


    public UnitViewModel()
    {
        _unit = new Models.Unit();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public UnitViewModel(Models.Unit unit)
    {
        _unit = unit;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private void CheckData()
    {
        IncorrectDataString = "";
        bool IsWrong = false;
        if (String.IsNullOrWhiteSpace(_unit.Name))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Name \n";
        }
        if (String.IsNullOrWhiteSpace(_unit.Abbreviation))
        {
            IsWrong = true;
            IncorrectDataString += "Incorrect Abbreviation \n";
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
        _unit.Save();
        await Shell.Current.GoToAsync($"..?saved={_unit.Abbreviation}");

    }

    private async Task Delete()
    {
        _unit.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_unit.Abbreviation}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _unit = Models.Unit.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _unit = Models.Unit.Load(_unit.Abbreviation);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Shortened));
    }
}