using LODApi.Models;
using MessagingToolkit.QRCode.Codec;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static LODApi.Models.AuxiliaresReport;

namespace LODApi.Helpers
{
    public class FirmarAnotacion
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<bool> FirmarAnotacionDB(int id, int tipo, string user)
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
                anot.UserId =user;
                db.Entry(anot).State = EntityState.Modified;
                await db.SaveChangesAsync();
   

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
        public async Task<string> GeneratePDF(int id)
        {
            List<ReportParameter> parametros = new List<ReportParameter>();

            LOD_Anotaciones anotacion = await db.LOD_Anotaciones.FindAsync(id);
            AnotacionHelper anothelp = new AnotacionHelper();
            AnotacionView anot = await anothelp.GetAnotacionData(id);
            string rutaWebApp= Convert.ToString(ConfigurationManager.AppSettings.Get("web_app_url"));
            //anot.Remitente.ImgRemitente = (anot.Remitente.ImgRemitente != "") ? HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + anot.Remitente.ImgRemitente : "";
            anot.Remitente.ImgRemitente = (anot.Remitente.ImgRemitente != "") ? rutaWebApp + anot.Remitente.ImgRemitente : "";
            anot.QR = QRBinary(anotacion.Correlativo, anotacion.IdLod, anotacion.LOD_LibroObras.IdContrato);

            List<AnotacionView> lstAnot = new List<AnotacionView>();
            lstAnot.Add(anot);

            List<Remitente> lstRem = new List<Remitente>();
            lstRem.Add(anot.Remitente);

            List<EstadoFirma> lstFirm = new List<EstadoFirma>();
            lstFirm.Add(anot.EstadoFirma);

            List<EstadoAnotacion> lstEstado = new List<EstadoAnotacion>();
            lstEstado.Add(anot.EstadoAnotacion);

            List<ReportDataSource> dsources = new List<ReportDataSource>();
            dsources.Add(new ReportDataSource("DsAnotacion", lstAnot));
            dsources.Add(new ReportDataSource("DSRemitente", lstRem));
            dsources.Add(new ReportDataSource("DSReceptor", anot.Receptores));
            dsources.Add(new ReportDataSource("DSEstadoFirma", lstFirm));
            dsources.Add(new ReportDataSource("DSEstadoAnotacion", lstEstado));
            dsources.Add(new ReportDataSource("DSEstadoRespuesta", new List<EstadoRespuesta>()));
            dsources.Add(new ReportDataSource("DSReferencias", anot.Referencias));
            dsources.Add(new ReportDataSource("DSAdjuntos", anot.Adjuntos));

            try
            {

                var informePDF = GenerarReporte("PDF", "rptAnotacionLod.rdlc", dsources, parametros, anotacion.LOD_LibroObras.CON_Contratos.IdEmpresaContratista);
                if (informePDF == null)
                {
                    return string.Empty;
                }
                else
                {

                }

                MemoryStream ms = new MemoryStream(informePDF.RenderedBytes);
                string tempPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), anotacion.RutaCarpetaPdf);
                tempPath = tempPath.Replace("LODApi", "LOD_APR");
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);

                string informeTempPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), anotacion.RutaPdfSinFirma);
                informeTempPath = informeTempPath.Replace("LODApi", "LOD_APR");

                FileStream newFile = new FileStream(informeTempPath, FileMode.Create, FileAccess.Write);
                ms.WriteTo(newFile);
                newFile.Close();

                return informeTempPath;
            }
            catch
            {
                return string.Empty;
            }


        }
        public ReportResult GenerarReporte(string tipo, string nombreArchivoRpt, List<ReportDataSource> dataSources, List<ReportParameter> parametros, int? IdEmpresa)
        {
            var empresa = db.MAE_sujetoEconomico.Find(IdEmpresa);
            ReportResult result = new ReportResult();

            List<DsEmpresa> lstEmpresa = new List<DsEmpresa>();
            DsEmpresa emp = new DsEmpresa();
            emp.Razon = empresa.RazonSocial;
            emp.Direccion = empresa.Direccion;
            emp.Web = empresa.web;
            emp.Telefono = empresa.Telefono;
            if (empresa.RutaImagen != null)
            {
                string rutaWebApp = Convert.ToString(ConfigurationManager.AppSettings.Get("web_app_url"));
                //emp.Logo = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/Images/Sujetos/" + empresa.RutaImagen;
                emp.Logo = rutaWebApp+ "/Images/Sujetos/" + empresa.RutaImagen;
            }
            emp.Email = empresa.email;
            lstEmpresa.Add(emp);

            //LISTADO DE VISTAS DEL INFORME
            //PDF
            //Excel
            //Word
            //Image

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            lr.EnableHyperlinks = true;
            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Areas/GLOD/Reports"), nombreArchivoRpt);
            path = path.Replace("LODApi", "LOD_APR");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return null;
            }

            //SE AGREGA EL DATASOURCE DE LOS DATOS DE LA EMPRESA
            lr.DataSources.Add(new ReportDataSource("DSEmpresa", lstEmpresa));
            //SE AGREGAN LOS DATASOURCES ESPECIFICOS DEL REPORTE
            foreach (ReportDataSource ds in dataSources)
                lr.DataSources.Add(ds);

            //SE AGREGA EL LISTADO DE PARAMETROS ESPECIFICOS POR CADA REPORTE
            if (parametros != null)
                lr.SetParameters(parametros);

            lr.Refresh();

            string reportType = tipo;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
            "  <OutputFormat>" + tipo + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.3937in</MarginTop>" +
            "  <MarginLeft>0.7874in</MarginLeft>" +
            "  <MarginRight>0.7874in</MarginRight>" +
            "  <MarginBottom>0.3937in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            try
            {
                renderedBytes = lr.Render(
                               reportType,
                               deviceInfo,
                               out mimeType,
                               out encoding,
                               out fileNameExtension,
                               out streams,
                               out warnings);
                result = new ReportResult() { RenderedBytes = renderedBytes, MimeType = mimeType };
                return result;
            }
            catch (Exception Ex)
            {
                return null;
            }

        }
        public async Task<string> PathDescargarAnotacion(int id)
        {
            LOD_Anotaciones anotacion = await db.LOD_Anotaciones.FindAsync(id);
            string tempPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), anotacion.RutaPdfConFirma);
            tempPath = tempPath.Replace("LODApi", "LOD_APR");
            return tempPath;
        }
        public byte[] QRBinary(int Folio, int libro, int cont)
        {
            string rutaWebApp = Convert.ToString(ConfigurationManager.AppSettings.Get("web_app_url"));
            //string ruta = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            string ruta = rutaWebApp;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            String encoding = "Byte";
            if (encoding == "Byte")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            }
            else if (encoding == "AlphaNumeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;
            }
            else if (encoding == "Numeric")
            {
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.NUMERIC;
            }
            try
            {
                int scale = Convert.ToInt16(4);
                qrCodeEncoder.QRCodeScale = scale;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Invalid size!");
                //return;
            }
            try
            {
                int version = Convert.ToInt16(7);
                qrCodeEncoder.QRCodeVersion = version;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Invalid version !");
            }

            string errorCorrect = "Q";
            if (errorCorrect == "L")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            else if (errorCorrect == "M")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            else if (errorCorrect == "Q")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            else if (errorCorrect == "H")
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;

            Image image;
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
            String data = String.Format("{0}/Pub/ValidarFolio?folio={1}&ldo={2}&cont={3}", ruta, Folio, libro, cont);
            image = qrCodeEncoder.Encode(data);

            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                return imageBytes;
            }


        }
        public async Task<string> AnotacionPDFToBase64(string path)
        {
            try
            {
                byte[] bytes;
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                    bytes = memory.ToArray();
                }

                string base64 = Convert.ToBase64String(bytes);
                return base64;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public class ReportResult
        {
            public byte[] RenderedBytes { get; set; }
            public string MimeType { get; set; }
        }
    }

    public class AnotacionView
    {
        public int IdAnotacion { get; set; }

        public string Folio { get; set; }

        public string Titulo { get; set; }
        public string Cuerpo { get; set; }

        public string LibroObras { get; set; }
        public int IdLibro { get; set; }

        public string Tipo { get; set; }

        public string Subtipo { get; set; }

        public int IdTipoSub { get; set; }

        public bool EsEstructurada { get; set; }
        public bool SolicitudRest { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaRespuesta { get; set; }
        public string FechaTopeRespuesta { get; set; }
        public string UserBorrador { get; set; }

        public byte[] QR { get; set; }

        //DATOS CONTRACTUALES   
        public string RutContratista { get; set; }
        public string Contratista { get; set; }
        public string Contrato { get; set; }
        public string LibroDeObras { get; set; }

        public UsuarioActual UsuarioActual { get; set; }

        public Remitente Remitente { get; set; }

        public List<Receptor> Receptores { get; set; }

        public EstadoFirma EstadoFirma { get; set; }

        public EstadoAnotacion EstadoAnotacion { get; set; }

        public EstadoRespuesta EstadoRespuesta { get; set; }

        public List<Referencias> Referencias { get; set; }

        public List<Adjuntos> Adjuntos { get; set; }

    }

    public class Remitente
    {
        public string Nombre { get; set; }
        public string ImgRemitente { get; set; }
        public string InicialesRemitente { get; set; }
        public string Cargo { get; set; }
        public string Empresa { get; set; }
    }
    public class Receptor
    {
        public int IdAnotacion { get; set; }
        public int IdReceptor { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Imagen { get; set; }
        public string Iniciales { get; set; }
        public bool RespRespuesta { get; set; }
        public bool EsPrincipal { get; set; }
        public bool ReqVB { get; set; }
        public bool VistoBueno { get; set; }
        public string FechaVB { get; set; }
        public int TipoFirma { get; set; }
        public string TipoFirmaDetalle
        {
            get
            {
                string tipo = string.Empty;
                if (this.TipoFirma == 1)
                {
                    tipo = "Firma Electrónica Avanzada";
                }
                else if (this.TipoFirma == 2)
                {
                    tipo = "Firma MOP";
                }
                else if (this.TipoFirma == 3)
                {
                    tipo = "Firma Electrónica Simple";
                }
                else
                {
                    tipo = "Otro tipo de Firma";
                }
                return tipo;
            }
        }
    }
    public class EstadoFirma
    {
        public bool IsFirmada { get; set; }
        public int IdTipo { get; set; }
        public string UsuarioFirma { get; set; }
        public string FechaFirma { get; set; }
        public string TipoFirma
        {
            get
            {
                string tipo = string.Empty;
                if (this.IdTipo == 1)
                {
                    tipo = "Firma Electrónica Avanzada";
                }
                else if (this.IdTipo == 2)
                {
                    tipo = "Firma MOP";
                }
                else if (this.IdTipo == 3)
                {
                    tipo = "Firma Electrónica Simple";
                }
                else
                {
                    tipo = "Otro tipo de Firma";
                }
                return tipo;
            }
        }
        public string claseFirma
        {
            get
            {
                return (this.IsFirmada) ? "success" : "muted";
            }
        }
    }
    public class EstadoAnotacion
    {
        public int IdEstado { get; set; }
        public string Descripcion
        {
            get
            {
                string tipo = string.Empty;
                if (this.IdEstado == 0)
                {
                    tipo = "Borrador";
                }
                else if (this.IdEstado == 1)
                {
                    tipo = "Solicitud de Firma";
                }
                else if (this.IdEstado == 2)
                {
                    tipo = "Publicada";
                }
                else
                {
                    tipo = "Firma Pendiente";
                }
                return tipo;
            }
        }
        public string claseEstado
        {
            get
            {
                string clase = string.Empty;
                if (this.IdEstado == 0)
                {
                    clase = "muted";
                }
                else if (this.IdEstado == 1)
                {
                    clase = "warning";
                }
                else if (this.IdEstado == 2)
                {
                    clase = "success";
                }
                else
                {
                    clase = "muted";
                }
                return clase;
            }
        }

    }
    public class EstadoRespuesta
    {
        public bool Respondida { get; set; }
        public string FechaRespuesta { get; set; }
        public string FolioRespuesta { get; set; }
        public string UsuarioRespuesta { get; set; }
        public string TituloRespuesta { get; set; }
        public int IdRespuesta { get; set; }
        public string Descripcion
        {
            get
            {
                string desc = string.Empty;
                if (!this.Respondida)
                {
                    desc = "Pendiente Respuesta";
                }
                else
                {
                    desc = "Respondida";
                }
                return desc;
            }
        }
    }
    public class UsuarioActual
    {
        public bool EsDestacada { get; set; }
        public bool DebeResponder { get; set; }
        public bool DebeDarVistoBueno { get; set; }
    }
    public class Referencias
    {
        public int IdRefAnot { get; set; }
        public int IdAnotacion { get; set; }
        public string Libro { get; set; }
        public string Folio { get; set; }
        public string Anotacion { get; set; }
        public string Remitente { get; set; }
        public string Fecha { get; set; }
        public string Observacion { get; set; }
    }
    public class Adjuntos
    {
        public string Tipo { get; set; }
        public string Subtipo { get; set; }
        public string Documento { get; set; }
    }
}