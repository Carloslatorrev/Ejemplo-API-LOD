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
using LODApi.Models;
using LODApi.ModelsView;

namespace LODApi.Controllers
{
    public class LOD_PermisosRolesContratoController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("PermisosRolesContrato/List")]
        [ResponseType(typeof(List<LOD_PermisosRolesContratoView>))]
        public async Task<IHttpActionResult> PermisosRolesContrato()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<LOD_PermisosRolesContratoView> roles = new List<LOD_PermisosRolesContratoView>();
            var dire = await db.LOD_PermisosRolesContrato.ToListAsync();
            dire.ForEach(
            x => roles.Add(new LOD_PermisosRolesContratoView()
            {
                IdPermiso = x.IdPermiso,
                IdRCContrato = x.IdRCContrato,
                NombreRol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.IdLod,
                NombreLibroObras = x.LOD_LibroObras.NombreLibroObra,
                Lectura = x.Lectura,
                Escritura = x.Escritura,
                FirmaGob = x.FirmaGob,
                FirmaFea = x.FirmaFea,
                FirmaSimple = x.FirmaSimple
            }));

            return Ok(roles);
        }


        [HttpGet]
        [Route("PermisosRolesContrato/Find/{IdPermiso}")]
        [ResponseType(typeof(LOD_PermisosRolesContratoView))]
        public async Task<IHttpActionResult> PermisosRolesContrato(string IdPermiso)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var x = await db.LOD_PermisosRolesContrato.FindAsync(IdPermiso);
            if (x == null)
                return BadRequest("El ID de la Ubicación no existe.");

            LOD_PermisosRolesContratoView tipo = new LOD_PermisosRolesContratoView()
            {
                IdPermiso = x.IdPermiso,
                IdRCContrato = x.IdRCContrato,
                NombreRol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.IdLod,
                NombreLibroObras = x.LOD_LibroObras.NombreLibroObra,
                Lectura = x.Lectura,
                Escritura = x.Escritura,
                FirmaGob = x.FirmaGob,
                FirmaFea = x.FirmaFea,
                FirmaSimple = x.FirmaSimple
            };

            return Ok(tipo);
        }


        [HttpGet]
        [Route("PermisosRolesContrato/FindByLod/{IdLod}")]
        [ResponseType(typeof(LOD_PermisosRolesContratoView))]
        public async Task<IHttpActionResult> PermisosRolesContrato(int IdLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<LOD_PermisosRolesContratoView> tipos = new List<LOD_PermisosRolesContratoView>();
            var dire = await db.LOD_PermisosRolesContrato.Where(x => x.IdLod == IdLod).ToListAsync();

            dire.ForEach(
            x => tipos.Add(new LOD_PermisosRolesContratoView()
            {
                IdPermiso = x.IdPermiso,
                IdRCContrato = x.IdRCContrato,
                NombreRol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.IdLod,
                NombreLibroObras = x.LOD_LibroObras.NombreLibroObra,
                Lectura = x.Lectura,
                Escritura = x.Escritura,
                FirmaGob = x.FirmaGob,
                FirmaFea = x.FirmaFea,
                FirmaSimple = x.FirmaSimple
            }));

            return Ok(tipos);
        }

        [HttpGet]
        [Route("PermisosRolesContrato/FindByUsuario/{UserId}")]
        [ResponseType(typeof(LOD_PermisosRolesContratoView))]
        public async Task<IHttpActionResult> PermisosRolesContratoByUser(string UserId)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa; 
            
            List<LOD_PermisosRolesContratoView> tipos = new List<LOD_PermisosRolesContratoView>();

            var dire = await db.LOD_UsuariosLod.Where(x => x.UserId == UserId && x.Activo).Select(x => x.IdRCContrato).ToListAsync();
            List<LOD_PermisosRolesContrato> list = await db.LOD_PermisosRolesContrato.Where(x => dire.Contains(x.IdRCContrato)).ToListAsync();

            list.ForEach(
            x => tipos.Add(new LOD_PermisosRolesContratoView()
            {
                IdPermiso = x.IdPermiso,
                IdRCContrato = x.IdRCContrato,
                NombreRol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.IdLod,
                NombreLibroObras = x.LOD_LibroObras.NombreLibroObra,
                Lectura = x.Lectura,
                Escritura = x.Escritura,
                FirmaGob = x.FirmaGob,
                FirmaFea = x.FirmaFea,
                FirmaSimple = x.FirmaSimple
            }));

            return Ok(tipos);
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
