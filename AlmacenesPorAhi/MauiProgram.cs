using System.Reflection;
using AlmacenesPorAhi.Data;
using AlmacenesPorAhi.Services;
using AlmacenesPorAhi.ViewModels;
using AlmacenesPorAhi.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AlmacenesPorAhi;

// ============================================================================
// MauiProgram: punto de entrada y configuracion de la aplicacion.
// ----------------------------------------------------------------------------
// CreateMauiApp() se ejecuta una sola vez al iniciar. Aqui se configura:
//   1. La app base y sus fuentes.
//   2. La lectura de appsettings.json (de donde sale la cadena de conexion).
//   3. El contexto de base de datos (MySQL, via Pomelo) con esa cadena.
//   4. La inyeccion de dependencia: repositorio, servicio, ViewModels y vistas.
//   5. La creacion de la base de datos si no existe (con datos de ejemplo).
// ============================================================================
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // 1. Aplicacion base y fuentes.
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 2. Lectura de la configuracion externa (appsettings.json).
        // El archivo viaja embebido en el ejecutable; se busca por el final de
        // su nombre para que funcione aunque cambie el espacio de nombres.
        var ensamblado = Assembly.GetExecutingAssembly();
        var nombreRecurso = ensamblado
            .GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));

        if (nombreRecurso is not null)
        {
            using var stream = ensamblado.GetManifestResourceStream(nombreRecurso);
            if (stream is not null)
                builder.Configuration.AddJsonStream(stream);
        }

        // Se obtiene la cadena de conexion de MySQL desde la configuracion.
        var cadenaMySql = builder.Configuration.GetConnectionString("MySql") ?? string.Empty;

        // 3. Registro del contexto de base de datos (Entity Framework Core).
        // Se usa una fabrica de DbContext: crea un contexto nuevo y de corta
        // vida por operacion, lo recomendado para MAUI.
        builder.Services.AddDbContextFactory<AppDbContext>(options =>
        {
            // Pomelo: proveedor de MySQL/MariaDB. Se indica la version del servidor.
            // Si usas MariaDB, cambia MySqlServerVersion por MariaDbServerVersion.
            options.UseMySql(cadenaMySql, new MySqlServerVersion(new Version(8, 0, 0)));
        });

        // 4. Inyeccion de dependencia.
        // Servicio de negocio (usa el DbContext a traves de la fabrica).
        builder.Services.AddSingleton<IProductoService, ProductoService>();
        builder.Services.AddTransient<IClienteService, ClienteService>();

        // ViewModels (uno nuevo por navegacion).
        builder.Services.AddTransient<MainMenuViewModel>();
        builder.Services.AddTransient<ProductoListViewModel>();
        builder.Services.AddTransient<ProductoFormViewModel>();
        builder.Services.AddTransient<ClienteListViewModel>();
        builder.Services.AddTransient<ClienteFormViewModel>();

        // Vistas (al registrarlas, el contenedor les inyecta su ViewModel).
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<ProductoListPage>();
        builder.Services.AddTransient<ProductoFormPage>();
        builder.Services.AddTransient<ClientesListPage>();
        builder.Services.AddTransient<ClienteFormPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // 5. Inicializacion de la base de datos (crea la BD y la semilla si no existen).
        InicializarBaseDeDatos(app.Services);

        return app;
    }

    private static void InicializarBaseDeDatos(IServiceProvider services)
    {
        try
        {
            var factory = services.GetRequiredService<IDbContextFactory<AppDbContext>>();
            using var db = factory.CreateDbContext();

            // Crea la base y sus tablas si no existen (con los datos de ejemplo).
            db.Database.EnsureCreated();

            // Comprueba que el esquema coincida con el modelo actual. Si el modelo
            // cambio (por ejemplo, se agrego una columna) y la base proviene de una
            // version anterior, esta consulta de prueba fallara. En ese caso se
            // recrea la base para dejar el esquema al dia.
            // (EnsureCreated no aplica cambios de esquema sobre una base ya existente.)
            try
            {
                _ = db.Productos.AsNoTracking().FirstOrDefault();
            }
            catch
            {
                using var dbReset = factory.CreateDbContext();
                dbReset.Database.EnsureDeleted();
                dbReset.Database.EnsureCreated();
            }
        }
        catch (Exception ex)
        {
            // Si la conexion falla (servidor apagado, etc.), se registra el error.
            // La app no se cae: las pantallas muestran el error de forma controlada.
            System.Diagnostics.Debug.WriteLine(
                $"[Inicializacion BD] No se pudo crear/conectar la base de datos: {ex.Message}");
        }
    }
}
