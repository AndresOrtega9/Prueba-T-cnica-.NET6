using System.ComponentModel.DataAnnotations;

namespace CATALOGO_DE_PRODUCTOS.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Producto> Productos { get; set; }

        public Categoria()
        {
            Productos = new List<Producto>();
        }
    }
}
