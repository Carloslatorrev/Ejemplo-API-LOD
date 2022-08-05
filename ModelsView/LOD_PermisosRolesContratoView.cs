using LODApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{

    public class LOD_PermisosRolesContratoView
    {

        public int IdPermiso { get; set; }

        public int? IdRCContrato { get; set; }
        public string NombreRol { get; set; }

        public int IdLod { get; set; }
        public string  NombreLibroObras { get; set; }

        public bool Lectura { get; set; }
        public bool Escritura { get; set; }
        public bool FirmaGob { get; set; }
        public bool FirmaFea { get; set; }
        public bool FirmaSimple { get; set; }

    }
}


