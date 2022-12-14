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
    public class MAE_SubtipoComunicacionController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        [Route("SubtipoComunicacion/List")]
        [ResponseType(typeof(List<MAE_SubtipoComunicacionView>))]
        public async Task<IHttpActionResult> SubtipoComunicacion()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<MAE_SubtipoComunicacionView> tipos = new List<MAE_SubtipoComunicacionView>();
            var dire = await db.MAE_SubtipoComunicacion.ToListAsync();
            dire.ForEach(
            x => tipos.Add(new MAE_SubtipoComunicacionView()
            {
                IdTipoSub = x.IdTipoSub,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Activo = x.Activo,
                IdTipoCom = x.IdTipoCom,
                TipoComunicacion = x.MAE_TipoComunicacion.Nombre
            }));

            return Ok(tipos);
        }

        /// <summary>
        /// Muestra la información de una ubicación determinada, encontrada por su ID.
        /// </summary>
        /// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("SubtipoComunicacion/Find/{IdTipoSub}")]
        //[ResponseType(typeof(MAE_SubtipoComunicacionView))]
        //public async Task<IHttpActionResult> SubtipoComunicacion(string IdTipoSub)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;

        //    var x = await db.MAE_SubtipoComunicacion.FindAsync(IdTipoSub);
        //    if (x == null)
        //        return BadRequest("El ID de la Ubicación no existe.");

        //    MAE_SubtipoComunicacionView tipo = new MAE_SubtipoComunicacionView()
        //    {
        //        IdTipoSub = x.IdTipoSub,
        //        Nombre = x.Nombre,
        //        Descripcion = x.Descripcion,
        //        Activo = x.Activo,
        //        IdTipoCom = x.IdTipoCom,
        //        TipoComunicacion = x.MAE_TipoComunicacion.Nombre
        //    };

        //    return Ok(tipo);
        //}


        //[HttpGet]
        //[Route("SubtipoComunicacion/FindByTipo/{IdTipoCom}")]
        //[ResponseType(typeof(List<MAE_SubtipoComunicacionView>))]
        //public async Task<IHttpActionResult> SubtipoComunicacion(int IdTipoCom)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;
        //    List<MAE_SubtipoComunicacionView> tipos = new List<MAE_SubtipoComunicacionView>();
        //    var dire = await db.MAE_SubtipoComunicacion.Where(x => x.IdTipoSub == IdTipoCom).ToListAsync();

        //    dire.ForEach(
        //    x => tipos.Add(new MAE_SubtipoComunicacionView()
        //    {
        //        IdTipoSub = x.IdTipoSub,
        //        Nombre = x.Nombre,
        //        Descripcion = x.Descripcion,
        //        Activo = x.Activo,
        //        IdTipoCom = x.IdTipoCom,
        //        TipoComunicacion = x.MAE_TipoComunicacion.Nombre
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
