using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{
    public class MAE_CodSubComView
    {

        public int IdControl { get; set; }

        public int IdTipoSub{ get; set; }
        public string SubtipoComunicacion { get; set; }


        public int IdTipo { get; set; }

        public string TipoDocumento { get; set; }

        public bool Activo { get; set; }
        public bool Obligatorio { get; set; }

    }
}
