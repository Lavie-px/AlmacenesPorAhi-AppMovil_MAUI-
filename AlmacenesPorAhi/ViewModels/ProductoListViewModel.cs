using System.Collections.ObjectModel;
using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;
using AlmacenesPorAhi.Helpers;
using AlmacenesPorAhi.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlmacenesPorAhi.ViewModels;

// ============================================================================
//  VIEWMODEL:  ProductoListViewModel
//  ----------------------------------------------------------------------------
//  Es el "cerebro" de la pantalla de listado de productos (ProductoListPage).
//  Pertenece a la capa VIEWMODEL del patron MVVM.
//
//  Responsabilidades:
//    - Mantener la coleccion de productos que la vista muestra.
//    - Exponer COMANDOS (acciones) que la vista invoca: cargar, agregar,
//      editar y eliminar.
//    - Comunicarse con la capa de servicios (IProductoService) para obtener
//      o modificar los datos. NUNCA habla directamente con la base de datos.
//
//  [ObservableProperty] y [RelayCommand] son generadores de codigo del
//  CommunityToolkit.Mvvm: escriben por nosotros el codigo repetitivo de
//  notificacion de cambios y de comandos. Por eso la clase es "partial".
// ============================================================================
public partial class ProductoListViewModel : ObservableObject
{
    private readonly IProductoService _service;

    // El servicio llega por INYECCION DE DEPENDENCIA (registrado en MauiProgram.cs).
    public ProductoListViewModel(IProductoService service)
    {
        _service = service;
        Titulo = "Inventario de Productos";
    }

    // Coleccion observable: cuando se agregan o quitan elementos, la interfaz
    // (el CollectionView) se actualiza automaticamente.
    public ObservableCollection<Producto> Productos { get; } = new();

    // Propiedades observables: al cambiar su valor, la vista se entera y se
    // refresca sola (binding bidireccional).
    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private bool estaCargando;   // controla el indicador de carga (spinner)

    // Mensaje de error visible en pantalla (banner rojo) cuando falla el acceso
    // a la base de datos. HayError controla si ese banner se muestra.
    [ObservableProperty]
    private string mensajeError = string.Empty;

    [ObservableProperty]
    private bool hayError;

    // ----- COMANDO: Cargar la lista -----------------------------------------
    // Lee todos los productos desde el servicio y reconstruye la coleccion.
    [RelayCommand]
    private async Task CargarAsync()
    {
        if (EstaCargando) return;   // evita ejecuciones simultaneas

        try
        {
            EstaCargando = true;
            HayError = false;
            MensajeError = string.Empty;

            var lista = await _service.ObtenerTodosAsync();

            Productos.Clear();
            foreach (var p in lista)
                Productos.Add(p);
        }
        catch (Exception ex)
        {
            // CAPTURA del error de base de datos: en vez de cerrarse, la app
            // muestra un mensaje claro en un banner y deja la lista vacia.
            MensajeError = DatabaseErrors.Describe(ex);
            HayError = true;
        }
        finally
        {
            EstaCargando = false;
        }
    }

    // ----- COMANDO: Crear nuevo producto ------------------------------------
    // Navega al formulario sin pasar un Id (modo "nuevo").
    [RelayCommand]
    private async Task AgregarAsync()
    {
        // Navegacion por rutas del Shell. La ruta se registra en AppShell.xaml.cs
        await Shell.Current!.GoToAsync(nameof(ProductoFormPage));
    }

    // ----- COMANDO: Editar producto seleccionado ----------------------------
    // Navega al formulario pasando el Id del producto como parametro de ruta.
    [RelayCommand]
    private async Task EditarAsync(Producto? producto)
    {
        if (producto is null) return;

        // Se pasa el Id como parametro de consulta (query string).
        // ProductoFormViewModel lo recibe gracias al atributo [QueryProperty].
        await Shell.Current!.GoToAsync($"{nameof(ProductoFormPage)}?id={producto.Id}");
    }

    // ----- COMANDO: Eliminar producto ---------------------------------------
    // Pide confirmacion al usuario y, si acepta, elimina y recarga la lista.
    [RelayCommand]
    private async Task EliminarAsync(Producto? producto)
    {
        if (producto is null) return;

        bool confirmar = await Shell.Current!.DisplayAlertAsync(
            "Confirmar eliminacion",
            $"Desea eliminar \"{producto.Nombre}\"?",
            "Si, eliminar", "Cancelar");

        if (!confirmar) return;

        try
        {
            await _service.EliminarAsync(producto.Id);
            await CargarAsync();   // refresca el listado
        }
        catch (Exception ex)
        {
            // Captura cualquier error de base de datos al eliminar.
            await Shell.Current!.DisplayAlertAsync(
                "Error", DatabaseErrors.Describe(ex), "Aceptar");
        }
    }
}
