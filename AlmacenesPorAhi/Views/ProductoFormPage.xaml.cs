using AlmacenesPorAhi.ViewModels;

namespace AlmacenesPorAhi.Views;

// ============================================================================
//  CODE-BEHIND:  ProductoFormPage
//  ----------------------------------------------------------------------------
//  Igual que en el listado, el code-behind solo recibe el ViewModel inyectado
//  y lo asigna como BindingContext. Toda la logica vive en el ViewModel.
// ============================================================================
public partial class ProductoFormPage : ContentPage
{
    public ProductoFormPage(ProductoFormViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
