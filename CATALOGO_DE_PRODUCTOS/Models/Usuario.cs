using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CATALOGO_DE_PRODUCTOS.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmarPassword { get; set; }
    }
}
