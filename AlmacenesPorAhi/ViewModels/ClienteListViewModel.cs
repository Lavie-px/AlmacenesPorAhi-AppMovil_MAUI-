using System.Collections.ObjectModel;
using AlmacenesPorAhi.Models;
using AlmacenesPorAhi.Services;
using AlmacenesPorAhi.Helpers;
using AlmacenesPorAhi.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AlmacenesPorAhi.ViewModels;

public partial class ClienteListViewModel : ObservableObject
{
    private readonly IClienteService _service;

    public ClienteListViewModel(IClienteService service)
    {
        _service = service;
        Titulo = "Listado de Clientes";
    }

    //COLECCION PRINCIPAL 
    public ObservableCollection<Cliente> Clientes { get; } = new();

    //PROPIEDADES UI
    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private bool estaCargando;

    [ObservableProperty]
    private string mensajeError = string.Empty;

    [ObservableProperty]
    private bool hayError;

    //COMANDO: CARGAR
    [RelayCommand]
    private async Task CargarAsync()
    {
        if (EstaCargando) return;

        try
        {
            EstaCargando = true;
            HayError = false;
            MensajeError = string.Empty;

            var lista = await _service.ObtenerTodosAsync();

            Clientes.Clear();
            foreach (var c in lista)
                Clientes.Add(c);
        }
        catch (Exception ex)
        {
            MensajeError = DatabaseErrors.Describe(ex);
            HayError = true;
        }
        finally
        {
            EstaCargando = false;
        }
    }

    //COMANDO: AGREGAR
    [RelayCommand]
    private async Task AgregarAsync()
    {
        await Shell.Current!.GoToAsync(nameof(ClienteFormPage));
    }

    //COMANDO: EDITAR
    [RelayCommand]
    private async Task EditarAsync(Cliente? cliente)
    {
        if (cliente is null) return;

        await Shell.Current!.GoToAsync($"{nameof(ClienteFormPage)}?rut={cliente.Rut}");
    }

    //COMANDO: ELIMINAR
    [RelayCommand]
    private async Task EliminarAsync(Cliente? cliente)
    {
        if (cliente is null) return;

        bool confirmar = await Shell.Current!.DisplayAlertAsync(
            "Confirmar eliminación",
            $"¿Desea eliminar a \"{cliente.Nombre}\"?",
            "Sí, eliminar", "Cancelar");

        if (!confirmar) return;

        try
        {
            await _service.EliminarAsync(cliente.Rut);
            await CargarAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current!.DisplayAlertAsync(
                "Error",
                DatabaseErrors.Describe(ex),
                "Aceptar");
        }
    }
}