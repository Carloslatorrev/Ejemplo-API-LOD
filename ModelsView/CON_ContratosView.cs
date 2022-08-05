
using LODApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Areas.GLOD.Models
{
    public class CON_ContratosView
    {
        public int IdContrato { get; set; }
        public string CodigoContrato { get; set; }
        public string NombreContrato { get; set; }

        public string DescripcionContrato { get; set; }
        public string RutaImagenContrato { get; set; }
        public DateTime? FechaCreacionContrato { get; set; }
        public string UserId { get; set; }
        public string UsuarioCreador { get; set; }
        public string UserIdInspectorFiscal { get; set; }
        public string NumeroResolucion { get; set; }
        public int? IdTipoContrato { get; set; }
        public string Sucursal { get; set; }

        public int? IdModalidadContrato { get; set; }

        public decimal? MontoInicialContrato { get; set; }

        public DateTime? FechaInicioContrato { get; set; }

        public string Empresa_Contratista { get; set; }

        public int? ModalidadReajuste { get; set; }
        public decimal? MontoVigenteContrato { get; set; }
        public int? PlazoInicialContrato { get; set; }
        public DateTime? FechaAdjudicacion { get; set; }
        public DateTime? FechaSubcripcion { get; set; }
        public decimal? MontoPresupuestado { get; set; }
        public decimal? MontoModTramite { get; set; }
        public int? PlazoVigente { get; set; }
        public bool? Activo { get; set; }
        public int? EstadoContrato { get; set; }

        public string Empresa_Fiscalizadora { get; set; }
        public string DireccionMOP { get; set; }
        public int? IdDireccionContrato { get; set;}


    }

    public class ContratosSelectView{
        public int Id { get; set; }
        public string Numero { get; set; }

        public string Nombre { get; set; }

        public string EmpContratista { get; set; }

        public string DireccionMOP { get; set; }

        public string RutaImg { get; set; }

    }
}