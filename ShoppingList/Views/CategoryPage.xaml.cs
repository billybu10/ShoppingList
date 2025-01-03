namespace ShoppingList.Views;

public partial class CategoryPage : ContentPage
{
	public CategoryPage()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AllCategoriesPage), typeof(AllCategoriesPage));
    }
}