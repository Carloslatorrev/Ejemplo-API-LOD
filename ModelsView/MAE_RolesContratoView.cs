using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LODApi.Models
{
    public class MAE_RolesContratoView
    {

        public int IdRolCtto { get; set; }

        public string NombreRol { get; set; }

        public string Descripcion { get; set; }

        public bool Activo { get; set; } 

        public bool EsGubernamental { get; set; }
        public bool EsFiscalizador { get; set; }
        public bool EsContratista { get; set; }

        public int IdTipoLod { get; set; }
        public string TipoLibro { get; set; }
        public bool Lectura { get; set; }
        public bool Escritura { get; set; }
        public bool FirmaGob { get; set; }
        public bool FirmaFea { get; set; }
        public bool FirmaSimple { get; set; }

        //Opciones del Libro Obra Mestro
        public bool Lectura1 { get; set; }
        public bool Escritura1 { get; set; }
        public bool FirmaGob1 { get; set; }
        public bool FirmaFea1 { get; set; }
        public bool FirmaSimple1 { get; set; }

        //Opciones del Libro Comunicaciones
        public bool Lectura2 { get; set; }
        public bool Escritura2 { get; set; }
        public bool FirmaGob2 { get; set; }
        public bool FirmaFea2 { get; set; }
        public bool FirmaSimple2 { get; set; }

        //Opciones de Libros de Comunicaciones
        public bool Lectura3 { get; set; }
        public bool Escritura3 { get; set; }
        public bool FirmaGob3 { get; set; }
        public bool FirmaFea3 { get; set; }
        public bool FirmaSimple3 { get; set; }

        //Opciones de Libros de Auxiliares o complementarios
        public bool Lectura4 { get; set; }
        public bool Escritura4 { get; set; }
        public bool FirmaGob4 { get; set; }
        public bool FirmaFea4 { get; set; }
        public bool FirmaSimple4 { get; set; }


        //Definición Deacuerdo al ROL

        public bool RolGubernamental { get; set; }
        public bool RolFiscalizador { get; set; }
        public bool RolContratista { get; set; }

        public int Jerarquia { get; set; }

    }
}
