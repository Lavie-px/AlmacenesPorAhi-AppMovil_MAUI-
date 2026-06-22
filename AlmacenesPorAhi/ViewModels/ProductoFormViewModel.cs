using AlmacenesPorAhi.Helpers;
using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlmacenesPorAhi.ViewModels;

// ============================================================================
// VIEWMODEL: ProductoFormViewModel
// ----------------------------------------------------------------------------
// Controla el formulario de alta y edicion de un producto.
// [QueryProperty] recibe el "id" enviado en la navegacion: 0 = nuevo producto.
// ============================================================================
[QueryProperty(nameof(ProductoId), "id")]
public partial class ProductoFormViewModel : ObservableObject
{
    private readonly IProductoService _service;

    // Se conserva la fecha original al editar (no se sobreescribe).
    private DateTime _fechaRegistro = DateTime.Now;

    public ProductoFormViewModel(IProductoService service)
    {
        _service = service;
    }

    // Opciones para los selectores (Picker) del formulario.
    public List<string> Tallas { get; } = new() { "XS", "S", "M", "L", "XL", "XXL" };
    public List<string> Estados { get; } = new() { "Activo", "Agotado", "Descontinuado" };

    [ObservableProperty]
    private int productoId;

    partial void OnProductoIdChanged(int value)
    {
        if (value > 0)
            _ = CargarProductoAsync(value);
    }

    // Campos enlazados al formulario.
    [ObservableProperty] private string nombre = string.Empty;
    [ObservableProperty] private string? descripcion;
    [ObservableProperty] private string categoria = string.Empty;
    [ObservableProperty] private string talla = "M";
    [ObservableProperty] private string color = string.Empty;
    [ObservableProperty] private decimal precio;
    [ObservableProperty] private int stock;
    [ObservableProperty] private string estado = "Activo";

    // Foto del producto. Al cambiar, se recalcula TieneImagen para mostrar/ocultar
    // la vista previa.
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TieneImagen))]
    private string? imagenUrl;

    public bool TieneImagen => !string.IsNullOrWhiteSpace(ImagenUrl);

    [ObservableProperty] private string titulo = "Nuevo Producto";

    private async Task CargarProductoAsync(int id)
    {
        var p = await _service.ObtenerPorIdAsync(id);
        if (p is null) return;

        Nombre = p.Nombre;
        Descripcion = p.Descripcion;
        Categoria = p.Categoria;
        Talla = p.Talla;
        Color = p.Color;
        Precio = p.Precio;
        Stock = p.Stock;
        Estado = p.Estado;
        ImagenUrl = p.ImagenUrl;
        _fechaRegistro = p.FechaRegistro;
        Titulo = "Editar Producto";
    }

    // COMANDO: seleccionar una foto desde el equipo.
    [RelayCommand]
    private async Task SeleccionarFotoAsync()
    {
        try
        {
            var resultado = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una foto del producto",
                FileTypes = FilePickerFileType.Images
            });

            if (resultado is not null)
                ImagenUrl = resultado.FullPath; // ruta local de la imagen elegida
        }
        catch (Exception ex)
        {
            await Shell.Current!.DisplayAlertAsync("Foto",
                "No se pudo seleccionar la imagen.\n\n" + ex.Message, "Aceptar");
        }
    }

    // COMANDO: guardar (crea o actualiza segun ProductoId).
    [RelayCommand]
    private async Task GuardarAsync()
    {
        // Validaciones minimas en la capa de presentacion.
        if (string.IsNullOrWhiteSpace(Nombre))
        {
            await Shell.Current!.DisplayAlertAsync("Validacion", "El nombre es obligatorio.", "Aceptar");
            return;
        }
        if (string.IsNullOrWhiteSpace(Categoria))
        {
            await Shell.Current!.DisplayAlertAsync("Validacion", "La categoria es obligatoria.", "Aceptar");
            return;
        }
        if (Precio <= 0)
        {
            await Shell.Current!.DisplayAlertAsync("Validacion", "El precio debe ser mayor que 0.", "Aceptar");
            return;
        }

        var producto = new Producto
        {
            Id = ProductoId,
            Nombre = Nombre.Trim(),
            Descripcion = Descripcion?.Trim(),
            Categoria = Categoria.Trim(),
            Talla = Talla,
            Color = Color.Trim(),
            Precio = Precio,
            Stock = Stock,
            Estado = Estado,
            ImagenUrl = ImagenUrl,
            FechaRegistro = ProductoId == 0 ? DateTime.Now : _fechaRegistro
        };

        try
        {
            if (ProductoId == 0)
                await _service.AgregarAsync(producto);
            else
                await _service.ActualizarAsync(producto);

            await Shell.Current!.GoToAsync(".."); // vuelve al listado
        }
        catch (Exception ex)
        {
            // Captura cualquier error de base de datos sin cerrar la app.
            await Shell.Current!.DisplayAlertAsync("Error", DatabaseErrors.Describe(ex), "Aceptar");
        }
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current!.GoToAsync("..");
    }
}
