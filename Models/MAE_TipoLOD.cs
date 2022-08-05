﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Models
{
    [Table("MAE_TipoLOD")]
    public class MAE_TipoLOD
    {
        [Key]
        public int IdTipoLod { get; set; }

        [DisplayName("Nombres")]
        [Required(ErrorMessage = "Dato obligatorio")]
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        [DisplayName("Activo")]
        public bool Activo { get; set; }
        public int TipoLodJer { get; set; }
        public bool EsObligatorio { get; set; }
        public string Color { get; set; }
    }
}