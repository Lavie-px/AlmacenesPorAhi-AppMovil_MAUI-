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

        /// <summary>Inserta un nuevo producto. EF Core asigna el Id automaticamente.</summary>
        public async Task AgregarAsync(Producto producto)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.Productos.Add(producto);     // marca la entidad para insertar
            await db.SaveChangesAsync();    // ejecuta el INSERT en la base de datos
        }

        /// <summary>Actualiza un producto existente (se identifica por su Id).</summary>
        public async Task ActualizarAsync(Producto producto)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.Productos.Update(producto);  // marca la entidad para actualizar
            await db.SaveChangesAsync();    // ejecuta el UPDATE
        }

        /// <summary>Elimina un producto por Id. Si no existe, no hace nada.</summary>
        public async Task EliminarAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var producto = await db.Productos.FindAsync(id);
            if (producto is not null)
            {
                db.Productos.Remove(producto);  // marca la entidad para borrar
                await db.SaveChangesAsync();    // ejecuta el DELETE
            }
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
            db.Clientes.Update(cliente);
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
