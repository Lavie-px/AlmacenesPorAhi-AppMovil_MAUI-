namespace AlmacenesPorAhi;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    // En .NET 10 se crea la ventana principal sobreescribiendo CreateWindow
    // (la propiedad MainPage quedo obsoleta). Aqui se fija el AppShell como
    // contenido raiz de la aplicacion.
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
