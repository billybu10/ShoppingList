using CommunityToolkit.Mvvm.Input;
using System.Linq;
using ShoppingList.ViewModels;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace ShoppingList.Views;

public partial class AllProductsPage : ContentPage, IQueryAttributable
{
    public CategoriesViewModel CategoriesViewModel { get; set; }
    public ObservableCollection<CategoryProductsViewModel> CategoryProductsViewModels { get; set; }
    public ObservableCollection<string> EmptyCollection { get; }
    public ICommand NewProductCommand { get; }
    public ICommand BrowseShopsCommand { get; }
    public ICommand BrowseCategoriesCommand { get; }
    public ICommand BrowseUnitsCommand { get; }

    public AllProductsPage()
    {
        BindingContext = this;
        CategoriesViewModel = new CategoriesViewModel();
        CategoryProductsViewModels = new ObservableCollection<CategoryProductsViewModel>();
        foreach (var category in CategoriesViewModel.AllCategories)
        {
            CategoryProductsViewModels.Add(new CategoryProductsViewModel(category));
        }
        
        NewProductCommand = new AsyncRelayCommand(NewProductAsync);
        BrowseShopsCommand = new AsyncRelayCommand(BrowseShopsAsync);
        BrowseCategoriesCommand = new AsyncRelayCommand(BrowseCategoriesAsync);
        BrowseUnitsCommand = new AsyncRelayCommand(BrowseUnitsAsync);
        InitializeComponent();
        categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
        categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "CategoryProductsViewModels");

    }

    private async Task NewProductAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ProductPage));
    }

    private async Task BrowseUnitsAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AllUnitsPage));
    }

    private async Task BrowseShopsAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AllShopsPage));
    }

    private async Task BrowseCategoriesAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AllCategoriesPage));
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            CategoriesViewModel.Reload();
            CategoryProductsViewModels.Clear();
            foreach (var category in CategoriesViewModel.AllCategories)
            {
                CategoryProductsViewModels.Add(new CategoryProductsViewModel(category));
            }
            InvalidateMeasure();
            categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
            categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "CategoryProductsViewModels");

        }
        else if (query.ContainsKey("saved"))
        {
            CategoriesViewModel.Reload();
            CategoryProductsViewModels.Clear();
            foreach (var category in CategoriesViewModel.AllCategories)
            {
                CategoryProductsViewModels.Add(new CategoryProductsViewModel(category));
            }
            InvalidateMeasure();
            categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
            categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "CategoryProductsViewModels");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CategoriesViewModel.Reload();
        CategoryProductsViewModels.Clear();
        foreach (var category in CategoriesViewModel.AllCategories)
        {
            CategoryProductsViewModels.Add(new CategoryProductsViewModel(category));
        }
        InvalidateMeasure();
        categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "EmptyCollection");
        categoriesCollection.SetBinding(CollectionView.ItemsSourceProperty, "CategoryProductsViewModels");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        categoriesCollection.SelectedItem = null;
    }
}