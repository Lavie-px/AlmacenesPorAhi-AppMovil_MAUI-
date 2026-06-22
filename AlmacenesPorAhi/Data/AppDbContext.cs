using AlmacenesPorAhi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlmacenesPorAhi.Data;

// ============================================================================
// DbContext: AppDbContext
// ----------------------------------------------------------------------------
// Componente central de Entity Framework Core. Representa la sesion con la base
// de datos y traduce entre las clases C# (modelos) y las tablas de MySQL.
// La configuracion del proveedor y la cadena de conexion se inyectan desde
// MauiProgram.cs, por lo que este contexto es independiente del motor.
// ============================================================================
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tabla de productos. En modulos futuros se agregaran mas DbSet.
    public DbSet<Producto> Productos => Set<Producto>();

    // Datos de semilla: productos de ejemplo que se insertan al crear la BD.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fechas fijas (no DateTime.Now) para que la semilla sea determinista.
        modelBuilder.Entity<Producto>().HasData(
            new Producto
            {
                Id = 1, Nombre = "Polera basica algodon", Descripcion = "Polera de algodon peinado, corte regular.",
                Categoria = "Poleras", Talla = "M", Color = "Blanco", Precio = 7990m, Stock = 25,
                Estado = "Activo", FechaRegistro = new DateTime(2024, 1, 15)
            },
            new Producto
            {
                Id = 2, Nombre = "Jeans recto clasico", Descripcion = "Jeans de mezclilla rigida, tiro medio.",
                Categoria = "Pantalones", Talla = "L", Color = "Azul", Precio = 19990m, Stock = 12,
                Estado = "Activo", FechaRegistro = new DateTime(2024, 2, 3)
            },
            new Producto
            {
                Id = 3, Nombre = "Chaqueta de cuero", Descripcion = "Chaqueta biker de cuero sintetico.",
                Categoria = "Abrigos", Talla = "L", Color = "Negro", Precio = 49990m, Stock = 5,
                Estado = "Activo", FechaRegistro = new DateTime(2024, 3, 20)
            },
            new Producto
            {
                Id = 4, Nombre = "Polera estampada", Descripcion = "Polera con estampado grafico edicion limitada.",
                Categoria = "Poleras", Talla = "S", Color = "Rojo", Precio = 9990m, Stock = 0,
                Estado = "Agotado", FechaRegistro = new DateTime(2024, 4, 10)
            },
            new Producto
            {
                Id = 5, Nombre = "Bufanda de lana", Descripcion = "Bufanda tejida, temporada anterior.",
                Categoria = "Accesorios", Talla = "M", Color = "Camel", Precio = 12990m, Stock = 8,
                Estado = "Descontinuado", FechaRegistro = new DateTime(2023, 11, 5)
            }
        );
    }
}
