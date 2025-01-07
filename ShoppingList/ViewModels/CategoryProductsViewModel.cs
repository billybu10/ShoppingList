using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.ViewModels
{
    public class CategoryProductsViewModel : ObservableObject
    {
        public ObservableCollection<ViewModels.ProductViewModel> Products { get; set; } = [];
        public ViewModels.CategoryViewModel Category { get; }

        private bool InternalVisible = false;
        public bool Visible
        {
            get => InternalVisible;
            set
            {
                if (InternalVisible != value)
                {
                    InternalVisible = value;
                    OnPropertyChanged();
                }
            }
        }

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
