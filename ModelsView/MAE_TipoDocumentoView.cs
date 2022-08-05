using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LODApi.Models
{
    public class MAE_TipoDocumentoView
    {
        public int IdTipo { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int TipoClasi { get; set; }
        /*
         1 = Formulario
         2 = Documento técnico
         3 = Documento administrativo
         4 = Otros
         */

    }
}