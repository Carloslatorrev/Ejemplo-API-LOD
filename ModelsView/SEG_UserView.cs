using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{
    public class SEG_UserView
    {

        public string UserId { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Apellidos")]
        public string apellido { get; set; }
    }
}