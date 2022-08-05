using ACAMicroFramework.Archivos;
using LODApi.Helpers;
using LODApi.Models;
using LODApi.ModelsViews;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace LOD_APR.Helpers
{
    public class HelperDocumentos
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public string SaveFileToDisk(int tipoObjeto,object objeto, HttpPostedFileBase FileToSave, string PathToSave, string rutaSave, string PreName, MAE_documentos mAE_documentos)
        {
            string fileExt = Path.GetExtension(FileToSave.FileName);
            string Name = MakeValidFileName(String.Format("{0}{1}{2}", PreName, mAE_documentos.NombreDoc, fileExt));
            string rutaWebApi = HostingEnvironment.ApplicationPhysicalPath;
            string auxPath = HttpContext.Current.Server.MapPath(PathToSave);
            string path = auxPath.Replace("LODApi", "LOD_APR");
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string estatus = "";
            File_Result save_result = ACA_Archivos.saveFile(FileToSave, path, Name, true);

            if (save_result.Status)
            {
                try
                {
                    mAE_documentos.NombreDoc = save_result.FileName;
                    mAE_documentos.Mb = save_result.FileSize.MegaBytes;
                    mAE_documentos.Extension = save_result.FileExt;
                    mAE_documentos.ContentType = FileToSave.ContentType;
                    mAE_documentos.FechaCreacion = DateTime.Now;
                    mAE_documentos.Ruta = rutaSave + Name;
                    db.MAE_documentos.Add(mAE_documentos);
                    db.SaveChanges();

                    switch (tipoObjeto)
                    {
                        case 0:
                            //LOD_docAnotacion lOD_docAnotacion = (LOD_docAnotacion) objeto;
                            //lOD_docAnotacion.MAE_documentos = mAE_documentos;
                            //lOD_docAnotacion.EstadoDoc = 1;
                            //db.LOD_docAnotacion.Add(lOD_docAnotacion);
                            //db.SaveChanges();
                            break;
                        case 1:
                            AddOtroDocumentoView newOtroDoc = (AddOtroDocumentoView)objeto;
                            LOD_docAnotacion otros_docs = new LOD_docAnotacion()
                            {
                                IdDoc = mAE_documentos.IdDoc,
                                IdTipoDoc = newOtroDoc.IdTipoDoc,
                                IdAnotacion = newOtroDoc.IdAnotacion,
                                EstadoDoc = 1,
                                MAE_documentos = mAE_documentos,
                                IdContrato = newOtroDoc.IdContrato
                            };
                            db.LOD_docAnotacion.Add(otros_docs);
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }


                    db.SaveChanges();
                    estatus = $"Ok;{mAE_documentos.IdDoc}";
                }
                catch (Exception ex)
                {
                    estatus = String.Format("Ocurrió un problema al tratar de guardar el archivo: {0}", ex.Message);
                }

            }
            else
            {
                estatus = save_result.ErrorMnsg;
            }
            
            return estatus;

        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            //name = name;
            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }
        //private static string sanitizePath(string path)
        //{
        //    string message = path;

        //    try
        //    {

        //        message = message.Replace("á", "a");
        //        message = message.Replace("é", "e");
        //        message = message.Replace("í", "i");
        //        message = message.Replace("ó", "o");
        //        message = message.Replace("ú", "u");

        //        message = message.Replace("Á", "A");
        //        message = message.Replace("É", "E");
        //        message = message.Replace("Í", "I");
        //        message = message.Replace("Ó", "O");
        //        message = message.Replace("Ú", "U");

        //        message = message.Replace("ñ", "n");
        //        message = message.Replace("Ñ", "N");

        //    }
        //    catch (Exception e)
        //    {
        //        return path;
        //    }

        //    return message;
        //}
        //public Status_Error SaveFileToDisk(HttpPostedFileBase FileToSave, MAE_documentos mAE_documentos, DatosDocumento data)
        //{

        //    Status_Error estatus = new Status_Error() { ErrorCode = 0, ErrorMnsg = "", Error = false };
        //    string fileExt = Path.GetExtension(FileToSave.FileName);
        //    string Name = String.Format("{0}_{1}{2}", data.PreName, mAE_documentos.NombreDoc, fileExt);

        //    File_Result save_result = ACA_Archivos.saveFile(FileToSave, data.ServerPath, Name, true);

        //    if (save_result.Status)
        //    {
        //        try
        //        {
        //            mAE_documentos.NombreDoc = data.DataBasePath + save_result.FileName;
        //            mAE_documentos.Mb = save_result.FileSize.MegaBytes;
        //            mAE_documentos.Extension = save_result.FileExt;
        //            mAE_documentos.FechaCreacion = DateTime.Now;
        //            db.MAE_documentos.Add(mAE_documentos);
        //            db.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {
        //            estatus.Error = true;
        //            estatus.ErrorMnsg = String.Format("Ocurrió un problema al tratar de guardar el archivo: {0}", ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        estatus.Error = true;
        //        estatus.ErrorMnsg = save_result.ErrorMnsg;
        //    }

        //    return estatus;

        //}
        //public Status_Error ZipFilesMultiPath(List<DocZip> lstDocs, string PreName)
        //{
        //    List<int> LstIdsDoc = new List<int>();
        //    foreach (DocZip item in lstDocs)
        //    {
        //        LstIdsDoc.Add(item.IdDoc);
        //    }

        //    Status_Error estatus = new Status_Error() { ErrorCode = 0, ErrorMnsg = "", Error = false };
        //    List<MAE_documentos> lstDcos = db.MAE_documentos.Where(i => LstIdsDoc.Contains(i.IdDoc)).ToList();


        //    string pathZipFolder = HttpContext.Current.Server.MapPath("~/Files/System/Zip/");
        //    string formatoFechaHora = DateTime.Now.Year.ToString() +
        //        DateTime.Now.Month.ToString() +
        //        DateTime.Now.Day.ToString() +
        //        DateTime.Now.Hour.ToString() +
        //        DateTime.Now.Minute.ToString() +
        //        DateTime.Now.Second.ToString();
        //    ArrayList filesPath = new ArrayList();

        //    foreach (MAE_documentos doc in lstDcos)
        //    {
        //        string PathToRead = lstDocs.Where(x => x.IdDoc == doc.IdDoc).First().PathToRead;

        //        string tempPath = Path.Combine(HttpContext.Current.Server.MapPath(PathToRead), doc.NombreDoc);
        //        filesPath.Add(tempPath);
        //    }

        //    try
        //    {
        //        string zipFileName = String.Format("Doc_{0}_{1}.zip", PreName, formatoFechaHora);
        //        string zipPath = Path.Combine(pathZipFolder, zipFileName);
        //        var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);

        //        foreach (string archivo in filesPath)
        //            zip.CreateEntryFromFile(archivo, Path.GetFileName(archivo), CompressionLevel.Optimal);

        //        zip.Dispose();

        //        estatus.Status = @"true;/Files/System/Zip/" + zipFileName;

        //    }
        //    catch (IOException ex)
        //    {
        //        estatus.ErrorMnsg = "false;" + ex.Message;
        //        estatus.Error = true;
        //    }

        //    return estatus;

        //}
        //public Status_Error ZipFiles(List<int> lstDocs, string PathToRead, string PreName)
        //{

        //    Status_Error estatus = new Status_Error() { ErrorCode = 0, ErrorMnsg = "", Error = false };
        //    List<MAE_documentos> lstDcos = db.MAE_documentos.Where(i => lstDocs.Contains(i.IdDoc)).ToList();

        //    string pathZipFolder = HttpContext.Current.Server.MapPath("~/Files/System/Zip/");
        //    string formatoFechaHora = DateTime.Now.Year.ToString() +
        //        DateTime.Now.Month.ToString() +
        //        DateTime.Now.Day.ToString() +
        //        DateTime.Now.Hour.ToString() +
        //        DateTime.Now.Minute.ToString() +
        //        DateTime.Now.Second.ToString();
        //    ArrayList filesPath = new ArrayList();

        //    foreach (MAE_documentos doc in lstDcos)
        //    {
        //        string tempPath = Path.Combine(HttpContext.Current.Server.MapPath(PathToRead), doc.NombreDoc);
        //        filesPath.Add(tempPath);
        //    }

        //    try
        //    {
        //        string zipFileName = String.Format("Doc_{0}_{1}.zip", PreName, formatoFechaHora);
        //        string zipPath = Path.Combine(pathZipFolder, zipFileName);
        //        var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);

        //        foreach (string archivo in filesPath)
        //            zip.CreateEntryFromFile(archivo, Path.GetFileName(archivo), CompressionLevel.Optimal);

        //        zip.Dispose();

        //        estatus.Status = @"true;/Files/System/Zip/" + zipFileName;

        //    }
        //    catch (IOException ex)
        //    {
        //        estatus.ErrorMnsg = "false;" + ex.Message;
        //        estatus.Error = true;
        //    }

        //    return estatus;

        //}
    }
}