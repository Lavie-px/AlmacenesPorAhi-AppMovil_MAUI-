using AlmacenesPorAhi.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlmacenesPorAhi.ViewModels;

// ============================================================================
// VIEWMODEL: MainMenuViewModel
// ----------------------------------------------------------------------------
// Logica de la pantalla de inicio (menu principal). Expone el comando que
// navega al modulo de inventario. Sigue el patron MVVM: la vista no navega
// por si misma, invoca este comando.
// ============================================================================
public partial class MainMenuViewModel : ObservableObject
{
    // Navega a la lista de productos (modulo de Gestion de Inventario).
    [RelayCommand]
    private async Task IrAInventarioAsync()
    {
        await Shell.Current!.GoToAsync(nameof(ProductoListPage));
    }


    [RelayCommand]
    private async Task AbrirClientes()
    {
        await Shell.Current!.GoToAsync(nameof(ClientesListPage));
    }
}
