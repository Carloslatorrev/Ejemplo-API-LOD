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
    public class LOD_RolesCttosContratoController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        [Route("RolesCttosContrato/List")]
        [ResponseType(typeof(List<LOD_RolesCttosContratoView>))]
        public async Task<IHttpActionResult> RolesCttosContrato()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<LOD_RolesCttosContratoView> roles = new List<LOD_RolesCttosContratoView>();
            var dire = await db.LOD_RolesCttosContrato.ToListAsync();
            dire.ForEach(
            x => roles.Add(new LOD_RolesCttosContratoView()
            {
                IdRCContrato = x.IdRCContrato,
                IdContrato = x.IdContrato,
                NombreContrato = x.CON_Contratos.CodigoContrato + "-" + x.CON_Contratos.NombreContrato,
                IdRolCtto = x.IdRolCtto,
                NombreMAERol = x.MAE_RolesContrato.NombreRol,
                NombreRol = x.NombreRol,
                Descripcion = x.Descripcion
            }));

            return Ok(roles);
        }


        [HttpGet]
        [Route("RolesContrato/Find/{IdRCContrato}")]
        [ResponseType(typeof(LOD_RolesCttosContratoView))]
        public async Task<IHttpActionResult> RolesCttosContrato(string IdRCContrato)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var x = await db.LOD_RolesCttosContrato.FindAsync(IdRCContrato);
            if (x == null)
                return BadRequest("El ID de la Ubicación no existe.");

            LOD_RolesCttosContratoView tipo = new LOD_RolesCttosContratoView()
            {
                IdRCContrato = x.IdRCContrato,
                IdContrato = x.IdContrato,
                NombreContrato = x.CON_Contratos.CodigoContrato + "-" + x.CON_Contratos.NombreContrato,
                IdRolCtto = x.IdRolCtto,
                NombreMAERol = x.MAE_RolesContrato.NombreRol,
                NombreRol = x.NombreRol,
                Descripcion = x.Descripcion
            };

            return Ok(tipo);
        }


        [HttpGet]
        [Route("RolesContrato/FindByContrato/{IdContrato}")]
        [ResponseType(typeof(LOD_RolesCttosContratoView))]
        public async Task<IHttpActionResult> RolesCttosContrato(int IdContrato)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<LOD_RolesCttosContratoView> tipos = new List<LOD_RolesCttosContratoView>();
            var dire = await db.LOD_RolesCttosContrato.Where(x => x.IdContrato == IdContrato).ToListAsync();

            dire.ForEach(
            x => tipos.Add(new LOD_RolesCttosContratoView()
            {
                IdRCContrato = x.IdRCContrato,
                IdContrato = x.IdContrato,
                NombreContrato = x.CON_Contratos.CodigoContrato + "-" + x.CON_Contratos.NombreContrato,
                IdRolCtto = x.IdRolCtto,
                NombreMAERol = x.MAE_RolesContrato.NombreRol,
                NombreRol = x.NombreRol,
                Descripcion = x.Descripcion
            }));

            return Ok(tipos);
        }

        [HttpGet]
        [Route("RolesContrato/FindByUsuario/{UserId}")]
        [ResponseType(typeof(LOD_RolesCttosContratoView))]
        public async Task<IHttpActionResult> RolesCttosContratoByUser(string UserId)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa; 
            
            List<LOD_RolesCttosContratoView> tipos = new List<LOD_RolesCttosContratoView>();

            var dire = await db.LOD_UsuariosLod.Where(x => x.UserId == UserId && x.Activo).Select(x => x.LOD_RolesCttosContrato).ToListAsync();

            dire.ForEach(
            x => tipos.Add(new LOD_RolesCttosContratoView()
            {
                IdRCContrato = x.IdRCContrato,
                IdContrato = x.IdContrato,
                NombreContrato = x.CON_Contratos.CodigoContrato + "-" + x.CON_Contratos.NombreContrato,
                IdRolCtto = x.IdRolCtto,
                NombreMAERol = x.MAE_RolesContrato.NombreRol,
                NombreRol = x.NombreRol,
                Descripcion = x.Descripcion
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
