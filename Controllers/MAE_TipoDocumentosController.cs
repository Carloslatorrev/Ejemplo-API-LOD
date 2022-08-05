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
    public class MAE_TipoDocumentosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        [Route("TipoDocumento/List")]
        [ResponseType(typeof(List<MAE_TipoDocumentoView>))]
        public async Task<IHttpActionResult> TipoDocumento()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<MAE_TipoDocumentoView> docs = new List<MAE_TipoDocumentoView>();
            var dire = await db.MAE_TipoDocumento.ToListAsync();
            dire.ForEach(
            x => docs.Add(new MAE_TipoDocumentoView()
            {
               
               IdTipo = x.IdTipo,
               Activo = x.Activo,
               Descripcion = x.Descripcion,
               Tipo = x.Tipo,
               TipoClasi = x.TipoClasi
            }));



            return Ok(docs);
        }

        /// <summary>
        /// Muestra la información de una ubicación determinada, encontrada por su ID.
        /// </summary>
        /// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("TipoDocumento/Find/{IdTipo}")]
        //[ResponseType(typeof(MAE_TipoDocumentoView))]
        //public async Task<IHttpActionResult> TipoDocumento(string IdTipo)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;

        //    var x = await db.MAE_TipoDocumento.FindAsync(IdTipo);
        //    if (x == null)
        //        return BadRequest("El ID de la Ubicación no existe.");

        //    MAE_TipoDocumento tipo = new MAE_TipoDocumento()
        //    {
        //        IdTipo = x.IdTipo,
        //        Activo = x.Activo,
        //        Descripcion = x.Descripcion,
        //        Tipo = x.Tipo,
        //        TipoClasi = x.TipoClasi
        //    };

        //    return Ok(tipo);
        //}


        //[HttpGet]
        //[Route("TipoDocumento/FindBySubtipo/{IdSubtipo}")]
        //[ResponseType(typeof(MAE_TipoDocumentoView))]
        //public async Task<IHttpActionResult> TipoDocumento(int IdSubtipo)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;
        //    List<MAE_TipoDocumentoView> docs = new List<MAE_TipoDocumentoView>();
        //    var dire = await db.MAE_CodSubCom.Where(x => x.IdTipoSub == IdSubtipo).ToListAsync();
        //    dire.ForEach(
        //    x => docs.Add(new MAE_TipoDocumentoView()
        //    {

        //        IdTipo = x.MAE_TipoDocumento.IdTipo,
        //        Activo = x.MAE_TipoDocumento.Activo,
        //        Descripcion = x.MAE_TipoDocumento.Descripcion,
        //        Tipo = x.MAE_TipoDocumento.Tipo,
        //        TipoClasi = x.MAE_TipoDocumento.TipoClasi
        //    }));

        //    return Ok(docs);
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
