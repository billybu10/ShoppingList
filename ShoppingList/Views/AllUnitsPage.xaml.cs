namespace ShoppingList.Views;

public partial class AllUnitsPage : ContentPage
{
	public AllUnitsPage()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AllProductsPage), typeof(AllProductsPage));
        Routing.RegisterRoute(nameof(UnitPage), typeof(UnitPage));
    }
}