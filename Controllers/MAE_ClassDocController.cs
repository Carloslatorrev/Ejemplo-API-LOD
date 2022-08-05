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
    public class MAE_ClassDocController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("ClassDoc/List")]
        [ResponseType(typeof(List<MAE_ClassDocView>))]
        public async Task<IHttpActionResult> ClassDoc()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<MAE_ClassDocView> tipos = new List<MAE_ClassDocView>();
            var dire = await db.MAE_ClassDoc.ToListAsync();
            foreach (var item in dire)
            {
                if(item.MAE_SubtipoComunicacion == null)
                {
                    MAE_SubtipoComunicacion subAux = new MAE_SubtipoComunicacion();
                    subAux.Nombre = "No registrado";
                    item.MAE_SubtipoComunicacion = subAux;
                }
            }

            dire.ForEach(
            x => tipos.Add(new MAE_ClassDocView()
            {
                IdClassDoc = x.IdClassDoc,
                EsLiquidacion = x.EsLiquidacion,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdClassTwo = x.IdClassTwo,
                IdTipo = x.IdTipo,
                ClassTwo = x.MAE_ClassTwo.Nombre

            }));

            return Ok(tipos);
        }

        /// <summary>
        /// Muestra la información de una ubicación determinada, encontrada por su ID.
        /// </summary>
        /// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassTwo/Find/{IdClassDoc}")]
        [ResponseType(typeof(MAE_ClassDocView))]
        public async Task<IHttpActionResult> ClassDoc(string IdClassDoc)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var x = await db.MAE_ClassDoc.FindAsync(IdClassDoc);
            if (x == null)
                return BadRequest("El ID de la Ubicación no existe.");

            MAE_ClassDocView tipo = new MAE_ClassDocView()
            {
                IdClassDoc = x.IdClassDoc,
                EsLiquidacion = x.EsLiquidacion,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdClassTwo = x.IdClassTwo,
                ClassTwo = x.MAE_ClassTwo.Nombre
            };

            return Ok(tipo);
        }


        [HttpGet]
        [Route("ClassTwo/FindByTwo/{IdClassTwo}")]
        [ResponseType(typeof(List<MAE_ClassDocView>))]
        public async Task<IHttpActionResult> ClassDoc(int IdClassTwo)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<MAE_ClassDocView> tipos = new List<MAE_ClassDocView>();
            var dire = await db.MAE_ClassDoc.Where(x => x.IdClassTwo == IdClassTwo).ToListAsync();

            dire.ForEach( 
            x => tipos.Add(new MAE_ClassDocView()
            {
                IdClassDoc = x.IdClassDoc,
                EsLiquidacion = x.EsLiquidacion,
                IdTipoSub = x.IdTipoSub,
                SubtipoComunicacion = x.MAE_SubtipoComunicacion.Nombre,
                IdClassTwo = x.IdClassTwo,
                ClassTwo = x.MAE_ClassTwo.Nombre
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
