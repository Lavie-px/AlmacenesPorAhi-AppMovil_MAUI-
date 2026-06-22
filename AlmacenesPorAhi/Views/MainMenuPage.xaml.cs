using AlmacenesPorAhi.ViewModels;

namespace AlmacenesPorAhi.Views;

// ============================================================================
// CODE-BEHID: MainMenuPage
// Solo recibe el ViewModel inyectado y lo asigna como BindingContext.
// ============================================================================
public partial class MainMenuPage : ContentPage
{
    public MainMenuPage(MainMenuViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
