using ShoppingList.ViewModels;

namespace ShoppingList.Views.Controls;

public partial class CategoryContentView : ContentView
{
    public static readonly BindableProperty CategoryProductsViewModelProperty =
        BindableProperty.Create(
            nameof(CategoryProductsViewModel),
            typeof(CategoryProductsViewModel),
            typeof(CategoryContentView),
            default(CategoryProductsViewModel),
            BindingMode.TwoWay,
            propertyChanged: OnCategoryProductsViewModelChanged
        );

    public CategoryProductsViewModel CategoryProductsViewModel
    {
        get => (CategoryProductsViewModel)GetValue(CategoryProductsViewModelProperty);
        set => SetValue(CategoryProductsViewModelProperty, value);
    }

    public CategoryContentView(CategoryProductsViewModel model)
    {
        InitializeComponent();
        CategoryProductsViewModel = model;
        BindingContext = CategoryProductsViewModel;
    }

    public CategoryContentView()
	{
		InitializeComponent();
	}

    private static void OnCategoryProductsViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CategoryContentView categoryView)
        {
            categoryView.BindingContext = newValue;
        }
    }
}