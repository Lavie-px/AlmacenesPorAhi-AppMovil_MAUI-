using AlmacenesPorAhi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AlmacenesPorAhi.Services
{
    public interface IClienteService
    {
        //tasks creadas para hacer un crud completo
        Task<List<Cliente>> ObtenerClientesAsync();
        Task<Cliente?> BuscarPorRutAsync(String Rut);
        Task<List<Cliente>> ObtenerTodosAsync();
        Task AgregarAsync(Cliente cliente);
        Task ActualizarAsync(Cliente cliente);
        Task EliminarAsync(String Rut);
    }
}
