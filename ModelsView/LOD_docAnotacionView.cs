using LODApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Models
{

    public class LOD_docAnotacionView
    {

        public int IdDocAnotacion { get; set; }

        
        public int IdDoc { get; set; }
        [DisplayName("Documentos")]
        public string documento { get; set; }

        
        public int IdAnotacion { get; set; }
        [DisplayName("Anotación")]
        public string anotacion { get; set; }

       
        public int IdTipoDoc { get; set; }
        [DisplayName("Tipo de Documento")]
        public string TipoDocumento { get; set; }

        public int IdContrato { get; set; }
        /*
         0 = pendiente
         1 = Aprobado
         2 = Rechazado
         */
        public int EstadoDoc { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        public DateTime? FechaEvento { get; set; }

       
        public string IdUserEvento { get; set; }
        public string UsuarioEvento { get; set; }

        public string ruta { get; set; }

    }

    public class CreateDocAnotacion
    {
        public int IdDoc { get; set; }

        public int IdAnotacion { get; set; }
        public int IdTipoDoc { get; set; }

        public int IdContrato { get; set; }

        public string Observaciones { get; set; }

    }

    public class CreateDoc
    {
        public int IdDoc { get; set; }

        public int IdAnotacion { get; set; }
        public int IdTipoDoc { get; set; }

        public int IdContrato { get; set; }

        public string Observaciones { get; set; }

    }
}