using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.ModelsViews
{
    public class AddDocumentoView
    {
        public int IdAnotacion { get; set; }
        public int IdTipoDoc { get; set; }
        public int IdContrato { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string UserId { get; set; }

        //public string file { get; set; }
    }

    public class AddOtroDocumentoView
    {
        public int IdAnotacion { get; set; }
        public int IdTipoDoc { get; set; }
        public int IdClassTwo { get; set; }
        public int IdContrato { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string UserId { get; set; }

        //public string file { get; set; }
    }
}