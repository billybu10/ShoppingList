using CommunityToolkit.Mvvm.Input;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using ShoppingList.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class UnitsViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.UnitViewModel> AllUnits { get; }
    public ICommand NewCommand { get; }

    public UnitsViewModel()
    {
        AllUnits = new ObservableCollection<ViewModels.UnitViewModel>(Models.Unit.LoadAll().Select(n => new UnitViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewUnitAsync);
    }

    private async Task NewUnitAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.UnitPage));
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string unitAbbreviation = query["deleted"].ToString();
            UnitViewModel matchedUnit = AllUnits.Where((n) => n.Shortened == unitAbbreviation).FirstOrDefault();

            if (matchedUnit != null)
                AllUnits.Remove(matchedUnit);
        }
        else if (query.ContainsKey("saved"))
        {
            string unitAbbreviation = query["saved"].ToString();
            UnitViewModel matchedUnit = AllUnits.Where((n) => n.Shortened == unitAbbreviation).FirstOrDefault();


            if (matchedUnit != null)
            {
                matchedUnit.Reload();
            }

            else
                AllUnits.Insert(0, new UnitViewModel(Models.Unit.Load(unitAbbreviation)));
        }
    }
}