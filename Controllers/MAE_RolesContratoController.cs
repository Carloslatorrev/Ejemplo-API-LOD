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

namespace LODApi.Controllers
{
    public class MAE_RolesContratoController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //[HttpGet]
        //[Route("RolesContrato/List")]
        //[ResponseType(typeof(List<MAE_RolesContratoView>))]
        //public async Task<IHttpActionResult> RolesContrato()
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;

        //    List<MAE_RolesContratoView> roles = new List<MAE_RolesContratoView>();
        //    var dire = await db.MAE_SubtipoComunicacion.ToListAsync();
        //    dire.ForEach(
        //    x => roles.Add(new MAE_RolesContratoView()
        //    {
                
        //    }));

        //    return Ok(roles);
        //}

        ///// <summary>
        ///// Muestra la información de una ubicación determinada, encontrada por su ID.
        ///// </summary>
        ///// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("RolesContrato/Find/{IdRol}")]
        //[ResponseType(typeof(MAE_RolesContratoView))]
        //public async Task<IHttpActionResult> RolesContrato(string IdTipoSub)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;

        //    var x = await db.MAE_SubtipoComunicacion.FindAsync(IdTipoSub);
        //    if (x == null)
        //        return BadRequest("El ID de la Ubicación no existe.");

        //    MAE_RolesContratoView tipo = new MAE_RolesContratoView()
        //    {

        //    };

        //    return Ok(tipo);
        //}


        //[HttpGet]
        //[Route("RolesContrato/FindByTipo/{IdRol}")]
        //[ResponseType(typeof(MAE_RolesContratoView))]
        //public async Task<IHttpActionResult> RolesContrato(int IdTipoCom)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;
        //    List<MAE_RolesContratoView> tipos = new List<MAE_RolesContratoView>();
        //    var dire = await db.MAE_SubtipoComunicacion.Where(x => x.IdTipoSub == IdTipoCom).ToListAsync();

        //    dire.ForEach(
        //    x => tipos.Add(new MAE_RolesContratoView()
        //    {
        //    }));

        //    return Ok(tipos);
        //}

        

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
