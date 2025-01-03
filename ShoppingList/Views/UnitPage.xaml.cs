namespace ShoppingList.Views;

public partial class UnitPage : ContentPage
{
	public UnitPage()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AllUnitsPage), typeof(AllUnitsPage));
    }
}