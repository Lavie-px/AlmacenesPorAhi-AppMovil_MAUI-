using AlmacenesPorAhi.ViewModels;

namespace AlmacenesPorAhi.Views;

// ============================================================================
//  CODE-BEHIND:  ProductoListPage
//  ----------------------------------------------------------------------------
//  En MVVM el code-behind se mantiene al MINIMO. Su unica tarea aqui es:
//    1. Recibir el ViewModel por inyeccion de dependencia.
//    2. Asignarlo como BindingContext (origen de los bindings del XAML).
//    3. Recargar la lista cada vez que la pagina aparece (OnAppearing).
// ============================================================================
public partial class ProductoListPage : ContentPage
{
    private readonly ProductoListViewModel _vm;

    public ProductoListPage(ProductoListViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;   // conecta el XAML con el ViewModel
    }

    // OnAppearing se ejecuta cada vez que la pagina se muestra en pantalla,
    // incluyendo al volver del formulario. Asi la lista siempre queda
    // actualizada despues de crear, editar o eliminar un producto.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CargarCommand.ExecuteAsync(null);
    }
}
