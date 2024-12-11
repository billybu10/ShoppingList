using System.Linq;

namespace ShoppingList.Views;

public partial class AllProductsPage : ContentPage
{
    public AllProductsPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        productsCollection.SelectedItem = null;
    }
}