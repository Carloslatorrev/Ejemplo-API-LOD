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
    public class LOD_UserAnotacionController : ApiController
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
        [Route("UserAnotacion/FindByUser/{UserId}")]
        [ResponseType(typeof(List<LOD_UserAnotacionView>))]
        public async Task<IHttpActionResult> UserAnotacion(string UserId)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var dire = await db.LOD_UserAnotacion.Where(u => u.LOD_UsuarioLod.UserId == UserId).ToListAsync();
            if (dire == null)
                return BadRequest("El ID del UsuarioLod no existe.");

            List<LOD_UserAnotacionView> lista = new List<LOD_UserAnotacionView>();

            dire.ForEach(
                x => lista.Add(new LOD_UserAnotacionView()
                {
                    IdUsLod = x.IdUsLod,
                    UsuarioLod = x.LOD_UsuarioLod.ApplicationUser.NombreCompleto,
                    IdAnotacion = x.IdAnotacion,
                    Anotacion = x.LOD_Anotaciones.Correlativo + " - " + x.LOD_Anotaciones.Titulo,
                    Destacado = x.Destacado,
                    TempCode = x.TempCode,
                    Leido = x.Leido,
                    EsPrincipal = x.EsPrincipal,
                    EsRespRespuesta = x.EsRespRespuesta,
                    RespVB = x.RespVB,
                    VistoBueno = x.VistoBueno,
                    FechaVB = x.FechaVB,
                    TipoVB = x.TipoVB,
                    RutaImg = x.RutaImg
                }));

            return Ok(lista);
        }



        [HttpGet]
        [Route("UserAnotacion/FindByAnotacion/{IdAnotacion}")]
        [ResponseType(typeof(LOD_UserAnotacionView))]
        public async Task<IHttpActionResult> UserAnotacion(int IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var dire = await db.LOD_UserAnotacion.Where(u => u.IdAnotacion == IdAnotacion).ToListAsync();
            if (dire == null)
                return BadRequest("El ID del UsuarioLod no existe.");

            List<LOD_UserAnotacionView> lista = new List<LOD_UserAnotacionView>();

            dire.ForEach(
                x => lista.Add(new LOD_UserAnotacionView()
                {
                    IdUsLod = x.IdUsLod,
                    UsuarioLod = x.LOD_UsuarioLod.ApplicationUser.NombreCompleto,
                    IdAnotacion = x.IdAnotacion,
                    Anotacion = x.LOD_Anotaciones.Correlativo + " - " + x.LOD_Anotaciones.Titulo,
                    Destacado = x.Destacado,
                    TempCode = x.TempCode,
                    Leido = x.Leido,
                    EsPrincipal = x.EsPrincipal,
                    EsRespRespuesta = x.EsRespRespuesta,
                    RespVB = x.RespVB,
                    VistoBueno = x.VistoBueno,
                    FechaVB = x.FechaVB,
                    TipoVB = x.TipoVB,
                    RutaImg = x.RutaImg
                }));

            return Ok(lista);
        }


        [HttpPost]
        [Route("UserAnotacion/CreateUserBorrador")]
        [ResponseType(typeof(LOD_UserAnotacionView))]
        public async Task<IHttpActionResult> CreateUserAnotacion(UserBorradorView receptor)
        {
            if (receptor == null)
                return BadRequest("UserBorrador Null");

            LOD_UserAnotacion userBorradorCreate = new LOD_UserAnotacion()
            {
                IdUsLod = receptor.IdUsLod,
                Leido = true,
                IdAnotacion = receptor.IdAnotacion,
                EsRespRespuesta = false,
                EsPrincipal = false,
                RespVB = false,
                Destacado = false,
                VistoBueno = false

            };

            try
            {
                db.LOD_UserAnotacion.Add(userBorradorCreate);
                await db.SaveChangesAsync();
                string nombreUser = await db.LOD_UsuariosLod.Where(x => x.IdUsLod == userBorradorCreate.IdUsLod).Select(x => x.ApplicationUser.NombreCompleto).FirstOrDefaultAsync();
                string tituloAnotacion = await db.LOD_Anotaciones.Where(x => x.IdAnotacion == userBorradorCreate.IdAnotacion).Select(x => x.Titulo).FirstOrDefaultAsync();
                LOD_UserAnotacionView u = new LOD_UserAnotacionView()
                {
                    IdUsLod = userBorradorCreate.IdUsLod,
                    Leido = userBorradorCreate.Leido,
                    IdAnotacion = userBorradorCreate.IdAnotacion,
                    EsRespRespuesta = userBorradorCreate.EsRespRespuesta,
                    EsPrincipal = userBorradorCreate.EsPrincipal,
                    RespVB = userBorradorCreate.RespVB,
                    UsuarioLod = nombreUser,
                    Anotacion = tituloAnotacion

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
