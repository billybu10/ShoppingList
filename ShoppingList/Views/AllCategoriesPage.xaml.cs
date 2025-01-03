namespace ShoppingList.Views;

public partial class AllCategoriesPage : ContentPage
{
	public AllCategoriesPage()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AllProductsPage), typeof(AllProductsPage));
        Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
    }
}