using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CATALOGO_DE_PRODUCTOS.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Imagen { get; set; }
        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; }
    }
}
