using AlmacenesPorAhi.ViewModels;

namespace AlmacenesPorAhi.Views;

// ============================================================================
// CODE-BEHIND: ClientesListPage
// ----------------------------------------------------------------------------
// Responsabilidad mínima en MVVM:
// 1. Recibir el ViewModel por inyección de dependencias
// 2. Asignarlo como BindingContext
// 3. Recargar la lista al volver a la página
// ============================================================================
public partial class ClientesListPage : ContentPage
{
    private readonly ClienteListViewModel _vm;

    public ClientesListPage(ClienteListViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    // Se ejecuta cada vez que la página aparece en pantalla
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Ejecuta el comando de carga del ViewModel
        await _vm.CargarCommand.ExecuteAsync(null);
    }
}