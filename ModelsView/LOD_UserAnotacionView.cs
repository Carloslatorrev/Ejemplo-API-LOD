using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LODApi.Models;

namespace LODApi.Models
{
    public class LOD_UserAnotacionView
    {
        public int IdUsLod { get; set; }
        public string UsuarioLod { get; set; }

        public int IdAnotacion { get; set; }
        public string Anotacion { get; set; }

        public bool Destacado { get; set; }
        public string TempCode { get; set; }
        public bool Leido { get; set; }

        public bool EsPrincipal { get; set; }

        public bool EsRespRespuesta { get; set; }

        public bool RespVB { get; set; }

        public bool VistoBueno { get; set; }

        public DateTime? FechaVB { get; set; }

        public int? TipoVB { get; set; }
        public string RutaImg { get; set; }
    }

    public class ReceptoresView
    {
        public int IdUsLod { get; set; }

        public int IdAnotacion { get; set; }

        public bool EsPrincipal { get; set; }

        public bool EsRespRespuesta { get; set; }


    }

    public class UserBorradorView
    {
        public int IdUsLod { get; set; }

        public int IdAnotacion { get; set; }

    }


    public class VistoBuenoView
    {
        public int IdUsLod { get; set; }
        public bool RespVB { get; set; }
        public int IdAnotacion { get; set; }
        public string NombreCompleto { get; set; }
        public bool VistoBueno { get; set; }
        public int? TipoVB { get; set; }

    }

}


