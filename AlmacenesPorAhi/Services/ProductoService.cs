using System.Linq;
using AlmacenesPorAhi.Data;
using AlmacenesPorAhi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlmacenesPorAhi.Services;

// ============================================================================
// CLASE: ProductoService
// ----------------------------------------------------------------------------
// Capa de SERVICIO del modulo de inventario. Implementa IProductoService y
// contiene la logica de acceso a datos (CRUD) usando Entity Framework Core.
//
// Arquitectura (MVVM + capa de servicio):
//     Vista (XAML)  ->  ViewModel  ->  ProductoService  ->  DbContext  ->  MySQL
//
// El ViewModel nunca habla con la base de datos directamente: siempre pasa por
// este servicio. Asi la logica de presentacion queda separada de la de datos.
//
// Se inyecta IDbContextFactory<AppDbContext> (en lugar de un DbContext unico)
// porque el DbContext NO es seguro para uso concurrente: la fabrica crea un
// contexto nuevo y de corta vida por cada operacion, lo recomendado en MAUI.
// ============================================================================
public class ProductoService : IProductoService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    /// <summary>Recibe la fabrica de contextos por inyeccion de dependencia.</summary>
    public ProductoService(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    /// <summary>Devuelve todos los productos ordenados por nombre (solo lectura).</summary>
    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        // AsNoTracking: consulta de solo lectura, mas eficiente (no rastrea cambios).
        return await db.Productos
            .AsNoTracking()
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    /// <summary>Busca un producto por su clave primaria. Devuelve null si no existe.</summary>
    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Productos.FindAsync(id);
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
}
