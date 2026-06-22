namespace AlmacenesPorAhi.Helpers;

// ============================================================================
// AYUDANTE (Helper): DatabaseErrors
// ----------------------------------------------------------------------------
// No es un servicio ni accede a datos: es una utilidad de presentacion que
// traduce las excepciones de base de datos a un mensaje claro en espanol.
// Por eso vive en la carpeta Helpers y no en Services.
//
// Recorre la cadena de excepciones internas e inspecciona el nombre del tipo,
// para no depender de los tipos concretos del proveedor (MySqlException, etc.).
// ============================================================================
public static class DatabaseErrors
{
    // Devuelve un mensaje legible a partir de una excepcion de base de datos.
    public static string Describe(Exception ex)
    {
        // Recorre la excepcion y todas sus excepciones internas.
        for (Exception? actual = ex; actual is not null; actual = actual.InnerException)
        {
            var tipo = actual.GetType().FullName ?? string.Empty;

            // Errores tipicos de conexion a MySQL (MySqlConnector lanza MySqlException).
            if (tipo.Contains("MySqlException") || tipo.Contains("SqlException"))
            {
                return "No se pudo conectar a la base de datos MySQL.\n\n" +
                       "Verifica que:\n" +
                       "  - El servicio de MySQL este encendido.\n" +
                       "  - El usuario y la contrasena en appsettings.json sean correctos.\n" +
                       "  - El servidor y el puerto (3306) sean correctos.\n\n" +
                       "Detalle tecnico: " + actual.Message;
            }
        }

        // Cualquier otro error de base de datos.
        return "Ocurrio un error al acceder a la base de datos.\n\n" +
               "Detalle tecnico: " + ex.Message;
    }
}
