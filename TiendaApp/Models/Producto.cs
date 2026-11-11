using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TiendaApp.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("idmarca")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una marca")]
        public int IdMarca { get; set; }

        [Column("Descripcion")]
        [Required(ErrorMessage ="La descripción es obligatoria")]
        [StringLength(300)]
        public string Descripcion { get; set; } = "";

        [Column("Precio", TypeName ="decimal(8,2)")]
        [Range(0.01, 999999, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Column("stock")]
        [Range(0, int.MaxValue, ErrorMessage ="El stock no puede ser negativo")]
        public int Stock { get; set; }

        //Foránea
        [ForeignKey(nameof(IdMarca))]
        public Marca? Marca { get; set; }
    }
}
