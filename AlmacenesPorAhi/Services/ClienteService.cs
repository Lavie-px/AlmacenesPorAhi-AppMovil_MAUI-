using AlmacenesPorAhi.Data;
using AlmacenesPorAhi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmacenesPorAhi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IDbContextFactory<AppDbContext> _dbFactory;

        /// <summary>Recibe la fabrica de contextos por inyeccion de dependencia.</summary>
        public ClienteService(IDbContextFactory<AppDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Cliente?> BuscarPorRutAsync(string rut)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Clientes.FirstOrDefaultAsync(c =>  c.Rut == rut);
        }

        public async Task AgregarAsync(Cliente cliente)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.Clientes.Add(cliente);
            await db.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cliente cliente)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var existente = await db.Clientes
                .FirstOrDefaultAsync(c => c.Rut == cliente.Rut);

            if (existente == null)
                return;

            existente.Nombre = cliente.Nombre;
            existente.ApellidoPaterno = cliente.ApellidoPaterno;
            existente.ApellidoMaterno = cliente.ApellidoMaterno;
            existente.Email = cliente.Email;
            existente.Telefono = cliente.Telefono;
            existente.Direccion = cliente.Direccion;
            existente.Estado = cliente.Estado;

            await db.SaveChangesAsync();
        }

        public async Task EliminarAsync(string rut)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var cliente = await db.Clientes
                .FirstOrDefaultAsync(c => c.Rut == rut);

            if (cliente != null)
            {
                db.Clientes.Remove(cliente);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Cliente>> ObtenerClientesAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.Clientes.ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            // AsNoTracking: solo lectura, más eficiente
            return await db.Clientes
                .AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }


    }
}
