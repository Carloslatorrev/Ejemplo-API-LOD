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
using Microsoft.AspNet.Identity;

namespace LODApi.Controllers
{
    public class ReceptoresController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("Receptores/FindByLibro/{IdLod}")]
        [ResponseType(typeof(LOD_UsuariosLodView))]
        public async Task<IHttpActionResult> Receptores(string IdLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            List<LOD_UsuariosLodView> receptores = new List<LOD_UsuariosLodView>();
            var dire = await db.LOD_UsuariosLod.Where(x => x.IdLod == Convert.ToInt32(IdLod) && x.Activo == true).ToListAsync();
            dire.ForEach(
            x => receptores.Add(new LOD_UsuariosLodView()
            {
                IdUsLod = x.IdUsLod,
                Activo = x.Activo,
                Libro = x.LOD_LibroObras.NombreLibroObra,
                FechaActivacion = x.FechaActivacion,
                FechaDesactivacion = x.FechaDesactivacion,
                rol = x.LOD_RolesCttosContrato.NombreRol,
                Usuario = x.ApplicationUser.NombreCompleto

            }));

            return Ok(receptores);
        }

        [HttpPost]
        [Route("Receptores/Create")]
        [ResponseType(typeof(LOD_UserAnotacionView))]
        public async Task<IHttpActionResult> CreateReceptor(ReceptoresView receptor)
        {
            if (receptor == null)
                return BadRequest("Receptor Null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //string userId = User.Identity.GetUserId();

            //string user = User.Identity.GetUserName();
            //string newId = KeyGenerator.GetUniqueKey(10);

            LOD_UserAnotacion receptorCreate = new LOD_UserAnotacion();

            receptorCreate.IdUsLod = receptor.IdUsLod;
            receptorCreate.Leido = false;
            receptorCreate.IdAnotacion = receptor.IdAnotacion;
            receptorCreate.EsRespRespuesta = receptor.EsRespRespuesta;
            receptorCreate.EsPrincipal = receptor.EsPrincipal;
            receptorCreate.RespVB = true;
            receptorCreate.VistoBueno = false;
            receptorCreate.Destacado = false;
                
            try
            {
                db.LOD_UserAnotacion.Add(receptorCreate);
                await db.SaveChangesAsync();
                string nombreUser = await db.LOD_UsuariosLod.Where(x => x.IdUsLod == receptorCreate.IdUsLod).Select(x => x.ApplicationUser.NombreCompleto).FirstOrDefaultAsync();
                string tituloAnotacion = await db.LOD_Anotaciones.Where(x => x.IdAnotacion == receptorCreate.IdAnotacion).Select(x => x.Titulo).FirstOrDefaultAsync();
                LOD_UserAnotacionView u = new LOD_UserAnotacionView()
                {
                    IdUsLod = receptorCreate.IdUsLod,
                    Leido = false,
                    IdAnotacion = receptorCreate.IdAnotacion,
                    EsRespRespuesta = receptorCreate.EsRespRespuesta,
                    EsPrincipal = receptorCreate.EsPrincipal,
                    RespVB = true,
                    UsuarioLod = nombreUser,
                    Anotacion = tituloAnotacion,
                    VistoBueno = receptorCreate.VistoBueno,
                    Destacado = receptorCreate.Destacado
                    
                };

                return Ok(u);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Receptores/Update")]
        [ResponseType(typeof(LOD_UserAnotacionView))]
        public async Task<IHttpActionResult> UpdateReceptor(ReceptoresView receptor)
        {
            if (receptor == null)
                return BadRequest("Receptor Null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            //string user = User.Identity.GetUserName();
            //string newId = KeyGenerator.GetUniqueKey(10);

            LOD_UserAnotacion receptorUpdate = await db.LOD_UserAnotacion.Where(x => x.IdAnotacion == receptor.IdAnotacion && x.IdUsLod == receptor.IdUsLod).FirstOrDefaultAsync();

            try
            {
                receptorUpdate.EsPrincipal = receptor.EsPrincipal;
                receptorUpdate.EsRespRespuesta = receptor.EsRespRespuesta;
                db.Entry(receptorUpdate).State = EntityState.Modified; 
                await db.SaveChangesAsync();
                var appUser = await db.LOD_UsuariosLod.Where(x => x.IdUsLod == receptorUpdate.IdUsLod).Include(x => x.ApplicationUser).Select(x => x.ApplicationUser).FirstOrDefaultAsync();
                string nombreUser = appUser.NombreCompleto;
                string tituloAnotacion = await db.LOD_Anotaciones.Where(x => x.IdAnotacion == receptorUpdate.IdAnotacion).Select(x => x.Titulo).FirstOrDefaultAsync();
                LOD_UserAnotacionView u = new LOD_UserAnotacionView()
                {
                    IdUsLod = receptorUpdate.IdUsLod,
                    Leido = false,
                    IdAnotacion = receptorUpdate.IdAnotacion,
                    EsRespRespuesta = receptorUpdate.EsRespRespuesta,
                    EsPrincipal = receptorUpdate.EsPrincipal,
                    RespVB = true,
                    UsuarioLod = nombreUser,
                    Anotacion = tituloAnotacion,
                    VistoBueno = receptorUpdate.VistoBueno,
                    Destacado = receptorUpdate.Destacado
                };

                return Ok(u);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

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
