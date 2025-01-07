using ShoppingList.ViewModels;

namespace ShoppingList.Views.Controls;

public partial class ProductContentView : ContentView
{
    public static readonly BindableProperty ProductViewModelProperty =
        BindableProperty.Create(
            nameof(ProductViewModel),
            typeof(ProductViewModel),
            typeof(ProductContentView),
            default(ProductViewModel),
            BindingMode.TwoWay,
            propertyChanged: OnProductViewModelChanged
        );

    public ProductViewModel ProductViewModel
    {
        get => (ProductViewModel)GetValue(ProductViewModelProperty);
        set => SetValue(ProductViewModelProperty, value);
    }

    public ProductContentView(ProductViewModel model)
    {
        InitializeComponent();
        ProductViewModel = model;
        BindingContext = ProductViewModel;
    }

    public ProductContentView()
	{
		InitializeComponent();
	}

    private static void OnProductViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ProductContentView productView)
        {
            productView.BindingContext = newValue;
        }
    }
}