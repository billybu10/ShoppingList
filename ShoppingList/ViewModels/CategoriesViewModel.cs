using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class CategoriesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.CategoryViewModel> AllCategories { get; }
    public ICommand NewCommand { get; }

    public CategoriesViewModel()
    {
        AllCategories = new ObservableCollection<ViewModels.CategoryViewModel>(Models.Category.LoadAll().Select(n => new CategoryViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewCategoryAsync);
    }

    public void Reload()
    {
        AllCategories.Clear();
        foreach(var categoryViewModel in Models.Category.LoadAll().Select(n => new CategoryViewModel(n))) AllCategories.Add(categoryViewModel);
    }

    private async Task NewCategoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CategoryPage));
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string categoryName = query["deleted"].ToString();
            CategoryViewModel matchedCategory = AllCategories.Where((n) => n.Text == categoryName).FirstOrDefault();

            if (matchedCategory != null)
                AllCategories.Remove(matchedCategory);
        }
        else if (query.ContainsKey("saved"))
        {
            string categoryName = query["saved"].ToString();
            CategoryViewModel matchedCategory = AllCategories.Where((n) => n.Text == categoryName).FirstOrDefault();


            if (matchedCategory != null)
            {
                matchedCategory.Reload();
            }

            else
                AllCategories.Insert(0, new CategoryViewModel(Models.Category.Load(categoryName)));
        }
    }
}