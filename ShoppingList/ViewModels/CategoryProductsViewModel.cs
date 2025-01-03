using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class CategoryProductsViewModel
    {
        public ObservableCollection<ViewModels.ProductViewModel> Products { get; set; } = [];
        public ViewModels.CategoryViewModel Category { get; }

        public CategoryProductsViewModel(CategoryViewModel Category) {
            this.Category = Category;
            Products = new ObservableCollection<ProductViewModel>((new ProductsViewModel()).AllProducts.Where(n => n.Category == Category.Text).OrderBy(n => n.Active));
        }

        public CategoryProductsViewModel()
        {
        }

        public void Reload()
        {
            Products.Clear();
            Products = new ObservableCollection<ProductViewModel>((new ProductsViewModel()).AllProducts.Where(n => n.Category == Category.Text).OrderBy(n => n.Active));

        }
    }
}
