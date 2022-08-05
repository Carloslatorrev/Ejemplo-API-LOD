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
    public class MAE_CodSubComController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("CodSubCom/List")]
        [ResponseType(typeof(List<MAE_CodSubComView>))]
        public async Task<IHttpActionResult> CodSubCom()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<MAE_CodSubComView> tipos = new List<MAE_CodSubComView>();
            var dire = await db.MAE_CodSubCom.ToListAsync();
            dire.ForEach(
            x => tipos.Add(new MAE_CodSubComView()
            {
                IdControl = x.IdControl,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdTipo = x.IdTipo,
                TipoDocumento = x.MAE_TipoDocumento.Tipo,
                Activo = x.Activo,
                Obligatorio = x.Obligatorio

            }));

            return Ok(tipos);
        }

        /// <summary>
        /// Muestra la información de una ubicación determinada, encontrada por su ID.
        /// </summary>
        /// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("CodSubCom/Find/{IdControl}")]
        [ResponseType(typeof(MAE_CodSubComView))]
        public async Task<IHttpActionResult> CodSubCom(string IdControl)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var x = await db.MAE_CodSubCom.FindAsync(IdControl);
            if (x == null)
                return BadRequest("El ID del control no existe.");

            MAE_CodSubComView tipo = new MAE_CodSubComView()
            {
                IdControl = x.IdControl,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdTipo = x.IdTipo,
                TipoDocumento = x.MAE_TipoDocumento.Tipo,
                Activo = x.Activo,
                Obligatorio = x.Obligatorio
            };

            return Ok(tipo);
        }


        [HttpGet]
        [Route("CodSubCom/FindBySubtipo/{IdSubtipo}")]
        [ResponseType(typeof(List<MAE_CodSubComView>))]
        public async Task<IHttpActionResult> CodSubCom(int IdSubtipo)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<MAE_CodSubComView> tipos = new List<MAE_CodSubComView>();
            var dire = await db.MAE_CodSubCom.Where(x => x.IdTipoSub == IdSubtipo).ToListAsync();

            dire.ForEach(
            x => tipos.Add(new MAE_CodSubComView()
            {
                IdControl = x.IdControl,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdTipo = x.IdTipo,
                TipoDocumento = x.MAE_TipoDocumento.Tipo,
                Activo = x.Activo,
                Obligatorio = x.Obligatorio
            }));

            return Ok(tipos);
        }

        [HttpGet]
        [Route("CodSubCom/FindByTipoLod/{IdTipoLod}")]
        [ResponseType(typeof(List<MAE_CodSubComView>))]
        public async Task<IHttpActionResult> CodSubComByTipo(int IdTipoLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<MAE_CodSubComView> tipos = new List<MAE_CodSubComView>();
            var dire = await db.MAE_CodSubCom.Where(x => x.MAE_SubtipoComunicacion.MAE_TipoComunicacion.IdTipoLod == IdTipoLod).ToListAsync();

            dire.ForEach(
            x => tipos.Add(new MAE_CodSubComView()
            {
                IdControl = x.IdControl,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdTipo = x.IdTipo,
                TipoDocumento = x.MAE_TipoDocumento.Tipo,
                Activo = x.Activo,
                Obligatorio = x.Obligatorio
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
