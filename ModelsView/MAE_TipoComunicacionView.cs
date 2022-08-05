using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Models
{
 
    public class MAE_TipoComunicacionView
    {
        public int IdTipoCom { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public string TipoLibro { get; set; }
        public int IdTipoLod { get; set; }
        
    }
}