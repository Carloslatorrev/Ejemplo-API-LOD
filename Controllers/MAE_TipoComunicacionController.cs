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
    public class MAE_TipoComunicacionController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        [Route("TipoComunicacion/List")]
        [ResponseType(typeof(List<MAE_TipoComunicacionView>))]
        public async Task<IHttpActionResult> TipoComunicacion()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<MAE_TipoComunicacionView> tipos = new List<MAE_TipoComunicacionView>();
            var dire = await db.MAE_TipoComunicacion.ToListAsync();
            dire.ForEach(
            x => tipos.Add(new MAE_TipoComunicacionView()
            {
                IdTipoCom = x.IdTipoCom,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Activo = x.Activo,
                TipoLibro = x.MAE_TipoLOD.Nombre,
                IdTipoLod = x.IdTipoLod
            }));

            return Ok(tipos);
        }


        /// <summary>
        /// Muestra la información de una ubicación determinada, encontrada por su ID.
        /// </summary>
        /// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("TipoComunicacion/Find/{IdTipoCom}")]
        //[ResponseType(typeof(MAE_TipoComunicacionView))]
        //public async Task<IHttpActionResult> TipoComunicacion(string IdTipoCom)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;

        //    var x = await db.MAE_TipoComunicacion.FindAsync(IdTipoCom);
        //    if (x == null)
        //        return BadRequest("El ID del Tipo Com. no existe.");

        //    MAE_TipoComunicacionView tipo = new MAE_TipoComunicacionView()
        //    {
        //        IdTipoCom = x.IdTipoCom,
        //        Nombre = x.Nombre,
        //        Descripcion = x.Descripcion,
        //        Activo = x.Activo,
        //        TipoLibro = x.MAE_TipoLOD.Nombre
        //    };

        //    return Ok(tipo);
        //}


        //[HttpGet]
        //[Route("TipoComunicacion/FindByLod/{IdTipoLod}")]
        //[ResponseType(typeof(List<MAE_TipoComunicacionView>))]
        //public async Task<IHttpActionResult> TipoComunicacion(int IdTipoLod)
        //{
        //    //string userId = User.Identity.GetUserId();
        //    //string empresaUser = db.Users.Find(userId).IdEmpresa;
        //    List<MAE_TipoComunicacion> mAE_TipoComunicacion = new List<MAE_TipoComunicacion>();
        //    if (IdTipoLod == 1 || IdTipoLod == 2)
        //    {
        //        mAE_TipoComunicacion = db.MAE_TipoComunicacion.Where(x => !x.Nombre.Equals("Comunicación General")).ToList();
        //        MAE_TipoComunicacion auxGeneral = db.MAE_TipoComunicacion.Where(x => x.IdTipoLod == IdTipoLod && x.Nombre.Equals("Comunicación General")).FirstOrDefault();
        //        mAE_TipoComunicacion.Add(auxGeneral);
        //    }
        //    else
        //    {
        //        mAE_TipoComunicacion = db.MAE_TipoComunicacion.Where(x => x.Activo && x.IdTipoLod == IdTipoLod).ToList();
        //    }


        //    List<MAE_TipoComunicacionView> tipos = new List<MAE_TipoComunicacionView>();


        //    mAE_TipoComunicacion.ForEach(
        //    x => tipos.Add(new MAE_TipoComunicacionView()
        //    {
        //        IdTipoCom = x.IdTipoCom,
        //        Nombre = x.Nombre,
        //        Descripcion = x.Descripcion,
        //        Activo = x.Activo,
        //        TipoLibro = x.MAE_TipoLOD.Nombre
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
