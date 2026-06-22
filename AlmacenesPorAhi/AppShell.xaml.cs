using AlmacenesPorAhi.Views;

namespace AlmacenesPorAhi;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rutas a las que se navega por codigo (Shell.Current.GoToAsync).
        Routing.RegisterRoute(nameof(ProductoListPage), typeof(ProductoListPage));
        Routing.RegisterRoute(nameof(ProductoFormPage), typeof(ProductoFormPage));
    }
}
