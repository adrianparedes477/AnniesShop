
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnniesShop.Models
{
    public class Direccion
    {
        [Key]
        public int DireccionId { get; set; }
        [Required]
        [StringLength(255)]
        public string Address { get; set; } =null!;
        [Required]
        [StringLength(20)]
        public string Cuidad { get; set; } =null!;
        [Required]
        [StringLength(20)]
        public string Estado { get; set; } =null!;
        [Required]
        [StringLength(10)]
        public string CodigoPostal { get; set; } = null!;

        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } =null!;
    }
}