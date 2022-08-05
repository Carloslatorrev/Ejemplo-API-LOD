
using LODApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Models
{
    public class LOD_UsuariosLodView
    {
     
        public int IdUsLod { get; set; }
        public int? IdRCContrato { get; set; }

        public string rol { get; set; }

        public int IdLod { get; set; }

        public string Libro { get; set; }

        public string UserId { get; set; }

        public string Usuario { get; set; }


        public bool Activo { get; set; }

        public DateTime FechaActivacion { get; set; }

        public Nullable<DateTime> FechaDesactivacion { get; set; }
        public string RutaImagen { get; set; }
        public string telefono { get; set; }


    }

   
}


