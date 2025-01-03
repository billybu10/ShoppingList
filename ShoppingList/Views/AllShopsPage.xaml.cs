namespace ShoppingList.Views;

public partial class AllShopsPage : ContentPage
{
	public AllShopsPage()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AllProductsPage), typeof(AllProductsPage));
        Routing.RegisterRoute(nameof(ShopPage), typeof(ShopPage));
    }
}