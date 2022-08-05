using LODApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace LODApi.Models
{

    public class LOD_AnotacionesView
    {
        public int IdAnotacion { get; set; }

        public string Titulo { get; set; }

        public int IdLod { get; set; }
        public string LibroObras { get; set; }

        public int IdTipoSub { get; set; }
        public string SubtipoComunicacion { get; set; }

        public int Correlativo { get; set; }

        public bool EsEstructurada { get; set; }

        public string Cuerpo { get; set; }

        public int Estado { get; set; }

        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaPub { get; set; }
        public DateTime? FechaFirma { get; set; }

        public bool SolicitudRest { get; set; }
        public DateTime? FechaResp { get; set; }
        public DateTime? FechaTopeRespuesta { get; set; }


        public string UserId { get; set; }
        public string UsuarioRemitente { get; set; }

        public bool SolicitudVB { get; set; }
        public int TipoFirma { get; set; }
        public bool EstadoFirma { get; set; }


        public string UserIdBorrador { get; set; }
        public string UsuarioBorrador { get; set; }
        public string TempCode { get; set; }

        public string RutaPdfSinFirma { get; set; }
        public string RutaPdfConFirma { get; set; }

        //public virtual ICollection<LOD_UserAnotacionView> vistoBueno {get; set;}

        //public virtual ICollection<LOD_docAnotacionView> Documentos { get; set; }

        //public virtual ICollection<LOD_UserAnotacionView> Receptores { get; set; }

        //public virtual ICollection<LOD_ReferenciasAnotView> Referencias { get; set; }

        //public virtual ICollection<LOD_logView> Logs { get; set; }

    }

    public class AnotacionUserView
    {
        public string UserID { get; set; }
        public int IdLod { get; set; }
    }

    public class CreateAnotacion
    {
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public string UserId { get; set; }
        public int IdSubTipo { get; set; }
        public int IdLod { get; set; }

        public bool SolicitudResp { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public bool SolicitudVB { get; set; }

    }

    public class UpdateAnotacion
    {
        public int IdAnotacion { get; set; }
        public string titulo { get; set; }
        public string cuerpo { get; set; }
        public int IdSubTipo { get; set; }
        public int IdLod { get; set; }

        public bool SolicitudResp { get; set; }
        public DateTime? FechaSolicitud { get; set; }

    }

    public class FirmarAnotacionView
    {
        public string password { get; set; }
        public int IdAnotacion { get; set; }
        public string UserId { get; set; }
    }

    public class VBConfirmedView
    {
        public int IdUsLod { get; set; }
        public int IdAnotacion { get; set; }
    }
    public class SolicitudFirma
    {
        public int IdAnotacion { get; set; }

        public int IdFirmante { get; set; }
    }

    public class TomaConocimiento
    {
        public int IdAnotacion { get; set; }

        public string password { get; set; }

        public int tipo { get; set; }

        public string UserId { get; set; }
    }

    public class CreateReferencia
    {
        public int IdAnontacionRef { get; set; }
        public int IdAnotacion { get; set; }
        public bool EsRepuesta { get; set; }
        public string Observacion {get; set;}
    }

}

