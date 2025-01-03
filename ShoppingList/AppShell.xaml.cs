using ShoppingList.Views;

namespace ShoppingList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(AllShopsPage), typeof(AllShopsPage));
            Routing.RegisterRoute(nameof(AllCategoriesPage), typeof(AllCategoriesPage));
            Routing.RegisterRoute(nameof(AllUnitsPage), typeof(AllUnitsPage));
        }
    }
}
