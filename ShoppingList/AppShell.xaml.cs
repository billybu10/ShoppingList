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
        }
    }
}
