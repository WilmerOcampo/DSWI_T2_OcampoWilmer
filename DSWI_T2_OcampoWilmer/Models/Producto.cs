using System.ComponentModel.DataAnnotations;

namespace DSWI_T2_OcampoWilmer.Models
{
    public class Producto
    {

        [Required, Display(Name = "Id")]public int Id { get; set; }
        [Required, Display(Name = "Producto")]public string? Nombre { get; set; }
        [Required]public int IdProveedor { get; set; }
        [Required]public int IdCategoria { get; set; }
        [Required, Display(Name = "Und. Medida")]public string? UnidadMedida { get; set; }
        [Required, Display(Name = "Precio Und.")]public decimal PrecioUnidad { get; set; }
        [Required, Display(Name = "Stock")]public int Stock { get; set; }

        // Atributos para mostrar nombre de Proveedor y Categoria
        [Display(Name = "Proveedor")]public string? NombreProveedor { get; set; }
        [Display(Name = "Categoría")] public string? NombreCategoria { get; set; }
    }
}
