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
    public class MAE_ClassTwoController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("ClassTwo/List")]
        [ResponseType(typeof(List<MAE_ClassTwoView>))]
        public async Task<IHttpActionResult> ClassOne()
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<MAE_ClassTwoView> tipos = new List<MAE_ClassTwoView>();
            var dire = await db.MAE_ClassTwo.ToListAsync();
            dire.ForEach(
            x => tipos.Add(new MAE_ClassTwoView()
            {
                IdClassTwo = x.IdClassTwo,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Activo = x.Activo,
                IdClassOne = x.IdClassOne,
                ClassOne = x.MAE_ClassOne.Nombre

            }));

            return Ok(tipos);
        }

        /// <summary>
        /// Muestra la información de una ubicación determinada, encontrada por su ID.
        /// </summary>
        /// <param name="IdTipoCom">ID de la ubicación a encontrar.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ClassTwo/Find/{IdClassTwo}")]
        [ResponseType(typeof(MAE_ClassTwoView))]
        public async Task<IHttpActionResult> ClassOne(string IdClassTwo)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var x = await db.MAE_ClassTwo.FindAsync(IdClassTwo);
            if (x == null)
                return BadRequest("El ID de la Ubicación no existe.");

            MAE_ClassTwoView tipo = new MAE_ClassTwoView()
            {
                IdClassTwo = x.IdClassTwo,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Activo = x.Activo,
                IdClassOne = x.IdClassOne,
                ClassOne = x.MAE_ClassOne.Nombre
            };

            return Ok(tipo);
        }


        [HttpGet]
        [Route("ClassTwo/FindByOne/{IdClassOne}")]
        [ResponseType(typeof(List<MAE_ClassTwoView>))]
        public async Task<IHttpActionResult> ClassTwo(int IdClassOne)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<MAE_ClassTwoView> tipos = new List<MAE_ClassTwoView>();
            var dire = await db.MAE_ClassTwo.Where(x => x.IdClassOne == IdClassOne).ToListAsync();

            dire.ForEach(
            x => tipos.Add(new MAE_ClassTwoView()
            {
                IdClassTwo = x.IdClassTwo,
                Nombre = x.Nombre,
                Descripcion = x.Descripcion,
                Activo = x.Activo,
                IdClassOne = x.IdClassOne,
                ClassOne = x.MAE_ClassOne.Nombre
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
