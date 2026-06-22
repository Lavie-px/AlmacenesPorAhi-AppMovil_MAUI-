using AlmacenesPorAhi.Models;

namespace AlmacenesPorAhi.Services;

// ============================================================================
//  INTERFAZ:  IProductoService
//  ----------------------------------------------------------------------------
//  Define el CONTRATO de operaciones que la aplicacion puede realizar sobre
//  los productos, SIN exponer como se implementan internamente.
//
//  Pertenece a la capa SERVICES del patron MVVM.
//
//  Trabajar contra una interfaz (y no contra la clase concreta) permite:
//    - Inyectar el servicio en los ViewModels mediante inyeccion de dependencia.
//    - Reemplazar la implementacion (por ejemplo, por una version de prueba)
//      sin modificar los ViewModels.
//
//  Las operaciones representan el CRUD completo:
//    C - Crear   (AgregarAsync)
//    R - Leer    (ObtenerTodosAsync / ObtenerPorIdAsync)
//    U - Actualizar (ActualizarAsync)
//    D - Eliminar   (EliminarAsync)
//
//  Todas las operaciones son asincronas (Task) porque el acceso a la base de
//  datos es una operacion de entrada/salida: no debe bloquear la interfaz.
// ============================================================================
public interface IProductoService
{
    Task<List<Producto>> ObtenerTodosAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);
}
