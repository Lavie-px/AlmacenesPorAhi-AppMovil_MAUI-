using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenesPorAhi.Models;

/// <summary>
/// Entidad de negocio que representa un producto del inventario.
/// Cada propiedad de esta clase se convierte en una columna de la
/// tabla "Productos" en MySQL.
/// </summary>
public class Producto
{
    // Clave primaria. EF Core la detecta por convencion al llamarse "Id"
    // y le aplica AUTO_INCREMENT automaticamente en MySQL.
    [Key]
    public int Id { get; set; }

    // Nombre del producto: obligatorio, maximo 100 caracteres.
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre del producto")]
    public string Nombre { get; set; } = string.Empty;

    // Descripcion opcional, maximo 500 caracteres.
    [StringLength(500, ErrorMessage = "La descripcion no puede superar los 500 caracteres.")]
    [Display(Name = "Descripcion")]
    public string? Descripcion { get; set; }

    // Categoria obligatoria (Ej: Camisas, Pantalones, Accesorios).
    [Required(ErrorMessage = "Debe indicar la categoria.")]
    [StringLength(50)]
    [Display(Name = "Categoria")]
    public string Categoria { get; set; } = string.Empty;

    // Talla obligatoria (XS, S, M, L, XL, XXL).
    [Required(ErrorMessage = "Debe indicar la talla.")]
    [StringLength(10)]
    [Display(Name = "Talla")]
    public string Talla { get; set; } = string.Empty;

    // Color obligatorio.
    [Required(ErrorMessage = "Debe indicar el color.")]
    [StringLength(30)]
    [Display(Name = "Color")]
    public string Color { get; set; } = string.Empty;

    // Precio: obligatorio, mayor que 0. Se almacena como decimal(18,2).
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Precio (CLP)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    // Cantidad disponible en stock. No puede ser negativa.
    [Required(ErrorMessage = "El stock es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    [Display(Name = "Stock disponible")]
    public int Stock { get; set; }

    // Estado del producto: Activo, Agotado, Descontinuado.
    [Required(ErrorMessage = "Debe indicar el estado.")]
    [StringLength(20)]
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "Activo";

    // Fecha en que se registro el producto en el sistema.
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    // Foto del producto: ruta local (seleccionada con el explorador) o URL.
    // Opcional. Maximo 500 caracteres.
    [StringLength(500)]
    [Display(Name = "Foto")]
    public string? ImagenUrl { get; set; }

    // Propiedad calculada (no se guarda en la BD): indica si hay foto.
    // Sirve para mostrar la imagen o un marcador de posicion en la interfaz.
    [NotMapped]
    public bool TieneImagen => !string.IsNullOrWhiteSpace(ImagenUrl);
}
