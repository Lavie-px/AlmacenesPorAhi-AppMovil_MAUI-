# Almacenes Por Ahí — Aplicación Móvil (.NET MAUI)

## Módulo 1: Gestión de Inventario — CRUD de Producto

Aplicación con **.NET MAUI** (.NET 10), patrón **MVVM** y **Entity Framework Core 9**
con el proveedor **Pomelo** sobre **MySQL**. Incluye una **pantalla de inicio** (menú
principal) con el logo y acceso al módulo de inventario, que implementa un CRUD completo
de la entidad **Producto**.

---

## 1. Requisitos

- **Visual Studio 2022/2026** con la carga de trabajo *.NET MAUI* (variante Windows).
- **.NET 10 SDK**.
- **MySQL Server 8.0+** (o MariaDB) en ejecución, con un usuario y contraseña.

> Configurado para ejecutarse en **Windows Machine** (modo desempaquetado).

---

## 2. Base de datos (MySQL, vía Pomelo)

La cadena de conexión vive en **`appsettings.json`**, no en el código:

```json
{
  "ConnectionStrings": {
    "MySql": "Server=localhost;Port=3306;Database=AlmacenesPorAhiDB;User=root;Password=root;"
  }
}
```

**Ajusta `User` y `Password`** a los de tu servidor MySQL local. La base `AlmacenesPorAhiDB`
se crea sola al iniciar (`EnsureCreated`) con productos de ejemplo; no necesitas crearla a mano.

La versión del servidor se indica en `MauiProgram.cs`:
`new MySqlServerVersion(new Version(8, 0, 0))`. Si usas **MariaDB**, cámbiala por
`new MariaDbServerVersion(new Version(11, 4, 0))` (o la versión que tengas).

---

## 3. Ejecutar

1. Asegúrate de que tu **servidor MySQL esté encendido** y de haber puesto el usuario y
   contraseña correctos en `appsettings.json`.
2. Abre `AlmacenesPorAhi.sln`.
3. Clic derecho en la solución → **Restaurar paquetes NuGet**.
4. Selecciona **Windows Machine** y presiona **F5**.
5. Se abre el **menú principal**; pulsa **Gestión de Inventario** para entrar al CRUD.

---

## 4. Arquitectura (MVVM + capa de Servicio)

```
Vista (XAML) → ViewModel → Service → DbContext → MySQL
```

| Carpeta | Responsabilidad |
|---------|-----------------|
| `Models/`       | Entidades del dominio (`Producto`). |
| `Data/`         | `AppDbContext` de EF Core + datos de semilla. |
| `Services/`     | Acceso a datos y lógica de negocio (`IProductoService`, `ProductoService`). |
| `ViewModels/`   | Lógica de presentación y comandos: `MainMenuViewModel`, `ProductoListViewModel`, `ProductoFormViewModel`. |
| `Views/`        | Interfaces XAML: `MainMenuPage` (inicio), `ProductoListPage`, `ProductoFormPage`. |
| `Helpers/`      | Utilidades transversales (`DatabaseErrors`). |

> **Nota de diseño:** se usa MVVM con una capa de servicio, tal como pide la rúbrica.
> No se agrega un patrón Repository porque el `DbContext` de EF Core ya es, en sí mismo,
> una implementación de Repository + Unit of Work; añadir otro sería redundante para este
> alcance. El `ViewModel` nunca accede a la base de datos directamente: siempre pasa por
> el servicio.

---

## 5. Dependencias

| Paquete | Versión |
|---------|---------|
| CommunityToolkit.Mvvm | 8.3.2 |
| Microsoft.EntityFrameworkCore | 9.0.0 |
| Pomelo.EntityFrameworkCore.MySql | 9.0.0 |
| Microsoft.Extensions.Configuration.Json | 9.0.0 |
| Microsoft.Extensions.Logging.Debug | 9.0.0 |

> **Nota sobre versiones:** la app se ejecuta en **.NET 10**, pero el stack de EF Core está
> en la versión **9**. Esto es a propósito: la última versión estable de **Pomelo**
> (proveedor de MySQL) es la **9.0.0**, compatible con EF Core 9; aún no hay una versión
> estable de Pomelo para EF Core 10. Por eso todo el stack de EF Core se mantiene en 9.x.
> EF Core 9 funciona sin problemas sobre .NET 10.

---

## 6. Manejo de errores

Si la base de datos no está accesible, la app **no se cae**: muestra el error en un
banner (en el listado) o en un diálogo (al guardar/eliminar), traducido por
`Helpers/DatabaseErrors`.

---

## 7. Paleta visual — "Earthy Premium"

- **Primario** (identidad): `#2D4236` Verde Bosque — barra, franjas y títulos.
- **Acento** (conversión): `#E76F51` Terracota / `#D4A373` Oro Viejo — botón de acción
  principal (Gestión de Inventario, Agregar, Guardar).
- **Fondo** (neutro claro): `#FAF7F2` Arena / Lino.
- **Superficie** (tarjetas): `#FFFFFF` Blanco Puro.
- **Texto principal**: `#1E2522` Negro Verdoso.
