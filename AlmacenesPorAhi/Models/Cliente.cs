using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmacenesPorAhi.Models;

/// <summary>
/// Entidad de negocio que representa un cliente.
/// Se mapea a la tabla "Clientes" en la base de datos.
/// </summary>
public class Cliente
{
    // Clave primaria (AUTO_INCREMENT en MySQL)
    [Key]
    public int IdCliente { get; set; }

    // Nombre obligatorio
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    // Apellido paterno obligatorio
    [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
    [StringLength(100)]
    [Display(Name = "Apellido paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    // Apellido materno opcional
    [StringLength(100)]
    [Display(Name = "Apellido materno")]
    public string? ApellidoMaterno { get; set; }

    // RUT obligatorio (identificador lógico único)
    [Required(ErrorMessage = "El RUT es obligatorio.")]
    [StringLength(12)]
    [Display(Name = "RUT")]
    public string Rut { get; set; } = string.Empty;

    // Email obligatorio con formato
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un email válido.")]
    [StringLength(150)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    // Teléfono opcional
    [StringLength(20)]
    [Phone(ErrorMessage = "Debe ingresar un teléfono válido.")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    // Preferencias o notas del cliente
    [StringLength(500)]
    [Display(Name = "Direccion")]
    public string? Direccion { get; set; }

    // Estado del cliente
    [Required]
    [StringLength(20)]
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "Activo";

    // Fecha de registro
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    // Propiedad calculada (no se guarda en BD)
    [NotMapped]
    public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}".Trim();
}