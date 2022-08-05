using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LODApi.Models;

namespace LODApi.Models
{
    public class LOD_ReferenciasAnotView
    {
 
        public int IdRefAnot { get; set; }
        public int IdAnotacion { get; set; }
        public string AnotacionOrigen { get; set; }

        public int IdAnontacionRef { get; set; }
        public string AnotacionReferencia { get; set; }

        public bool EsRepuesta { get; set; }

        public string Observacion { get; set; }
    }
}