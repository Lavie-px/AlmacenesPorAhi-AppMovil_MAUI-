using AlmacenesPorAhi.ViewModels;

namespace AlmacenesPorAhi.Views;

// ============================================================================
// CODE-BEHIND: ClienteFormPage
// ----------------------------------------------------------------------------
// Solo conecta el ViewModel con la vista (MVVM puro)
// ============================================================================
public partial class ClienteFormPage : ContentPage
{
    public ClienteFormPage(ClienteFormViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}