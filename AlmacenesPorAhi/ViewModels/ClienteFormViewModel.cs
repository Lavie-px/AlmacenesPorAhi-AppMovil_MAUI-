using AlmacenesPorAhi.Helpers;
using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlmacenesPorAhi.ViewModels;

[QueryProperty(nameof(RutQuery), "rut")]
public partial class ClienteFormViewModel : ObservableObject
{
    private readonly IClienteService _service;
    private DateTime _fechaRegistro = DateTime.Now;

    // Control del modo del formulario
    private bool _modoEdicion;
    private string _rutOriginal = string.Empty;

    public ClienteFormViewModel(IClienteService service)
    {
        _service = service;
    }

    // ---------------- ESTADOS ----------------
    public List<string> Estados { get; } = new()
    {
        "Activo",
        "Inactivo"
    };

    // ---------------- QUERY RUT ----------------
    [ObservableProperty]
    private string rutQuery = string.Empty;

    partial void OnRutQueryChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            _ = CargarClienteAsync(value);
        else
        {
            _modoEdicion = false;
            _rutOriginal = string.Empty;
            Titulo = "Nuevo Cliente";
        }
    }

    // ---------------- CAMPOS ----------------
    [ObservableProperty] private string nombre = string.Empty;
    [ObservableProperty] private string apellidoPaterno = string.Empty;
    [ObservableProperty] private string apellidoMaterno = string.Empty;
    [ObservableProperty] private string rut = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string telefono = string.Empty;
    [ObservableProperty] private string direccion = string.Empty;
    [ObservableProperty] private string estado = "Activo";
    [ObservableProperty] private string titulo = "Nuevo Cliente";

    // ---------------- CARGAR POR RUT ----------------
    private async Task CargarClienteAsync(string rut)
    {
        var c = await _service.BuscarPorRutAsync(rut);

        if (c is null)
        {
            _modoEdicion = false;
            _rutOriginal = string.Empty;

            Titulo = "Nuevo Cliente";
            Rut = rut;
            return;
        }

        _modoEdicion = true;
        _rutOriginal = c.Rut;

        Nombre = c.Nombre ?? string.Empty;
        ApellidoPaterno = c.ApellidoPaterno ?? string.Empty;
        ApellidoMaterno = c.ApellidoMaterno ?? string.Empty;
        Rut = c.Rut ?? string.Empty;
        Email = c.Email ?? string.Empty;
        Telefono = c.Telefono ?? string.Empty;
        Direccion = c.Direccion ?? string.Empty;
        Estado = c.Estado ?? "Activo";

        _fechaRegistro = c.FechaRegistro;
        Titulo = "Editar Cliente";
    }

    // ---------------- GUARDAR ----------------
    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(ApellidoPaterno) ||
            string.IsNullOrWhiteSpace(Rut) ||
            string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current!.DisplayAlertAsync(
                "Validación",
                "Nombre, apellido, RUT y email son obligatorios.",
                "Aceptar");
            return;
        }

        try
        {
            // CREAR CLIENTE NUEVO
            if (!_modoEdicion)
            {
                var existeRut = await _service.BuscarPorRutAsync(Rut);

                if (existeRut != null)
                {
                    await Shell.Current!.DisplayAlertAsync(
                        "RUT duplicado",
                        "Ya existe un cliente registrado con ese RUT.",
                        "Aceptar");
                    return;
                }

                var nuevoCliente = new Cliente
                {
                    Nombre = Nombre.Trim(),
                    ApellidoPaterno = ApellidoPaterno.Trim(),
                    ApellidoMaterno = ApellidoMaterno.Trim(),
                    Rut = Rut.Trim(),
                    Email = Email.Trim(),
                    Telefono = Telefono.Trim(),
                    Direccion = Direccion.Trim(),
                    Estado = Estado,
                    FechaRegistro = DateTime.Now
                };

                await _service.AgregarAsync(nuevoCliente);
            }
            // EDITAR CLIENTE EXISTENTE
            else
            {
                var cliente = new Cliente
                {
                    Nombre = Nombre.Trim(),
                    ApellidoPaterno = ApellidoPaterno.Trim(),
                    ApellidoMaterno = ApellidoMaterno.Trim(),
                    Rut = _rutOriginal, // importante
                    Email = Email.Trim(),
                    Telefono = Telefono.Trim(),
                    Direccion = Direccion.Trim(),
                    Estado = Estado,
                    FechaRegistro = _fechaRegistro
                };

                await _service.ActualizarAsync(cliente);
            }

            await Shell.Current!.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current!.DisplayAlertAsync(
                "Error",
                DatabaseErrors.Describe(ex),
                "Aceptar");
        }
    }

    // ---------------- CANCELAR ----------------
    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current!.GoToAsync("..");
    }
}