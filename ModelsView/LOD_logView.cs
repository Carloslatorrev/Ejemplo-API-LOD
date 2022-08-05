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

    public class LOD_logView
    {

        public int IdLog { get; set; }
        public string Objeto { get; set; }
        [DisplayName("Anotación")]
        public int IdObjeto { get; set; }
        public DateTime FechaLog { get; set; }

        
        public string UserId { get; set; }
        [DisplayName("Usuario")]
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Campo { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorActualizado { get; set; }

    }
}