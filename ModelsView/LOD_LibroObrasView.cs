using LODApi.Areas.GLOD.Models;
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
    public class LOD_LibroObrasView
    {
        public int IdLod { get; set; }
        public int IdContrato { get; set; }

        public string nombreContrato { get; set; }

        public string NombreLibroObra { get; set; }

        public string CodigoLObras { get; set; }
        public string DescripcionLObra { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UserId { get; set; }
        public string Usuario_Creacion { get; set; }

        public int IdTipoLod { get; set; }

        public string TipoLOD { get; set; }

        public int? Estado { get; set; }

        public string FechaCierre { get; set; }

        public string UsuarioApertura { get; set; }

        public string Usuario_Apertura { get; set; }

        public string UsuarioCierre { get; set; }
        public string Usuario_Cierre { get; set; }

        public string FechaApertura { get; set; }
        public bool TipoApertura { get; set; }

        public string RutaImagenLObras { get; set; }
        public string OTP { get; set; }

        public bool HerImgPadre { get; set; }

        //public virtual ICollection<LOD_AnotacionesView> Anotaciones { get; set; }

    }

    public class LibroObrasUserView
    {
        public int IdContrato { get; set; }
        public string UserId { get; set; }
    }
}