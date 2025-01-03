using CommunityToolkit.Mvvm.Input;
using ShoppingList.Models;
using ShoppingList.ViewModels;
using ShoppingList.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ShoppingList.ViewModels;

public class ProductsViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.ProductViewModel> AllProducts { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectProductCommand { get; }

    public ProductsViewModel()
    {
        AllProducts = new ObservableCollection<ViewModels.ProductViewModel>(Models.Product.LoadAll().Select(n => new ProductViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewProductAsync);
        SelectProductCommand = new AsyncRelayCommand<ViewModels.ProductViewModel>(SelectProductAsync);
    }

    private async Task NewProductAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ProductPage));
    }

    private async Task SelectProductAsync(ViewModels.ProductViewModel product)
    {
        if (product != null)
            await Shell.Current.GoToAsync($"{nameof(Views.ProductPage)}?load={product.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string productId = query["deleted"].ToString();
            ProductViewModel matchedProduct = AllProducts.Where((n) => n.Identifier == productId).FirstOrDefault();

            if (matchedProduct != null)
                AllProducts.Remove(matchedProduct);
        }
        else if (query.ContainsKey("saved"))
        {
            string productId = query["saved"].ToString();
            ProductViewModel matchedProduct = AllProducts.Where((n) => n.Identifier == productId).FirstOrDefault();


            if (matchedProduct != null)
            {
                matchedProduct.Reload();
            }

            else
                AllProducts.Insert(0, new ProductViewModel(Models.Product.Load(productId)));
        }
    }
}