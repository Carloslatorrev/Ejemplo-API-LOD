using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LODApi.ModelsView
{
    public class ApplicationUserView
    {
        public string UserId { get; set; }
        public bool Activo { get; set; }
        public int IdSucursal { get; set; }
        public string Run { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string AnexoEmpresa { get; set; }
        public string CargoContacto { get; set; }
        public string DataLetters { get; set; }
        public string RutaImagen { get; set; }
        public string IdCertificado { get; set; }
        public string Email { get; set; }
    }

    public class ApplicationUserLoginView
    {
        public string Run { get; set; }
        public string Password { get; set; }
    }

    public class ApplicationUserPostLoginView
    {
        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}