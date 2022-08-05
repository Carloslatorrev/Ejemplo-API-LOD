using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using LOD_APR.Helpers;
using LODApi.Areas.GLOD.Models;
using LODApi.Helpers;
using LODApi.Models;
using LODApi.ModelsViews;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace LODApi.Controllers
{
    public class LOD_AnotacionesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Log_Helper Log_Helper = new Log_Helper();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Anotaciones/AddReferencia")]
        [ResponseType(typeof(LOD_ReferenciasAnotView))]
        public async Task<IHttpActionResult> AddReferencia(CreateReferencia referencia)
        {
            try
            {
                LOD_ReferenciasAnot refe = new LOD_ReferenciasAnot()
                 {
                   IdAnontacionRef = referencia.IdAnontacionRef,
                    IdAnotacion = referencia.IdAnotacion,
                   EsRepuesta = false,
                     Observacion = referencia.Observacion
                };

                db.LOD_ReferenciasAnot.Add(refe);
                await db.SaveChangesAsync();

                LOD_ReferenciasAnotView refAnot = new LOD_ReferenciasAnotView();
                LOD_ReferenciasAnot refDB = await db.LOD_ReferenciasAnot.Include(x => x.AnotacionOrigen).Include(x => x.AnotacionReferencia).Where(x => x.IdRefAnot == refe.IdRefAnot).FirstOrDefaultAsync();
                refAnot.IdRefAnot = refDB.IdRefAnot;
                refAnot.EsRepuesta = refDB.EsRepuesta;
                refAnot.IdAnontacionRef = refDB.IdAnontacionRef;
                refAnot.IdAnotacion = refDB.IdAnotacion;
                refAnot.AnotacionOrigen = refDB.AnotacionOrigen.Correlativo + "-" + refDB.AnotacionOrigen.Titulo;
                refAnot.AnotacionReferencia = refDB.AnotacionReferencia.Correlativo + "-" + refDB.AnotacionReferencia.Titulo;
                refAnot.Observacion = refDB.Observacion;
                return Ok(refAnot);
                    

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Anotaciones/FindByUserLod")]
        [ResponseType(typeof(List<LOD_AnotacionesView>))]
        public async Task<IHttpActionResult> AnotacionByUserLod(AnotacionUserView userAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var anotaciones = await db.LOD_UserAnotacion.Where(x => x.LOD_UsuarioLod.IdLod == userAnotacion.IdLod && x.LOD_UsuarioLod.UserId == userAnotacion.UserID).ToListAsync();
            List<LOD_AnotacionesView> anotacionesView = new List<LOD_AnotacionesView>();
            foreach (var item in anotaciones)
            {
                if(item.LOD_Anotaciones.UsuarioRemitente == null)
                {
                    ApplicationUser userAux = new ApplicationUser() { Nombres = "-", Apellidos = ""};
                    item.LOD_Anotaciones.UsuarioRemitente = userAux;
                }
            }

            anotaciones.ForEach(
                u => anotacionesView.Add(new LOD_AnotacionesView()
                {
                    IdAnotacion = u.IdAnotacion,
                    IdLod = u.LOD_Anotaciones.IdLod,
                    UserId = u.LOD_Anotaciones.UserId,
                    UserIdBorrador = u.LOD_Anotaciones.UserIdBorrador,
                    Correlativo = u.LOD_Anotaciones.Correlativo,
                    Cuerpo = u.LOD_Anotaciones.Cuerpo,
                    EsEstructurada = u.LOD_Anotaciones.EsEstructurada,
                    Estado = u.LOD_Anotaciones.Estado,
                    EstadoFirma = u.LOD_Anotaciones.EstadoFirma,
                    FechaFirma = u.LOD_Anotaciones.FechaFirma,
                    FechaIngreso = u.LOD_Anotaciones.FechaIngreso,
                    FechaPub = u.LOD_Anotaciones.FechaPub,
                    FechaResp = u.LOD_Anotaciones.FechaResp,
                    FechaTopeRespuesta = u.LOD_Anotaciones.FechaTopeRespuesta,
                    IdTipoSub = u.LOD_Anotaciones.IdTipoSub,
                    SubtipoComunicacion = u.LOD_Anotaciones.MAE_SubtipoComunicacion.Nombre,
                    LibroObras = u.LOD_Anotaciones.LOD_LibroObras.NombreLibroObra,
                    SolicitudRest = u.LOD_Anotaciones.SolicitudRest,
                    SolicitudVB = u.LOD_Anotaciones.SolicitudVB,
                    TempCode = u.TempCode,
                    TipoFirma = u.LOD_Anotaciones.TipoFirma,
                    Titulo = u.LOD_Anotaciones.Titulo,
                    UsuarioBorrador = u.LOD_Anotaciones.UsuarioBorrador.NombreCompleto,
                    UsuarioRemitente = u.LOD_Anotaciones.UsuarioRemitente.NombreCompleto
                }
            ));
            
            return Ok(anotacionesView);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Anotaciones/FindByLod/{IdLod}")]
        [ResponseType(typeof(List<LOD_AnotacionesView>))]
        public async Task<IHttpActionResult> AnotacionByLod(int IdLod)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var anotaciones = await db.LOD_UserAnotacion.Where(x => x.LOD_Anotaciones.IdLod == IdLod && x.LOD_Anotaciones.EstadoFirma == true).ToListAsync();
            List<LOD_AnotacionesView> anotacionesView = new List<LOD_AnotacionesView>();

            anotaciones.ForEach(
                u => anotacionesView.Add(new LOD_AnotacionesView()
                {
                    IdAnotacion = u.IdAnotacion,
                    IdLod = u.LOD_Anotaciones.IdLod,
                    UserId = u.LOD_Anotaciones.UserId,
                    UserIdBorrador = u.LOD_Anotaciones.UserIdBorrador,
                    Correlativo = u.LOD_Anotaciones.Correlativo,
                    Cuerpo = u.LOD_Anotaciones.Cuerpo,
                    EsEstructurada = u.LOD_Anotaciones.EsEstructurada,
                    Estado = u.LOD_Anotaciones.Estado,
                    EstadoFirma = u.LOD_Anotaciones.EstadoFirma,
                    FechaFirma = u.LOD_Anotaciones.FechaFirma,
                    FechaIngreso = u.LOD_Anotaciones.FechaIngreso,
                    FechaPub = u.LOD_Anotaciones.FechaPub,
                    FechaResp = u.LOD_Anotaciones.FechaResp,
                    FechaTopeRespuesta = u.LOD_Anotaciones.FechaTopeRespuesta,
                    IdTipoSub = u.LOD_Anotaciones.IdTipoSub,
                    SubtipoComunicacion = u.LOD_Anotaciones.MAE_SubtipoComunicacion.Nombre,
                    LibroObras = u.LOD_Anotaciones.LOD_LibroObras.NombreLibroObra,
                    SolicitudRest = u.LOD_Anotaciones.SolicitudRest,
                    SolicitudVB = u.LOD_Anotaciones.SolicitudVB,
                    TempCode = u.TempCode,
                    TipoFirma = u.LOD_Anotaciones.TipoFirma,
                    Titulo = u.LOD_Anotaciones.Titulo,
                    UsuarioBorrador = u.LOD_Anotaciones.UsuarioBorrador.NombreCompleto,
                    UsuarioRemitente = u.LOD_Anotaciones.UsuarioRemitente.NombreCompleto,
                    RutaPdfSinFirma = u.LOD_Anotaciones.RutaPdfSinFirma,
                    RutaPdfConFirma = u.LOD_Anotaciones.RutaPdfConFirma
                }
            ));

            return Ok(anotacionesView);
        }


        [HttpGet]
        [Route("Anotaciones/Find/{IdAnotacion}")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> Anotacion(string IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            int id = Convert.ToInt32(IdAnotacion);
            var x = await db.LOD_UserAnotacion.Where(u => u.IdAnotacion == id /*&& x.LOD_UsuarioLod.UserId == userId*/).FirstOrDefaultAsync();
            LOD_AnotacionesView anotacion = new LOD_AnotacionesView()
            {
                IdAnotacion = x.IdAnotacion,
                IdLod = x.LOD_Anotaciones.IdLod,
                UserId = x.LOD_Anotaciones.UserId,
                UserIdBorrador = x.LOD_Anotaciones.UserIdBorrador,
                Correlativo = x.LOD_Anotaciones.Correlativo,
                Cuerpo = x.LOD_Anotaciones.Cuerpo,
                EsEstructurada = x.LOD_Anotaciones.EsEstructurada,
                Estado = x.LOD_Anotaciones.Estado,
                EstadoFirma = x.LOD_Anotaciones.EstadoFirma,
                FechaFirma = x.LOD_Anotaciones.FechaFirma,
                FechaIngreso = x.LOD_Anotaciones.FechaIngreso,
                FechaPub = x.LOD_Anotaciones.FechaPub,
                FechaResp = x.LOD_Anotaciones.FechaResp,
                FechaTopeRespuesta = x.LOD_Anotaciones.FechaTopeRespuesta,
                IdTipoSub = x.LOD_Anotaciones.IdTipoSub,
                SubtipoComunicacion = x.LOD_Anotaciones.MAE_SubtipoComunicacion.Nombre,
                LibroObras = x.LOD_Anotaciones.LOD_LibroObras.NombreLibroObra,
                SolicitudRest = x.LOD_Anotaciones.SolicitudRest,
                SolicitudVB = x.LOD_Anotaciones.SolicitudVB,
                TempCode = x.TempCode,
                TipoFirma = x.LOD_Anotaciones.TipoFirma,
                Titulo = x.LOD_Anotaciones.Titulo,
                UsuarioBorrador = x.LOD_Anotaciones.UsuarioBorrador.NombreCompleto,
                UsuarioRemitente = x.LOD_Anotaciones.UsuarioRemitente.NombreCompleto

            };

            return Ok(anotacion);
        }


        [HttpGet]
        [Route("Anotaciones/FindReceptores/{IdAnotacion}")]
        [ResponseType(typeof(List<LOD_UserAnotacionView>))]
        public async Task<IHttpActionResult> GetReceptores(string IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            int idAnot = Convert.ToInt32(IdAnotacion);
            var recep = await db.LOD_UserAnotacion.Where(ua => ua.IdAnotacion ==  idAnot/*&& x.LOD_UsuarioLod.UserId != userId*/).ToListAsync();
            List<LOD_UserAnotacionView> receptores = new List<LOD_UserAnotacionView>();
            recep.ForEach(r => receptores.Add(new LOD_UserAnotacionView
            {
                IdUsLod = r.IdUsLod,
                IdAnotacion = r.IdAnotacion,
                UsuarioLod = r.LOD_UsuarioLod.ApplicationUser.NombreCompleto,
                EsPrincipal = r.EsPrincipal,
                EsRespRespuesta = r.EsRespRespuesta,
                RutaImg = r.RutaImg,
                VistoBueno = r.VistoBueno,
                RespVB = r.RespVB,
                Anotacion = r.LOD_Anotaciones.Correlativo + " - " + r.LOD_Anotaciones.Titulo,
                Destacado = r.Destacado,
                FechaVB = r.FechaVB,
                Leido = r.Leido
                
            }
           ));
        
            return Ok(receptores);
        }



        [HttpGet]
        [Route("Anotaciones/FindReferencias/{IdAnotacion}")]
        [ResponseType(typeof(List<LOD_ReferenciasAnotView>))]
        public async Task<IHttpActionResult> GetReferencia(string IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;
            int id = Convert.ToInt32(IdAnotacion);
            var refer = await db.LOD_ReferenciasAnot.Where(r => r.IdAnotacion == id).ToListAsync();
            List<LOD_ReferenciasAnotView> referencias = new List<LOD_ReferenciasAnotView>();
            refer.ForEach(r => referencias.Add(new LOD_ReferenciasAnotView
            {
                IdRefAnot = r.IdRefAnot,
                IdAnotacion = r.IdAnotacion,
                IdAnontacionRef = r.IdAnontacionRef,
                AnotacionReferencia = r.AnotacionReferencia.Correlativo + " - " + r.AnotacionReferencia.Titulo,
                EsRepuesta = r.EsRepuesta,
                Observacion = r.Observacion

            }));

            return Ok(referencias);
        }

        [HttpGet]
        [Route("Anotaciones/FindVistoBueno/{IdAnotacion}")]
        [ResponseType(typeof(List<VistoBuenoView>))]
        public async Task<IHttpActionResult> GetVistoBueno(string IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;          
            int id = Convert.ToInt32(IdAnotacion);
            var vb = await db.LOD_UserAnotacion.Where(ua => ua.IdAnotacion == id /*&& x.LOD_UsuarioLod.UserId != userId*/).ToListAsync();
            List<VistoBuenoView> vistoBueno = new List<VistoBuenoView>();
            vb.ForEach(r => vistoBueno.Add(new VistoBuenoView
            {
                IdUsLod = r.IdUsLod,
                IdAnotacion = r.IdAnotacion,
                RespVB = r.RespVB,
                TipoVB = r.TipoVB,
                VistoBueno = r.VistoBueno
            }
            ));

            return Ok(vistoBueno);
        }

        [HttpGet]
        [Route("Anotaciones/FindLogs/{IdAnotacion}")]
        [ResponseType(typeof(List<LOD_logView>))]
        public async Task<IHttpActionResult> GetLogs(string IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;          

            int id = Convert.ToInt32(IdAnotacion);
            var logs = await db.LOD_log.Where(l => l.IdObjeto == id && l.Objeto.Equals("Anotacion")).ToListAsync();
            List<LOD_logView> logAnotacion = new List<LOD_logView>();
            foreach (var item in logs)
            {
                if(item.ApplicationUser == null)
                {
                    ApplicationUser userAux = new ApplicationUser();
                    userAux.Nombres = "-";
                    userAux.Apellidos = "";
                    userAux.Id = "";
                    item.ApplicationUser = userAux;
                }
            }

            logs.ForEach(l => logAnotacion.Add(new LOD_logView
            {
                IdLog = l.IdLog,
                IdObjeto = l.IdObjeto,
                Objeto = l.Objeto,
                Accion = l.Accion,
                UserId = l.UserId,
                Usuario = l.ApplicationUser.NombreCompleto,
                FechaLog = l.FechaLog

            }));
 

            return Ok(logAnotacion);
        }


        [HttpGet]
        [Route("Anotaciones/GetDocumentos/{IdAnotacion}")]
        [ResponseType(typeof(List<LOD_docAnotacionView>))]
        public async Task<IHttpActionResult> GetDocumentos(string IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            int id = Convert.ToInt32(IdAnotacion);
            var anots = await db.LOD_docAnotacion.Where(u => u.IdAnotacion == id /*&& x.LOD_UsuarioLod.UserId == userId*/).ToListAsync();
            List<LOD_docAnotacionView> anotaciones = new List<LOD_docAnotacionView>();

            anots.ForEach(a => anotaciones.Add(new LOD_docAnotacionView
            {
                IdAnotacion = a.IdAnotacion,
                IdDoc = a.IdDoc,
                anotacion = a.LOD_Anotaciones.Correlativo + "-" +a.LOD_Anotaciones.Titulo,
                documento = a.MAE_documentos.NombreDoc,
                EstadoDoc = a.EstadoDoc,
                IdDocAnotacion = a.IdDocAnotacion,
                FechaEvento = a.FechaEvento,
                IdContrato = a.IdContrato,
                IdTipoDoc = a.IdTipoDoc,
                IdUserEvento = a.IdUserEvento,
                Observaciones = a.Observaciones,
                ruta = a.MAE_documentos.Ruta,
                TipoDocumento = a.MAE_TipoDocumento.Tipo,
                UsuarioEvento = "-"
            }));


            return Ok(anotaciones);
        }


        [HttpPost]
        [Route("Anotaciones/Create")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> CreateAnotacion(CreateAnotacion anotacion)
        {
            if (anotacion == null)
                return BadRequest("Anotación Null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //string userId = User.Identity.GetUserId();

            //string user = User.Identity.GetUserName();
            //string newId = KeyGenerator.GetUniqueKey(10);

            LOD_Anotaciones anotacionCreate = new LOD_Anotaciones(){
                IdTipoSub = anotacion.IdSubTipo,
                IdLod = anotacion.IdLod,
                UserIdBorrador = anotacion.UserId,
                Cuerpo = anotacion.cuerpo,
                Titulo = anotacion.titulo,
                Correlativo = 0,
                EsEstructurada = false, //Depende del subtipo de anotación
                Estado = 0,//borrador
                FechaIngreso = DateTime.Now,
                SolicitudRest = anotacion.SolicitudResp,
                FechaTopeRespuesta = anotacion.FechaSolicitud,
                SolicitudVB = anotacion.SolicitudVB,
                TipoFirma = 1, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                EstadoFirma = false,
                FechaPub = null,
                FechaResp = anotacion.FechaSolicitud
            };

            try
            {
                db.LOD_Anotaciones.Add(anotacionCreate);
                await db.SaveChangesAsync();


                LOD_UsuariosLod receptor = await db.LOD_UsuariosLod.Where(x => x.IdLod == anotacionCreate.IdLod && x.UserId == anotacionCreate.UserIdBorrador).FirstOrDefaultAsync();
                LOD_UserAnotacion userBorradorCreate = new LOD_UserAnotacion()
                {
                    IdUsLod = receptor.IdUsLod,
                    Leido = true,
                    IdAnotacion = anotacionCreate.IdAnotacion,
                    EsRespRespuesta = false,
                    EsPrincipal = false,
                    RespVB = false,
                    Destacado = false,
                    VistoBueno = false
                };

                db.LOD_UserAnotacion.Add(userBorradorCreate);
                await db.SaveChangesAsync();

                string accion = "Se ha añadido una nueva anotación en estado Borrador";
                bool response = await Log_Helper.SetLOGAnotacionAsync(anotacionCreate, accion, anotacion.UserId);

                LOD_AnotacionesView a = new LOD_AnotacionesView() {
                    IdAnotacion = anotacionCreate.IdAnotacion,
                    IdTipoSub = anotacionCreate.IdTipoSub,
                    IdLod = anotacionCreate.IdLod,
                    UserIdBorrador = anotacionCreate.UserIdBorrador,
                    Cuerpo = anotacionCreate.Cuerpo,
                    Titulo = anotacionCreate.Titulo,
                    Correlativo = anotacionCreate.Correlativo,
                    EsEstructurada = false, //Depende del subtipo de anotación
                    Estado = anotacionCreate.Estado, //borrador
                    FechaIngreso = anotacionCreate.FechaIngreso,
                    SolicitudRest = anotacionCreate.SolicitudRest,
                    FechaTopeRespuesta = anotacionCreate.FechaTopeRespuesta,
                    SolicitudVB = anotacionCreate.SolicitudVB,
                    TipoFirma = 1, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                    EstadoFirma = anotacionCreate.EstadoFirma,
                    FechaPub = anotacionCreate.FechaPub,
                    FechaResp = anotacionCreate.FechaResp,
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Anotaciones/CreateDocumento")]
        [ResponseType(typeof(LOD_docAnotacionView))]
        public async Task<IHttpActionResult> CreateDoc()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            AddDocumentoView documento = new AddDocumentoView();

            //var file = JsonConvert.DeserializeObject(documento.file);
            var algo2 = HttpContext.Current.Request["JsonDetails"];
            try
            {
                documento = JsonConvert.DeserializeObject<AddDocumentoView>(algo2);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

            var algo = HttpContext.Current.Request;
            HttpPostedFile PerFileNameAux = HttpContext.Current.Request.Files[0];
            HttpPostedFileBase PerFileName = new HttpPostedFileWrapper(PerFileNameAux);

            string fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(PerFileName.FileName);

            if (PerFileName == null || PerFileName.ContentLength <= 0)
            {
                return BadRequest("Debe Añadir un Archivo");
            }



            try
            {
                if (ModelState.IsValid)
                {
                    HelperDocumentos helper_docs = new HelperDocumentos();
                    var anot = await db.LOD_Anotaciones.Where(a => a.IdAnotacion == documento.IdAnotacion && a.EstadoFirma == false).FirstOrDefaultAsync();
                    if (anot != null)
                    {

                        MAE_TipoDocumento tipoDoc = db.MAE_TipoDocumento.Find(documento.IdTipoDoc);
                        MAE_ClassDoc mAE_ClassDoc = db.MAE_ClassDoc.Where(x => x.IdTipo == tipoDoc.IdTipo && x.IdTipoSub == anot.IdTipoSub).FirstOrDefault();

                        MAE_documentos newDoc = new MAE_documentos();
                        newDoc.UserId = documento.UserId;
                        newDoc.NombreDoc = documento.Nombre;
                        newDoc.Descripcion = documento.Descripcion;
                        newDoc.IdPath = mAE_ClassDoc.IdClassTwo;
                        newDoc.Extension = Path.GetExtension(PerFileName.FileName);
                        string PrimaryKeyIdentify = documento.IdAnotacion.ToString();
                        //string doc_pre_name = DateTime.Now.ToString("yyyyMMddTHHmmss");

                        string save_file = helper_docs.SaveFileToDisk(1, documento, PerFileName, $"~/{anot.RutaCarpetaBorradores}", anot.RutaCarpetaBorradores, "", newDoc);
                        string[] result = save_file.Split(';');
                        if (result[0].Equals("Ok"))
                        {
                            string accion = $"Se ha agregado el documento: {documento.Nombre} en la anotación.";
                            bool response = await Log_Helper.SetObjectLog(0, anot, accion, documento.UserId);

                            LOD_docAnotacionView docAnot = new LOD_docAnotacionView();
                            int id = Convert.ToInt32(result[1]);
                            LOD_docAnotacion docAnotAux = await db.LOD_docAnotacion.Where(x => x.IdDoc == id).FirstOrDefaultAsync();
                            docAnot.IdAnotacion = docAnotAux.IdAnotacion;
                            docAnot.IdTipoDoc = docAnotAux.IdTipoDoc;
                            docAnot.anotacion = docAnotAux.LOD_Anotaciones.Correlativo + " - " + docAnotAux.LOD_Anotaciones.Titulo;
                            docAnot.TipoDocumento = docAnotAux.MAE_TipoDocumento.Tipo;
                            docAnot.IdDoc = docAnotAux.IdDoc;
                            docAnot.IdDocAnotacion = docAnotAux.IdDocAnotacion;
                            docAnot.EstadoDoc = docAnotAux.EstadoDoc;
                            docAnot.FechaEvento = docAnotAux.FechaEvento;
                            docAnot.IdContrato = docAnotAux.IdContrato;
                            docAnot.IdUserEvento = docAnotAux.IdUserEvento;
                            docAnot.ruta = docAnotAux.MAE_documentos.Ruta;
                            if(docAnotAux.UsuarioEvento != null)
                            {
                                docAnot.UsuarioEvento = docAnotAux.UsuarioEvento.NombreCompleto;
                            }
                            else
                            {
                                docAnot.UsuarioEvento = "-";
                            }
                            docAnot.Observaciones = docAnotAux.Observaciones;

                            return Ok(docAnot);
                        }
                        else
                        {
                            return BadRequest("Ocurrió un error al subir el documento.");
                        }

                    }
                    else
                    {
                        return BadRequest("La anotación se encuentra inhabilitada para su modificación.");
                    }
                }
                else
                {
                    return BadRequest("Error en el modelo");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }

        [HttpPost]
        [Route("Anotaciones/CreateOtroDocumento")]
        [ResponseType(typeof(LOD_docAnotacionView))]
        public async Task<IHttpActionResult> CreateOtroDoc()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            AddOtroDocumentoView documento = new AddOtroDocumentoView();

            //var file = JsonConvert.DeserializeObject(documento.file);
            var algo2 = HttpContext.Current.Request["JsonDetails"];
            try
            {
                documento = JsonConvert.DeserializeObject<AddOtroDocumentoView>(algo2);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



            var algo = HttpContext.Current.Request;
            HttpPostedFile PerFileNameAux = HttpContext.Current.Request.Files[0];
            HttpPostedFileBase PerFileName = new HttpPostedFileWrapper(PerFileNameAux);

            string fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(PerFileName.FileName);

            if (PerFileName == null || PerFileName.ContentLength <= 0)
            {
                return BadRequest("Debe Añadir un Archivo");
            }



            try
            {
                if (ModelState.IsValid)
                {
                    HelperDocumentos helper_docs = new HelperDocumentos();
                    var anot = await db.LOD_Anotaciones.Where(a => a.IdAnotacion == documento.IdAnotacion && a.EstadoFirma == false).FirstOrDefaultAsync();
                    if (anot != null)
                    {

                        MAE_TipoDocumento tipoDoc = db.MAE_TipoDocumento.Find(documento.IdTipoDoc);
                        MAE_ClassDoc mAE_ClassDoc = db.MAE_ClassDoc.Where(x => x.IdClassTwo == documento.IdClassTwo && x.IdTipo == tipoDoc.IdTipo).FirstOrDefault();
                        int idClasstwo = documento.IdClassTwo;
                        if(mAE_ClassDoc != null)
                        {
                            idClasstwo = mAE_ClassDoc.IdClassTwo;
                        }

                        MAE_documentos newDoc = new MAE_documentos();
                        newDoc.UserId = documento.UserId;
                        newDoc.NombreDoc = documento.Nombre;
                        newDoc.Descripcion = documento.Descripcion;
                        newDoc.IdPath = idClasstwo;
                        newDoc.Extension = Path.GetExtension(PerFileName.FileName);
                        string PrimaryKeyIdentify = documento.IdAnotacion.ToString();
                        //string doc_pre_name = DateTime.Now.ToString("yyyyMMddTHHmmss");

                        string save_file = helper_docs.SaveFileToDisk(1, documento, PerFileName, $"~/{anot.RutaCarpetaBorradores}", anot.RutaCarpetaBorradores, "", newDoc);
                        string[] result = save_file.Split(';');
                        if (result[0].Equals("Ok"))
                        {
                            string accion = $"Se ha agregado el documento: {documento.Nombre} en la anotación.";
                            bool response = await Log_Helper.SetObjectLog(0, anot, accion, documento.UserId);

                            LOD_docAnotacionView docAnot = new LOD_docAnotacionView();
                            int id = Convert.ToInt32(result[1]);
                            LOD_docAnotacion docAnotAux = await db.LOD_docAnotacion.Where(x => x.IdDoc == id).FirstOrDefaultAsync();
                            docAnot.IdAnotacion = docAnotAux.IdAnotacion;
                            docAnot.IdTipoDoc = docAnotAux.IdTipoDoc;
                            docAnot.anotacion = docAnotAux.LOD_Anotaciones.Correlativo + " - " + docAnotAux.LOD_Anotaciones.Titulo;
                            docAnot.TipoDocumento = docAnotAux.MAE_TipoDocumento.Tipo;
                            docAnot.IdDoc = docAnotAux.IdDoc;
                            docAnot.IdDocAnotacion = docAnotAux.IdDocAnotacion;
                            docAnot.EstadoDoc = docAnotAux.EstadoDoc;
                            docAnot.FechaEvento = docAnotAux.FechaEvento;
                            docAnot.IdContrato = docAnotAux.IdContrato;
                            docAnot.IdUserEvento = docAnotAux.IdUserEvento;
                            docAnot.ruta = docAnotAux.MAE_documentos.Ruta;
                            if (docAnotAux.UsuarioEvento != null)
                            {
                                docAnot.UsuarioEvento = docAnotAux.UsuarioEvento.NombreCompleto;
                            }
                            else
                            {
                                docAnot.UsuarioEvento = "-";
                            }
                            docAnot.Observaciones = docAnotAux.Observaciones;

                            return Ok(docAnot);
                        }
                        else
                        {
                            return BadRequest("Ocurrió un error al subir el documento.");
                        }

                    }
                    else
                    {
                        return BadRequest("La anotación se encuentra inhabilitada para su modificación.");
                    }
                }
                else
                {
                    return BadRequest("Error en el modelo");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


        }



        [HttpPost]
        [Route("Anotaciones/Update")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> UpdateAnotacion(UpdateAnotacion anotacion)
        {
            if (anotacion == null)
                return BadRequest("Anotación Null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //string userId = User.Identity.GetUserId();

            //string user = User.Identity.GetUserName();
            //string newId = KeyGenerator.GetUniqueKey(10);

            try
            {
                LOD_Anotaciones anotacionCreate = await db.LOD_Anotaciones.FindAsync(anotacion.IdAnotacion);
                anotacionCreate.Titulo = anotacion.titulo;
                anotacionCreate.Cuerpo = anotacion.cuerpo;
                anotacionCreate.SolicitudRest = anotacion.SolicitudResp;
                anotacionCreate.FechaTopeRespuesta = anotacion.FechaSolicitud;


                db.Entry(anotacionCreate).State = EntityState.Modified;
                await db.SaveChangesAsync();

                LOD_AnotacionesView a = new LOD_AnotacionesView()
                {
                    IdAnotacion = anotacionCreate.IdAnotacion,
                    IdTipoSub = anotacionCreate.IdTipoSub,
                    IdLod = anotacionCreate.IdLod,
                    UserIdBorrador = anotacionCreate.UserIdBorrador,
                    Cuerpo = anotacionCreate.Cuerpo,
                    Titulo = anotacionCreate.Titulo,
                    Correlativo = anotacionCreate.Correlativo,
                    EsEstructurada = false, //Depende del subtipo de anotación
                    Estado = anotacionCreate.Estado, //borrador
                    FechaIngreso = anotacionCreate.FechaIngreso,
                    SolicitudRest = anotacionCreate.SolicitudRest,
                    FechaTopeRespuesta = anotacionCreate.FechaTopeRespuesta,
                    SolicitudVB = anotacionCreate.SolicitudVB,
                    TipoFirma = anotacionCreate.TipoFirma, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                    EstadoFirma = anotacionCreate.EstadoFirma,
                    FechaPub = anotacionCreate.FechaPub,
                    FechaResp = anotacionCreate.FechaResp,
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Route("Anotaciones/Delete/{IdAnotacion}")]
        [ResponseType(typeof(LOD_LibroObrasView))]
        public async Task<IHttpActionResult> DeleteAnotacion(int IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_Anotaciones anotacionDelete = await db.LOD_Anotaciones.FindAsync(IdAnotacion);
                LOD_LibroObras libroReturn = await db.LOD_LibroObras.FindAsync(anotacionDelete.IdLod);


                db.LOD_Anotaciones.Remove(anotacionDelete);
                await db.SaveChangesAsync();
                LOD_LibroObrasView a = new LOD_LibroObrasView()
                {
                    IdLod = libroReturn.IdLod
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Route("Anotaciones/RemoveDoc/{IdDocAnotacion}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> RemoveDoc(int IdDocAnotacion)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_docAnotacion anotacionDelete = await db.LOD_docAnotacion.FindAsync(IdDocAnotacion);
                int IdDoc = anotacionDelete.IdDoc;
                db.LOD_docAnotacion.Remove(anotacionDelete);
                await db.SaveChangesAsync();
                MAE_documentos doc = await db.MAE_documentos.FindAsync(IdDoc);
                db.MAE_documentos.Remove(doc);
                await db.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Route("Anotaciones/RemoveReferencia/{IdRefAnot}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> RemoveReferencia(int IdRefAnot)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_ReferenciasAnot anotacionDelete = await db.LOD_ReferenciasAnot.FindAsync(IdRefAnot);
                db.LOD_ReferenciasAnot.Remove(anotacionDelete);
                await db.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Route("Anotaciones/RemoveReceptor/{IdCompuesto}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> RemoveReceptor(string IdCompuesto)
        {
            //string userId = User.Identity.GetUserId();
            var cadenas = IdCompuesto.Split(';');
            try
            {
                int IdUsLod = Convert.ToInt32(cadenas[0]);
                int IdAnotacion = Convert.ToInt32(cadenas[1]);
                LOD_UserAnotacion anotacionDelete = await db.LOD_UserAnotacion.Where(x => x.IdAnotacion == IdAnotacion && x.IdUsLod == IdUsLod).FirstOrDefaultAsync();
                db.LOD_UserAnotacion.Remove(anotacionDelete);

                await db.SaveChangesAsync();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        [HttpGet]
        [Route("Anotaciones/ExiteAnotacion")]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> ExiteAnotacion(int IdAnotacion)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_Anotaciones anotacion = await db.LOD_Anotaciones.FindAsync(IdAnotacion);
                if (anotacion != null)
                {
                    return Ok("true");
                }
                else
                {
                    return Ok("false");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Anotaciones/FirmarAnotacionOne")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> FirmarAnotacionOne(FirmarAnotacionView anotacion)
        {
            //string userId = User.Identity.GetUserId();
            bool val = true;
            int IdAnotacion = anotacion.IdAnotacion;
            try
            {
                LOD_Anotaciones anot = await db.LOD_Anotaciones.FindAsync(IdAnotacion);

                if (anot.EstadoFirma)
                {
                    return InternalServerError();
                }

                if (await IsDestPendientes(IdAnotacion))
                {
                    bool response = await SetDocsPendientes(IdAnotacion, anotacion.UserId);


                }
                else if (await IsDestPrincipal(IdAnotacion) == false && anot.MAE_SubtipoComunicacion.MAE_TipoComunicacion.IdTipoLod == 1)
                {
                    bool response = await SetDocsPendientes(IdAnotacion, anotacion.UserId);
                }


                if (await IsDocsPendientes(IdAnotacion))
                {
                    return InternalServerError();
                }


                int tipo = 3;

                FirmarAnotacion firm = new FirmarAnotacion();
                string filePath = string.Empty;
                if (tipo > 1)
                {
                    if (await firm.FirmarAnotacionDB(IdAnotacion, tipo, anotacion.UserId))
                    {
                        db.Entry(anot).Reload(); //REFRESCAMOS LA ANOTACION EN MEMORIA CON LOS NUEVOS CAMBIOS DE LA BBDD
                        //filePath = await firm.GeneratePDF(IdAnotacion);
                        //if (String.IsNullOrEmpty(filePath))
                        //{
                        //    //val.ErrorMessage.Add("Ocurrió un Problema al tratar de guardar el archivo pdf de la Anotación.");
                        //    await firm.QuitarFirmaAnotacionDB(IdAnotacion);
                        //    return InternalServerError();
                        //    val = false;
                        //    //return Json(val, JsonRequestBehavior.AllowGet);
                        //}

                        GLOD_Notificaciones notificaciones = new GLOD_Notificaciones();
                        //string userid = User.Identity.GetUserId();
                        string userid = anotacion.UserId;
                        LOD_ReferenciasAnot referencia = db.LOD_ReferenciasAnot.Where(x => x.IdAnotacion == anot.IdAnotacion).FirstOrDefault();
                        int resu = 0;
                        if (referencia == null)  //NOTIFICAR PUBLICACION O NOTIFICAR RESPUESTA
                            resu = await notificaciones.NotificarPublicacion(anot, userid);
                        else
                            resu = await notificaciones.NotificarRespuesta(anot, userid);
                    }
                    else
                    {
                        //val.ErrorMessage.Add("Ocurrió un Problema al tratar de guardar los datos de firma de la Anotación.");
                        //return Json(val, JsonRequestBehavior.AllowGet);
                        val = false;
                        return InternalServerError();
                    }
                }

                //try
                //{
                //    //var valid = (await UserManager.PasswordValidator.ValidateAsync(anotacion.password)).Succeeded;
                //    bool valid = await ValidarPassword(anotacion.password);
                //    if (valid)
                //    {
                //        //val.Parametros = $"/GLOD/Anotaciones/Edit/{anot.IdAnotacion}";
                //        System.IO.File.Move(filePath, filePath.Replace(".preview", ""));
                //        string accion = "Anotación Firmada Correctamente";
                //        val = true;
                //        bool response = await Log_Helper.SetLOGAnotacionAsync(anot, accion, User.Identity.GetUserId());
                //    }
                //    else
                //    {
                //        return InternalServerError();
                //    }


                //}
                //catch (Exception ex)
                //{
                //    return InternalServerError(ex);
                //}

                //if (!val)
                //{
                //    try
                //    {
                //        await firm.QuitarFirmaAnotacionDB(IdAnotacion);
                //        bool response2 = await SetNoAprobadoDocsPendientes(IdAnotacion);
                //        System.IO.File.Delete(filePath);
                //    }
                //    catch { }
                //}
                //else
                //{

                //    if (tipo > 1)//CON FIRMA FEA NO SE DEBEN MOVER LOS ADJUNTOS DEBIDO A QUE LO HACE EN API
                //    {
                //        try
                //        {
                //            List<LOD_docAnotacion> docsAnotacion = await db.LOD_docAnotacion.Where(d => d.IdAnotacion == IdAnotacion).ToListAsync();
                //            string rutaBase = System.IO.Path.GetDirectoryName(filePath);
                //            foreach (var doc in docsAnotacion)
                //            {
                //                string borrador = Path.Combine(HttpContext.Current.Server.MapPath("~/"), doc.MAE_documentos.Ruta);
                //                string destino = Path.Combine(HttpContext.Current.Server.MapPath("~/"), anot.RutaCarpetaPdf, doc.MAE_documentos.NombreDoc);
                //                System.IO.File.Move(borrador, destino);

                //                doc.MAE_documentos.Ruta = $"{anot.RutaCarpetaPdf}/{doc.MAE_documentos.NombreDoc}";
                //                db.Entry(doc).State = EntityState.Modified;
                //                await db.SaveChangesAsync();
                //            }

                //            System.IO.Directory.Delete(Path.Combine(HttpContext.Current.Server.MapPath("~/"), anot.RutaCarpetaBorradores));
                //        }
                //        catch { }
                //    }
                //}

                anot = await db.LOD_Anotaciones.FindAsync(anotacion.IdAnotacion);

                LOD_AnotacionesView a = new LOD_AnotacionesView()
                {
                    IdAnotacion = anot.IdAnotacion,
                    IdTipoSub = anot.IdTipoSub,
                    IdLod = anot.IdLod,
                    UserIdBorrador = anot.UserIdBorrador,
                    UserId = anot.UserId,
                    Cuerpo = anot.Cuerpo,
                    Titulo = anot.Titulo,
                    Correlativo = anot.Correlativo,
                    EsEstructurada = false, //Depende del subtipo de anotación
                    Estado = anot.Estado, //borrador
                    FechaIngreso = anot.FechaIngreso,
                    SolicitudRest = anot.SolicitudRest,
                    FechaTopeRespuesta = anot.FechaTopeRespuesta,
                    SolicitudVB = anot.SolicitudVB,
                    TipoFirma = anot.TipoFirma, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                    EstadoFirma = anot.EstadoFirma,
                    //UserIdBorrador = userId,
                    FechaPub = anot.FechaPub,
                    FechaResp = anot.FechaResp,
                    FechaFirma = anot.FechaFirma,
                    RutaPdfConFirma = anot.RutaPdfConFirma

                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Anotaciones/FirmarAnotacion")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> FirmarAnotacion(FirmarAnotacionView anotacion)
        {
            //string userId = User.Identity.GetUserId();
            bool val = true;
            int IdAnotacion = anotacion.IdAnotacion;
            try
            {
                LOD_Anotaciones anot = await db.LOD_Anotaciones.FindAsync(Convert.ToInt32(anotacion.IdAnotacion));

                if (anot.EstadoFirma)
                {
                    return InternalServerError();
                }

                if (await IsDestPendientes(IdAnotacion))
                {
                    bool response = await SetDocsPendientes(IdAnotacion, anotacion.UserId);

                   
                }
                else if (await IsDestPrincipal(IdAnotacion) == false && anot.MAE_SubtipoComunicacion.MAE_TipoComunicacion.IdTipoLod == 1)
                {
                    bool response = await SetDocsPendientes(IdAnotacion, anotacion.UserId);
                }


                if (await IsDocsPendientes(IdAnotacion))
                {
                    return InternalServerError();
                }

                //if (await IsPrincipalPendiente(IdAnotacion))
                //{
                //    val.ErrorMessage.Add("Debe agregar a lo menos un Receptor como Principal en la anotación.");
                //    return Json(val, JsonRequestBehavior.AllowGet);
                //}

                //if (await IsRespRespuestaPendiente(IdAnotacion, anot.SolicitudRest))
                //{
                //    val.ErrorMessage.Add("Debe agregar a lo menos un Receptor como Responsable de Respuesta en la anotación.");
                //    return Json(val, JsonRequestBehavior.AllowGet);
                //}

                int tipo = 3;

                FirmarAnotacion firm = new FirmarAnotacion();
                string filePath = string.Empty;
                if (tipo > 1)
                {
                    if (await firm.FirmarAnotacionDB(IdAnotacion, tipo, anotacion.UserId))
                    {
                        db.Entry(anot).Reload(); //REFRESCAMOS LA ANOTACION EN MEMORIA CON LOS NUEVOS CAMBIOS DE LA BBDD
                        filePath = await firm.GeneratePDF(IdAnotacion);
                        if (String.IsNullOrEmpty(filePath))
                        {
                            //val.ErrorMessage.Add("Ocurrió un Problema al tratar de guardar el archivo pdf de la Anotación.");
                            await firm.QuitarFirmaAnotacionDB(IdAnotacion);
                            return InternalServerError();
                            val = false;
                            //return Json(val, JsonRequestBehavior.AllowGet);
                        }

                        GLOD_Notificaciones notificaciones = new GLOD_Notificaciones();
                        string userid = anotacion.UserId;
                        LOD_ReferenciasAnot referencia = db.LOD_ReferenciasAnot.Where(x => x.IdAnotacion == anot.IdAnotacion).FirstOrDefault();
                        int resu = 0;
                        if (referencia == null)  //NOTIFICAR PUBLICACION O NOTIFICAR RESPUESTA
                            resu = await notificaciones.NotificarPublicacion(anot, userid);
                        else
                            resu = await notificaciones.NotificarRespuesta(anot, userid);
                    }
                    else
                    {
                        //val.ErrorMessage.Add("Ocurrió un Problema al tratar de guardar los datos de firma de la Anotación.");
                        //return Json(val, JsonRequestBehavior.AllowGet);
                        val = false;
                        return InternalServerError();
                    }
                }

                try
                {
                    //var valid = (await UserManager.PasswordValidator.ValidateAsync(anotacion.password)).Succeeded;
                        bool valid = await ValidarPassword(anotacion.password);
                        if (valid)
                        {
                            //val.Parametros = $"/GLOD/Anotaciones/Edit/{anot.IdAnotacion}";
                            System.IO.File.Move(filePath, filePath.Replace(".preview", ""));
                            string accion = "Anotación Firmada Correctamente";
                            val = true;
                            bool response = await Log_Helper.SetLOGAnotacionAsync(anot, accion, anotacion.UserId);
                        }
                        else
                        {
                            return InternalServerError();
                        }

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }

                if (!val)
                {
                    try
                    {
                        await firm.QuitarFirmaAnotacionDB(IdAnotacion);
                        bool response2 = await SetNoAprobadoDocsPendientes(IdAnotacion);
                        System.IO.File.Delete(filePath);
                    }
                    catch { }
                }
                else
                {

                    if (tipo > 1)//CON FIRMA FEA NO SE DEBEN MOVER LOS ADJUNTOS DEBIDO A QUE LO HACE EN API
                    {
                        try
                        {
                            List<LOD_docAnotacion> docsAnotacion = await db.LOD_docAnotacion.Where(d => d.IdAnotacion == IdAnotacion).ToListAsync();
                            string rutaBase = System.IO.Path.GetDirectoryName(filePath);
                            foreach (var doc in docsAnotacion)
                            {
                                string borrador = Path.Combine(HttpContext.Current.Server.MapPath("~/"), doc.MAE_documentos.Ruta);
                                borrador = borrador.Replace("LODApi", "LOD_APR");
                                string destino = Path.Combine(HttpContext.Current.Server.MapPath("~/"), anot.RutaCarpetaPdf, doc.MAE_documentos.NombreDoc);
                                destino = destino.Replace("LODApi", "LOD_APR");
                                System.IO.File.Move(borrador, destino);

                                doc.MAE_documentos.Ruta = $"{anot.RutaCarpetaPdf}/{doc.MAE_documentos.NombreDoc}";
                                db.Entry(doc).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                            }
                            string pathDelete = Path.Combine(HttpContext.Current.Server.MapPath("~/"));
                            pathDelete = pathDelete.Replace("LODApi", "LOD_APR");
                            System.IO.Directory.Delete(Path.Combine(pathDelete, anot.RutaCarpetaBorradores));
                        }
                        catch (Exception ex){
                            string message = ex.Message;
                        }
                    }
                }

                anot = await db.LOD_Anotaciones.FindAsync(anotacion.IdAnotacion);

                LOD_AnotacionesView a = new LOD_AnotacionesView()
                {
                    IdAnotacion = anot.IdAnotacion,
                    IdTipoSub = anot.IdTipoSub,
                    IdLod = anot.IdLod,
                    UserIdBorrador = anot.UserIdBorrador,
                    Cuerpo = anot.Cuerpo,
                    Titulo = anot.Titulo,
                    Correlativo = anot.Correlativo,
                    EsEstructurada = false, //Depende del subtipo de anotación
                    Estado = anot.Estado, //borrador
                    FechaIngreso = anot.FechaIngreso,
                    SolicitudRest = anot.SolicitudRest,
                    FechaTopeRespuesta = anot.FechaTopeRespuesta,
                    SolicitudVB = anot.SolicitudVB,
                    TipoFirma = anot.TipoFirma, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                    EstadoFirma = anot.EstadoFirma,
                    //UserIdBorrador = userId,
                    FechaPub = anot.FechaPub,
                    FechaResp = anot.FechaResp,
                    FechaFirma = anot.FechaFirma,
                    RutaPdfConFirma = anot.RutaPdfConFirma
                   
                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        public async Task<bool> ValidarPassword(string password)
        {
            bool response = true;


            return response;
        }

        public async Task<bool> IsDocsPendientes(int id)
        {
            LOD_Anotaciones lOD_Anotaciones = db.LOD_Anotaciones.Find(id);
            List<MAE_TipoDocumento> docRequeridos = db.MAE_CodSubCom.Where(x => x.IdTipoSub == lOD_Anotaciones.IdTipoSub).Select(x => x.MAE_TipoDocumento).ToList();
            List<LOD_docAnotacion> docCargados = db.LOD_docAnotacion.Where(x => x.IdAnotacion == lOD_Anotaciones.IdAnotacion).ToList();
            docRequeridos = docRequeridos.Except(docCargados.Select(x => x.MAE_TipoDocumento)).ToList();

            return (docRequeridos.Count > 0) ? true : false;
        }

        public async Task<bool> SetDocsPendientes(int id, string userid)
        {
            LOD_Anotaciones lOD_Anotaciones = db.LOD_Anotaciones.Find(id);

            List<LOD_docAnotacion> docCargados = db.LOD_docAnotacion.Where(x => x.IdAnotacion == lOD_Anotaciones.IdAnotacion).ToList();
            bool response = true;
            foreach (var doc in docCargados)
            {
                try
                {
                    doc.EstadoDoc = 2;
                    doc.FechaEvento = DateTime.Now;
                    doc.IdUserEvento = userid;
                    db.Entry(doc).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    string tipodoc = doc.MAE_TipoDocumento.Tipo;

                    string accion = "Se ha aprobado el documento:" + tipodoc;
                    bool res = await Log_Helper.SetObjectLog(0, lOD_Anotaciones, accion, userid);

                    if (doc.LOD_Anotaciones.IdTipoSub.Equals(30))
                    {
                        if (doc.IdTipoDoc.Equals(64))
                        {
                            List<FORM_InformesItems> itemsInformes = db.FORM_InformesItems.Where(x => x.IdAnotacion == doc.IdAnotacion).ToList();
                            if (itemsInformes != null)
                            {
                                foreach (var item in itemsInformes)
                                {
                                    string nombreLimpio = doc.MAE_documentos.NombreDoc.Replace("_", " ");
                                    if (nombreLimpio.Contains(item.Titulo))
                                    {
                                        item.Estado = 3;
                                        db.Entry(item).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }

                                }

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    response = false;
                }
            }



            return response;
        }


        public async Task<bool> SetNoAprobadoDocsPendientes(int id)
        {
            LOD_Anotaciones lOD_Anotaciones = db.LOD_Anotaciones.Find(id);

            List<LOD_docAnotacion> docCargados = db.LOD_docAnotacion.Where(x => x.IdAnotacion == lOD_Anotaciones.IdAnotacion).ToList();
            bool response = true;
            foreach (var item in docCargados)
            {
                try
                {
                    item.EstadoDoc = 1;
                    db.Entry(item).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    response = false;
                }
            }
            return response;
        }

        public async Task<bool> IsDestPendientes(int id)
        {
            int totaluser = db.LOD_UserAnotacion.Where(u => u.IdAnotacion == id && u.RespVB).ToList().Count;
            return (totaluser == 0) ? true : false;
        }

        public async Task<bool> IsDestPrincipal(int id)
        {
            int totaluser = db.LOD_UserAnotacion.Where(u => u.IdAnotacion == id && u.EsPrincipal).ToList().Count;
            return (totaluser != 0) ? true : false;
        }

        public async Task<bool> IsRespRespuestaPendiente(int id, bool reqResp)
        {
            if (reqResp)
            {
                int totaluser = db.LOD_UserAnotacion.Where(u => u.IdAnotacion == id && u.EsRespRespuesta).ToList().Count;
                return (totaluser == 0) ? true : false;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> IsPrincipalPendiente(int id)
        {
            int totaluser = db.LOD_UserAnotacion.Where(u => u.IdAnotacion == id && u.EsPrincipal).ToList().Count;
            return (totaluser == 0) ? true : false;
        }

        public async Task<bool> FirmarAnotacionDB(int id, int tipo, string userid)
        {

            try
            {
                LOD_Anotaciones anot = await db.LOD_Anotaciones.FindAsync(id);
                anot.Correlativo = db.LOD_Anotaciones.Where(a => a.Estado == 2 && a.IdLod == anot.IdLod).Count() + 1;
                anot.FechaFirma = DateTime.Now;
                anot.Estado = 2;
                anot.EstadoFirma = true;
                anot.FechaPub = DateTime.Now;
                anot.TipoFirma = tipo;
                anot.UserId = userid;
                db.Entry(anot).State = EntityState.Modified;
                await db.SaveChangesAsync();

                GLOD_Notificaciones notificaciones = new GLOD_Notificaciones();
                //string userid = GetUserInSession().Id;
                LOD_ReferenciasAnot referencia = db.LOD_ReferenciasAnot.Where(x => x.IdAnotacion == anot.IdAnotacion).FirstOrDefault();
                int resu = 0;
                if (referencia == null)  //NOTIFICAR PUBLICACION O NOTIFICAR RESPUESTA
                    resu = await notificaciones.NotificarPublicacion(anot, userid);
                else
                    resu = await notificaciones.NotificarRespuesta(anot, userid);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> QuitarFirmaAnotacionDB(int id)
        {
            try
            {
                LOD_Anotaciones anot = await db.LOD_Anotaciones.FindAsync(id);
                anot.Correlativo = 0;
                anot.FechaFirma = null;
                anot.Estado = 0;
                anot.EstadoFirma = false;
                anot.FechaPub = null;
                anot.TipoFirma = 0;
                anot.UserId = null;
                db.Entry(anot).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        [Route("Anotaciones/VBConfirmed")]
        [ResponseType(typeof(LOD_UserAnotacionView))]
        public async Task<IHttpActionResult> VBConfirmed(VBConfirmedView anotacion)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_UserAnotacion userAnot = await db.LOD_UserAnotacion.Where(x => x.IdUsLod == anotacion.IdUsLod && x.IdAnotacion == anotacion.IdAnotacion).FirstOrDefaultAsync();
                userAnot.VistoBueno = true;
                userAnot.FechaVB = DateTime.Now;
                db.Entry(userAnot).State = EntityState.Modified;
                await db.SaveChangesAsync();


                LOD_UserAnotacionView u = new LOD_UserAnotacionView()
                {
                    IdUsLod = userAnot.IdUsLod,
                    Leido = true,
                    IdAnotacion = userAnot.IdAnotacion,
                    EsRespRespuesta = userAnot.EsRespRespuesta,
                    EsPrincipal = userAnot.EsPrincipal,
                    RespVB = userAnot.RespVB,
                    VistoBueno = userAnot.VistoBueno,
                    Destacado = userAnot.Destacado,
                    UsuarioLod = userAnot.LOD_UsuarioLod.ApplicationUser.NombreCompleto,
                    Anotacion = userAnot.LOD_Anotaciones.Titulo

                };

                return Ok(u);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        [HttpPost]
        [Route("Anotaciones/SolicitarFirma")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> SolicitudFirma(SolicitudFirma anotacion)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_Anotaciones anotacionFirmada = await db.LOD_Anotaciones.FindAsync(anotacion.IdAnotacion);
                LOD_UsuariosLod userLod = await db.LOD_UsuariosLod.Where(x => x.IdUsLod == anotacion.IdFirmante).FirstOrDefaultAsync();
                anotacionFirmada.Estado = 1;
                anotacionFirmada.UserId = userLod.UserId;
                db.Entry(anotacionFirmada).State = EntityState.Modified;
                await db.SaveChangesAsync();

                string accion = "Solicitud de Firma Enviada Correctamente";
                bool response = await Log_Helper.SetLOGAnotacionAsync(anotacionFirmada, accion, userLod.UserId);

                LOD_AnotacionesView a = new LOD_AnotacionesView()
                {
                    IdAnotacion = anotacionFirmada.IdAnotacion,
                    IdTipoSub = anotacionFirmada.IdTipoSub,
                    IdLod = anotacionFirmada.IdLod,
                    UserId = anotacionFirmada.UserId,
                    UserIdBorrador = anotacionFirmada.UserIdBorrador,
                    Cuerpo = anotacionFirmada.Cuerpo,
                    Titulo = anotacionFirmada.Titulo,
                    Correlativo = anotacionFirmada.Correlativo,
                    EsEstructurada = false, //Depende del subtipo de anotación
                    Estado = anotacionFirmada.Estado, //borrador
                    FechaIngreso = anotacionFirmada.FechaIngreso,
                    SolicitudRest = anotacionFirmada.SolicitudRest,
                    FechaTopeRespuesta = anotacionFirmada.FechaTopeRespuesta,
                    SolicitudVB = anotacionFirmada.SolicitudVB,
                    TipoFirma = anotacionFirmada.TipoFirma, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                    EstadoFirma = anotacionFirmada.EstadoFirma,
                    //UserIdBorrador = userId,
                    FechaPub = anotacionFirmada.FechaPub,
                    FechaResp = anotacionFirmada.FechaResp,
                    FechaFirma = anotacionFirmada.FechaFirma,

                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Anotaciones/TomaConocimiento")]
        [ResponseType(typeof(LOD_AnotacionesView))]
        public async Task<IHttpActionResult> TomaConocimiento(TomaConocimiento toma)
        {
            //string userId = User.Identity.GetUserId();

            try
            {
                LOD_Anotaciones anotacionFirmada = await db.LOD_Anotaciones.FindAsync(toma.IdAnotacion);
                var valid = (await UserManager.PasswordValidator.ValidateAsync(toma.password)).Succeeded;
                if (valid)
                {
                    var userAnotacionActual = db.LOD_UserAnotacion.Where(x => x.IdAnotacion == toma.IdAnotacion && x.LOD_UsuarioLod.UserId == toma.UserId).FirstOrDefault();
                    if (userAnotacionActual != null)
                    {
                        userAnotacionActual.FechaVB = DateTime.Now;
                        userAnotacionActual.VistoBueno = true;
                        userAnotacionActual.TipoVB = 3;
                        db.Entry(userAnotacionActual).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    string accion = $"{userAnotacionActual.LOD_UsuarioLod.ApplicationUser.NombreCompleto} confirma Toma de Conocimiento correctamente";
                    bool response = await Log_Helper.SetLOGAnotacionAsync(anotacionFirmada, accion, toma.UserId);
                }
                else
                {
                    return BadRequest("El password ingresado no es válido para el usuario en sesión.");
                }


                LOD_AnotacionesView a = new LOD_AnotacionesView()
                {
                    IdAnotacion = anotacionFirmada.IdAnotacion,
                    IdTipoSub = anotacionFirmada.IdTipoSub,
                    IdLod = anotacionFirmada.IdLod,
                    UserIdBorrador = anotacionFirmada.UserIdBorrador,
                    Cuerpo = anotacionFirmada.Cuerpo,
                    Titulo = anotacionFirmada.Titulo,
                    Correlativo = anotacionFirmada.Correlativo,
                    EsEstructurada = false, //Depende del subtipo de anotación
                    Estado = anotacionFirmada.Estado, //borrador
                    FechaIngreso = anotacionFirmada.FechaIngreso,
                    SolicitudRest = anotacionFirmada.SolicitudRest,
                    FechaTopeRespuesta = anotacionFirmada.FechaTopeRespuesta,
                    SolicitudVB = anotacionFirmada.SolicitudVB,
                    TipoFirma = anotacionFirmada.TipoFirma, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
                    EstadoFirma = anotacionFirmada.EstadoFirma,
                    //UserIdBorrador = userId,
                    FechaPub = anotacionFirmada.FechaPub,
                    FechaResp = anotacionFirmada.FechaResp,
                    FechaFirma = anotacionFirmada.FechaFirma,

                };

                return Ok(a);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Anotaciones/AprobarDoc")]
        [ResponseType(typeof(LOD_docAnotacionView))]
        public async Task<IHttpActionResult> AprobarDocumento(string IdDocAnotacion)
        {
            try
            {
                HelperDocumentos helper_docs = new HelperDocumentos();
                //VALIDAR QUE SE PUEDA EDITAR AL RECEPTOR (SI NO ESTA FIRMADA, ETC)
                int id = Convert.ToInt32(IdDocAnotacion);
                LOD_docAnotacion docAnotacion = await db.LOD_docAnotacion.FindAsync(id);
                LOD_Anotaciones anot = await db.LOD_Anotaciones.FindAsync(docAnotacion.IdAnotacion);
               
                if (anot != null)
                {
                    LOD_UserAnotacion userAnot = await db.LOD_UserAnotacion.Where(x => x.IdAnotacion == anot.IdAnotacion && x.EsPrincipal).FirstOrDefaultAsync();
                    string user = "";
                    if (userAnot != null)
                    {
                        user = userAnot.LOD_UsuarioLod.UserId;
                    }
                    docAnotacion.EstadoDoc = 2;
                    docAnotacion.FechaEvento = DateTime.Now;
                    docAnotacion.IdUserEvento = user;
                    db.Entry(docAnotacion).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    string tipodoc = db.MAE_TipoDocumento.Find(docAnotacion.IdTipoDoc).Tipo;
                    string accion = "Se ha aprobado el documento:" + tipodoc;
                   
                   
                    bool response = await Log_Helper.SetObjectLog(0, anot, accion, user);


                    GLOD_Notificaciones noty = new GLOD_Notificaciones();  //NOTIFICACION
                    int resu = await noty.NotificarAprobacionDoc(anot, docAnotacion, user);

                    if (docAnotacion.LOD_Anotaciones.IdTipoSub.Equals(30))
                    {
                        if (docAnotacion.IdTipoDoc.Equals(64))
                        {
                            List<FORM_InformesItems> itemsInformes = db.FORM_InformesItems.Where(x => x.IdAnotacion == docAnotacion.IdAnotacion).ToList();
                            if (itemsInformes != null)
                            {
                                foreach (var item in itemsInformes)
                                {
                                    string nombreLimpio = docAnotacion.MAE_documentos.NombreDoc.Replace("_", " ");
                                    if (nombreLimpio.Contains(item.Titulo))
                                    {
                                        item.Estado = 3;
                                        db.Entry(item).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }
                                }

                            }
                        }
                    }

                    return Ok(docAnotacion);
                }
                else
                {
                    return BadRequest("La anotación se encuentra inhabilitada para su modificación.");

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("Anotaciones/RechazarDoc")]
        [ResponseType(typeof(LOD_docAnotacionView))]
        public async Task<IHttpActionResult> RechazarDocumento(string IdDocAnotacion)
        {
            try
            {
                HelperDocumentos helper_docs = new HelperDocumentos();
                //VALIDAR QUE SE PUEDA EDITAR AL RECEPTOR (SI NO ESTA FIRMADA, ETC)
                int id = Convert.ToInt32(IdDocAnotacion);
                LOD_docAnotacion docAnotacion = await db.LOD_docAnotacion.FindAsync(id);
                LOD_Anotaciones anot = await db.LOD_Anotaciones.FindAsync(docAnotacion.IdAnotacion);

                if (anot != null)
                {
                    LOD_UserAnotacion userAnot = await db.LOD_UserAnotacion.Where(x => x.IdAnotacion == anot.IdAnotacion && x.EsPrincipal).FirstOrDefaultAsync();
                    string user = "";
                    if (userAnot != null)
                    {
                        user = userAnot.LOD_UsuarioLod.UserId;
                    }
                    docAnotacion.EstadoDoc = 3;
                    docAnotacion.FechaEvento = DateTime.Now;
                    docAnotacion.IdUserEvento = user;
                    db.Entry(docAnotacion).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    string tipodoc = db.MAE_TipoDocumento.Find(docAnotacion.IdTipoDoc).Tipo;
                    string accion = "Se ha aprobado el documento:" + tipodoc;
                    
                    bool response = await Log_Helper.SetObjectLog(0, anot, accion, user);


                    GLOD_Notificaciones noty = new GLOD_Notificaciones();  //NOTIFICACION
                    int resu = await noty.NotificarAprobacionDoc(anot, docAnotacion, user);
                    if (docAnotacion.LOD_Anotaciones.IdTipoSub.Equals(30))
                    {
                        if (docAnotacion.IdTipoDoc.Equals(64))
                        {
                            List<FORM_InformesItems> itemsInformes = db.FORM_InformesItems.Where(x => x.IdAnotacion == docAnotacion.IdAnotacion).ToList();
                            if (itemsInformes != null)
                            {
                                foreach (var item in itemsInformes)
                                {
                                    string nombreLimpio = docAnotacion.MAE_documentos.NombreDoc.Replace("_", " ");
                                    if (nombreLimpio.Contains(item.Titulo))
                                    {
                                        item.Estado = 4;
                                        db.Entry(item).State = EntityState.Modified;
                                        await db.SaveChangesAsync();
                                    }

                                }

                            }
                        }
                    }

                    return Ok(docAnotacion);
                }
                else
                {
                    return BadRequest("La anotación se encuentra inhabilitada para su modificación.");

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        
        //[HttpPost]
        //[Route("Anotaciones/TomaConocimiento")]
        //[ResponseType(typeof(LOD_AnotacionesView))]
        //public async Task<IHttpActionResult> CreateReferencia(CreateReferencia referencia)
        //{
        //    string userId = User.Identity.GetUserId();

        //    try
        //    {
        //        LOD_Anotaciones anotacionFirmada = await db.LOD_Anotaciones.FindAsync(referencia.IdAnotacion);
        //        var valid = (await UserManager.PasswordValidator.ValidateAsync(referencia.password)).Succeeded;
        //        if (valid)
        //        {
        //            var userAnotacionActual = db.LOD_UserAnotacion.Where(x => x.IdAnotacion == referencia.IdAnotacion && x.LOD_UsuarioLod.UserId == userId).FirstOrDefault();
        //            if (userAnotacionActual != null)
        //            {
        //                userAnotacionActual.FechaVB = DateTime.Now;
        //                userAnotacionActual.VistoBueno = true;
        //                userAnotacionActual.TipoVB = 3;
        //                db.Entry(userAnotacionActual).State = EntityState.Modified;
        //                await db.SaveChangesAsync();
        //            }
        //            string accion = $"{userAnotacionActual.LOD_UsuarioLod.ApplicationUser.NombreCompleto} confirma Toma de Conocimiento correctamente";
        //            bool response = await Log_Helper.SetLOGAnotacionAsync(anotacionFirmada, accion, User.Identity.GetUserId());
        //        }
        //        else
        //        {
        //            return BadRequest("El password ingresado no es válido para el usuario en sesión.");
        //        }


        //        LOD_AnotacionesView a = new LOD_AnotacionesView()
        //        {
        //            IdAnotacion = anotacionFirmada.IdAnotacion,
        //            IdTipoSub = anotacionFirmada.IdTipoSub,
        //            IdLod = anotacionFirmada.IdLod,
        //            UserIdBorrador = anotacionFirmada.UserIdBorrador,
        //            Cuerpo = anotacionFirmada.Cuerpo,
        //            Titulo = anotacionFirmada.Titulo,
        //            Correlativo = anotacionFirmada.Correlativo,
        //            EsEstructurada = false, //Depende del subtipo de anotación
        //            Estado = anotacionFirmada.Estado, //borrador
        //            FechaIngreso = anotacionFirmada.FechaIngreso,
        //            SolicitudRest = anotacionFirmada.SolicitudRest,
        //            FechaTopeRespuesta = anotacionFirmada.FechaTopeRespuesta,
        //            SolicitudVB = anotacionFirmada.SolicitudVB,
        //            TipoFirma = anotacionFirmada.TipoFirma, //depende del tipo de Usuario y su perfil, estado userFirma avanzada
        //            EstadoFirma = anotacionFirmada.EstadoFirma,
        //            //UserIdBorrador = userId,
        //            FechaPub = anotacionFirmada.FechaPub,
        //            FechaResp = anotacionFirmada.FechaResp,
        //            FechaFirma = anotacionFirmada.FechaFirma,

        //        };

        //        return Ok(a);
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }

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
