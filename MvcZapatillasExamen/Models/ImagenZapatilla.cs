using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcZapatillasExamen.Models
{
    [Table("IMAGENESZAPASPRACTICA")]
    public class ImagenZapatilla
    {
        [Key]
        [Column("IDIMAGEN")]
        public int IdImagen { get; set; }

        [Column("IDPRODUCTO")]
        public int IdProdcuto { get; set; }

        [Column("IMAGEN")]
        public string Imagen { get; set; }
    }
}
