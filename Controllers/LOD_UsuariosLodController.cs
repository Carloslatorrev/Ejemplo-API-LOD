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
using LODApi.ModelsViews;

namespace LODApi.Controllers
{
    public class LOD_UsuariosLodController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        //[HttpGet]
        //[Route("UsuariosLod/List")]
        //[ResponseType(typeof(List<MAE_CodSubComView>))]
        //public async Task<IHttpActionResult> CodSubCom()
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;

        //    List<MAE_CodSubComView> tipos = new List<MAE_CodSubComView>();
        //    var dire = await db.MAE_CodSubCom.ToListAsync();
        //    dire.ForEach(
        //    x => tipos.Add(new MAE_CodSubComView()
        //    {
        //        IdControl = x.IdControl,
        //        IdTipoSub = x.IdTipoSub,
        //        SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
        //        IdTipo = x.IdTipo,
        //        TipoDocumento = x.MAE_TipoDocumento.Tipo,
        //        Activo = x.Activo,
        //        Obligatorio = x.Obligatorio

        //    }));

        //    return Ok(tipos);
        //}


        [HttpGet]
        [Route("UsuariosLod/Find/{IdUsLod}")]
        [ResponseType(typeof(LOD_UsuariosLodView))]
        public async Task<IHttpActionResult> UsuariosLod(string IdUsLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            int id = Convert.ToInt32(IdUsLod);
            var x = await db.LOD_UsuariosLod.FindAsync(id);
            if (x == null)
                return BadRequest("El ID del UsuarioLod no existe.");

            LOD_UsuariosLodView tipo = new LOD_UsuariosLodView()
            {
                IdUsLod = x.IdUsLod,
                IdRCContrato = x.IdRCContrato,
                rol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.LOD_LibroObras.IdLod,
                Libro = x.LOD_LibroObras.NombreLibroObra,
                UserId = x.UserId,
                Usuario = x.ApplicationUser.NombreCompleto,
                Activo = x.Activo,
                FechaActivacion = x.FechaActivacion,
                FechaDesactivacion = x.FechaDesactivacion,
                RutaImagen = x.ApplicationUser.RutaImagen,
                telefono = x.ApplicationUser.Telefono
            };

            return Ok(tipo);
        }

        [HttpGet]
        [Route("UsuariosLod/FindByUser/{UserId}")]
        [ResponseType(typeof(LOD_UsuariosLodView))]
        public async Task<IHttpActionResult> UsuariosLodByUser(string UserId)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var dire = await db.LOD_UsuariosLod.Where(ul => ul.UserId == UserId && ul.Activo).ToListAsync();

            List<LOD_UsuariosLodView> tipos = new List<LOD_UsuariosLodView>();

            dire.ForEach(
            x => tipos.Add(new LOD_UsuariosLodView()
            {
                IdUsLod = x.IdUsLod,
                IdRCContrato = x.IdRCContrato,
                rol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.LOD_LibroObras.IdLod,
                Libro = x.LOD_LibroObras.NombreLibroObra,
                UserId = x.UserId,
                Usuario = x.ApplicationUser.NombreCompleto,
                Activo = x.Activo,
                FechaActivacion = x.FechaActivacion,
                FechaDesactivacion = x.FechaDesactivacion,
                RutaImagen = x.ApplicationUser.RutaImagen,
                telefono = x.ApplicationUser.Telefono


            }));

            return Ok(tipos);
        }

        [HttpGet]
        [Route("UsuariosLod/FindByLod/{IdLod}")]
        [ResponseType(typeof(List<LOD_UsuariosLodView>))]
        public async Task<IHttpActionResult> UsuariosLod(int IdLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var dire = await db.LOD_UsuariosLod.Where(x => x.IdLod == IdLod && x.Activo).ToListAsync();

            List<LOD_UsuariosLodView> tipos = new List<LOD_UsuariosLodView>();

            dire.ForEach(
            x => tipos.Add(new LOD_UsuariosLodView()
            {
                IdUsLod = x.IdUsLod,
                IdRCContrato = x.IdRCContrato,
                rol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.LOD_LibroObras.IdLod,
                Libro = x.LOD_LibroObras.NombreLibroObra,
                UserId = x.UserId,
                Usuario = x.ApplicationUser.NombreCompleto,
                Activo = x.Activo,
                FechaActivacion = x.FechaActivacion,
                FechaDesactivacion = x.FechaDesactivacion,
                RutaImagen = x.ApplicationUser.RutaImagen,
                telefono = x.ApplicationUser.Telefono

            }));

            return Ok(tipos);

        }

        [HttpGet]
        [Route("UsuariosLod/FindByContrato/{IdContrato}")]
        [ResponseType(typeof(List<LOD_UsuariosLodView>))]
        public async Task<IHttpActionResult> UsuariosLodByContrato(int IdContrato)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<int> IdLods = await db.LOD_LibroObras.Where(u => u.IdContrato == IdContrato).Select(x => x.IdLod).ToListAsync();

            //var direAux = await db.LOD_UsuariosLod.Where(l => IdLods.Contains(l.IdLod) && l.Activo).Select(x => x.UserId).Distinct().ToListAsync();
            var direAux = await db.LOD_UsuariosLod.Where(l => IdLods.Contains(l.IdLod) && l.Activo).ToListAsync();

            List<LOD_UsuariosLodView> tipos = new List<LOD_UsuariosLodView>();

            direAux.ForEach(
            x => tipos.Add(new LOD_UsuariosLodView()
            {
                IdUsLod = x.IdUsLod,
                IdRCContrato = x.IdRCContrato,
                rol = x.LOD_RolesCttosContrato.NombreRol,
                IdLod = x.LOD_LibroObras.IdLod,
                Libro = x.LOD_LibroObras.NombreLibroObra,
                UserId = x.UserId,
                Usuario = x.ApplicationUser.NombreCompleto,
                Activo = x.Activo,
                FechaActivacion = x.FechaActivacion,
                FechaDesactivacion = x.FechaDesactivacion,
                RutaImagen = x.ApplicationUser.RutaImagen,
                telefono = x.ApplicationUser.Telefono

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
