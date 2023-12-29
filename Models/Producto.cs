using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnniesShop.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }
        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } =null!;
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } =null!;
        [Required]
        [StringLength(255)]
        public string Modelo { get; set; } =null!;
        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } =null!;
        [Required]
        public double Precio { get; set; }
        [Required]
        [StringLength(255)]
        public string Imagen { get; set; } =null!;
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; } = null!;
        [Required]
        public int Stock { get; set; }
        [Required]
        [StringLength(100)]
        public string Marca { get; set; }= null!;
        [Required]
        public bool Activo { get; set; }

        public ICollection<Detalle_Pedido> DetallesPedido { get; set; } = null!;
    }
}