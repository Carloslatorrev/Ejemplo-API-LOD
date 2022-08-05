
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LODApi.Models
{
    [Table("MAE_ciudad")]
    public class MAE_ciudad
    {
        [Key]
		[Display(Name = "Ciudad")]
		public int IdCiudad { get; set; }
        [Required(ErrorMessage = "Dato obligatorio")]
        public string Ciudad { get; set; }
        public string ZonaHoraria { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        [ForeignKey("MAE_region")]
        public int? IdRegion { get; set; }
        public virtual MAE_region MAE_region { get; set; }


    }
}