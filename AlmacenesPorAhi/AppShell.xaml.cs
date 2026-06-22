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

        //Rutas a las que se navega hacia clientes
        Routing.RegisterRoute(nameof(ClientesListPage), typeof(ClientesListPage));
        Routing.RegisterRoute(nameof(ClienteFormPage),typeof(ClienteFormPage));
    }
}
