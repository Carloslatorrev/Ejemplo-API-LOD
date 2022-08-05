using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LODApi.Models
{
    [Table("MAE_region")]
    public class MAE_region
    {
        [Key]
        public int IdRegion { get; set; }
        public string Pais { get; set; }
        [Required(ErrorMessage = "Dato obligatorio")]
        public string Region { get; set; }
    }
}