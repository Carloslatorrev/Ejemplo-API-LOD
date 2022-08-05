using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{

    public class MAE_ClassTwoView
    {
        public int IdClassTwo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public int IdClassOne { get; set; }
        public string ClassOne { get; set; }
    }
}