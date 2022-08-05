using LODApi.Areas.GLOD.Models;
using LODApi.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{
    public class LOD_RolesCttosContratoView
    {
 
        public int IdRCContrato { get; set; }

        public int IdContrato { get; set; }
        public string NombreContrato { get; set; }

        public int? IdRolCtto { get; set; }
        public string NombreMAERol { get; set; }

        public string NombreRol { get; set; }
        public string Descripcion { get; set; }


    }
}

