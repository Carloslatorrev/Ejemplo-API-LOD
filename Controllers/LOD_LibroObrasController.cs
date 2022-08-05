using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using LODApi.Areas.GLOD.Models;
using LODApi.Models;

namespace LODApi.Controllers
{
    public class LOD_LibroObrasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("LibroObras/Find/{IdLod}")]
        [ResponseType(typeof(LOD_LibroObrasView))]
        public async Task<IHttpActionResult> LibroObras(string IdLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

           
            var l = await db.LOD_LibroObras.FindAsync(IdLod);
            LOD_LibroObrasView libro = new LOD_LibroObrasView()
            {
                IdLod = l.IdLod,
                IdContrato = l.IdContrato,
                NombreLibroObra = l.NombreLibroObra,
                FechaApertura = l.FechaApertura.Value.ToString(),
                IdTipoLod = l.IdTipoLod,
                TipoLOD = l.MAE_TipoLOD.Nombre,
                UsuarioApertura = l.UsuarioApertura,
                Usuario_Apertura = l.Usuario_Apertura.NombreCompleto,
                UserId = l.UserId,
                Usuario_Creacion = l.Usuario_Creacion.NombreCompleto,
                RutaImagenLObras = l.RutaImagenLObras,
                Estado = l.Estado,
                DescripcionLObra = l.DescripcionLObra,
                OTP = l.OTP,
                TipoApertura = l.TipoApertura,
                CodigoLObras = l.CodigoLObras,
                FechaCierre = l.FechaCierre.ToString(),
                FechaCreacion = l.FechaCreacion,
                HerImgPadre = l.HerImgPadre,
                nombreContrato = l.ContratoNombre,
                UsuarioCierre = l.UsuarioCierre,
                Usuario_Cierre = l.Usuario_Cierre.NombreCompleto
            };

            return Ok(libro);
        }

        [HttpGet]
        [Route("LibroObras/FindByContrato/{IdContrato}")]
        [ResponseType(typeof(List<LOD_LibroObrasView>))]
        public async Task<IHttpActionResult> LibroObrasByContrato(string IdContrato)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            int idAux = Convert.ToInt32(IdContrato);
            var lib = await db.LOD_LibroObras.Where(l => l.IdContrato == idAux && l.Estado == 1).ToListAsync();
            List<LOD_LibroObrasView> libros = new List<LOD_LibroObrasView>();
            foreach (var item in lib)
            {
                if(item.UsuarioCierre == null)
                {
                    ApplicationUser userAux = new ApplicationUser();
                    userAux.Nombres = "-";
                    userAux.Apellidos = "-";
                    item.Usuario_Cierre = userAux;
                    item.UsuarioCierre = "-";
                }
            }      


            lib.ForEach(
                l => libros.Add(new LOD_LibroObrasView()
                {
                    IdLod = l.IdLod,
                    IdContrato = l.IdContrato,
                    NombreLibroObra = l.NombreLibroObra,
                    FechaApertura = l.FechaApertura.Value.ToString(),
                    IdTipoLod = l.IdTipoLod,
                    TipoLOD = l.MAE_TipoLOD.Nombre,
                    UsuarioApertura = l.UsuarioApertura,
                    Usuario_Apertura = l.Usuario_Apertura.NombreCompleto,
                    UserId = l.UserId,
                    Usuario_Creacion = l.Usuario_Creacion.NombreCompleto,
                    RutaImagenLObras = l.RutaImagenLObras,
                    Estado = l.Estado,
                    DescripcionLObra = l.DescripcionLObra,
                    OTP = l.OTP,
                    TipoApertura = l.TipoApertura,
                    CodigoLObras = l.CodigoLObras,
                    FechaCierre = l.FechaCierre.ToString(),
                    FechaCreacion = l.FechaCreacion,
                    HerImgPadre = l.HerImgPadre,
                    nombreContrato = l.ContratoNombre,
                    UsuarioCierre = l.UsuarioCierre,
                    Usuario_Cierre = l.Usuario_Cierre.NombreCompleto
                }
            ));

            return Ok(libros);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("LibroObras/FindLibrosByUser")]
        [ResponseType(typeof(List<LOD_LibroObrasView>))]
        public async Task<IHttpActionResult> LibroObrasByUser(LibroObrasUserView libUser)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<int> IdLods = db.LOD_UsuariosLod.Where(x => x.UserId.Equals(libUser.UserId) && x.LOD_LibroObras.IdContrato == libUser.IdContrato && x.Activo).Select(x => x.IdLod).ToList();

            var lib = await db.LOD_LibroObras.Where(l => IdLods.Contains(l.IdLod) && l.Estado == 1).ToListAsync();
            List<LOD_LibroObrasView> libros = new List<LOD_LibroObrasView>();

            foreach (var item in lib)
            {
                if(item.Usuario_Cierre == null)
                {
                    ApplicationUser userAux = new ApplicationUser();
                    userAux.Nombres = "-";
                    userAux.Apellidos = "";
                    item.Usuario_Cierre = userAux;
                }
            }

            lib.ForEach(
                l => libros.Add(new LOD_LibroObrasView()
                {
                    IdLod = l.IdLod,
                    IdContrato = l.IdContrato,
                    NombreLibroObra = l.NombreLibroObra,
                    FechaApertura = l.FechaApertura.Value.ToString(),
                    IdTipoLod = l.IdTipoLod,
                    TipoLOD = l.MAE_TipoLOD.Nombre,
                    UsuarioApertura = l.UsuarioApertura,
                    Usuario_Apertura = l.Usuario_Apertura.NombreCompleto,
                    UserId = l.UserId,
                    Usuario_Creacion = l.Usuario_Creacion.NombreCompleto,
                    RutaImagenLObras = l.RutaImagenLObras,
                    Estado = l.Estado,
                    DescripcionLObra = l.DescripcionLObra,
                    OTP = l.OTP,
                    TipoApertura = l.TipoApertura,
                    CodigoLObras = l.CodigoLObras,
                    FechaCierre = l.FechaCierre.ToString(),
                    FechaCreacion = l.FechaCreacion,
                    HerImgPadre = l.HerImgPadre,
                    nombreContrato = l.ContratoNombre,
                    UsuarioCierre = l.UsuarioCierre,
                    Usuario_Cierre = l.Usuario_Cierre.NombreCompleto
                    
                }
            ));

            return Ok(libros);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
