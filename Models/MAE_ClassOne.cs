using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Models
{
    [Table("MAE_ClassOne")]
    public class MAE_ClassOne
    {
        [Key]
        public int IdClassOne { get; set; }

        [DisplayName("Nombres")]
        [Required(ErrorMessage = "Dato obligatorio")]
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        [DisplayName("Activo")]
        public bool Activo { get; set; }
        
    }
}