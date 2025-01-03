using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class ShopProductsViewModel
    {
        public ObservableCollection<ViewModels.ProductViewModel> Products { get; set; } = [];
        public ViewModels.ShopViewModel Shop { get; }

        public ShopProductsViewModel(ShopViewModel Shop)
        {
            this.Shop = Shop;
            Products = new ObservableCollection<ProductViewModel>((new ProductsViewModel()).AllProducts.Where(n => n.Shop == Shop.Text && !n.Active).OrderBy(n => n.Category));
        }

        public ShopProductsViewModel()
        {
        }

        public void Reload()
        {
            Products.Clear();
            Products = new ObservableCollection<ProductViewModel>((new ProductsViewModel()).AllProducts.Where(n => n.Shop == Shop.Text && !n.Active).OrderBy(n => n.Category));

        }
    }
}
