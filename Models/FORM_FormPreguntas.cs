
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace LODApi.Models
{
    [Table("FORM_FormPreguntas")]
    public class FORM_FormPreguntas
    {
        [Key]
        public string IdPregunta { get; set; }
        
        [ForeignKey("FORM_FormItems")]
        public string IdItem { get; set; }
        public virtual FORM_FormItems FORM_FormItems { get; set; }

        [ForeignKey("FORM_Formularios")]
        public string IdForm { get; set; }
        public virtual FORM_Formularios FORM_Formularios { get; set; }

        [MaxLength(200, ErrorMessage = "Máximo 200 Caracteres")]
        [Required]
        [DisplayName("Nombre Pregunta")]
        public string Titulo { get; set; }

        [MaxLength(500, ErrorMessage = "Máximo 500 Caracteres")]
        [DisplayName("Descripción")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "-")]
        public string Descripcion { get; set; }

        [DisplayName("Tipo Pregunta")]
        [Required]
        public int TipoParam { get; set; }

        [DisplayName("Obligatoria")]
        [Required]
        public bool Obligatoria { get; set; }

        [DisplayName("Indice")]
        public int Indice { get; set; }

        [DisplayName("Tamaño")]
        [Range(1, 10,
        ErrorMessage = "Seleccione entre {1} y {2}.")]
        public int Largo { get; set; }

        public virtual List<FORM_FormAlternativa> FORM_FormAlternativa { get; set; }
        
       
    }
}