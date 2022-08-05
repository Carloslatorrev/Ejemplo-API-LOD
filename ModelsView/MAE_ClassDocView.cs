using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{
    public class MAE_ClassDocView
    {
        public int IdClassDoc { get; set; }

        public int IdClassTwo { get; set; }
        public string ClassTwo { get; set; }

        public int IdTipo { get; set; }
        public string TipoDocumento { get; set; }

        public int? IdTipoSub { get; set; }
        public string SubtipoComunicacion { get; set; }
        public bool EsLiquidacion { get; set; }
    }
}