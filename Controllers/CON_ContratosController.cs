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
using LODApi.Areas.GLOD.Models;
using LODApi.Models;

namespace LODApi.Controllers
{
    public class CON_ContratosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpGet]
        [Route("Contratos/FindByUser/{UserId}")]
        [ResponseType(typeof(List<ContratosSelectView>))]
        public async Task<IHttpActionResult> Contratos(string UserId)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            List<CON_ContratosView> contratos = new List<CON_ContratosView>();
            var direAux = await db.LOD_UsuariosLod.Where(x => x.UserId == UserId && x.Activo && x.LOD_LibroObras.CON_Contratos.EstadoContrato == 1).Select(x => x.LOD_LibroObras.CON_Contratos).ToListAsync();
            var dire = new List<CON_Contratos>();
            foreach (var item in direAux)
            {
                if(!(dire.Select(x => x.IdContrato).Contains(item.IdContrato)))
                {
                    dire.Add(item);
                }
            }


            dire.ForEach(
        
            x => contratos.Add(new CON_ContratosView()
            {
                IdContrato = x.IdContrato,
                CodigoContrato = x.CodigoContrato,
                Activo = x.Activo,
                DescripcionContrato = x.DescripcionContrato,
                Empresa_Contratista = x.Empresa_Contratista.RazonSocial,
                Empresa_Fiscalizadora = x.Empresa_Fiscalizadora.RazonSocial,
                UserId = x.UserId,
                UsuarioCreador = x.Creador,
                IdDireccionContrato = x.IdDireccionContrato,
                EstadoContrato = x.EstadoContrato,
                FechaAdjudicacion = x.FechaAdjudicacion,
                FechaCreacionContrato = x.FechaCreacionContrato,
                FechaInicioContrato = x.FechaInicioContrato,
                MontoInicialContrato = x.MontoInicialContrato,
                NombreContrato = x.NombreContrato,
                PlazoInicialContrato = x.PlazoInicialContrato,
                RutaImagenContrato = x.RutaImagenContrato,
                DireccionMOP = x.NombreDireccion,
                Sucursal = x.MAE_Sucursal.Sucursal
            }));

            return Ok(contratos);
        }

        [HttpGet]
        [Route("Contratos/Find/{IdContrato}")]
        [ResponseType(typeof(List<CON_ContratosView>))]
        public async Task<IHttpActionResult> Contratos(int IdContrato)
        {
            //string userId = User.Identity.GetUserId();
            //string empresaUser = db.Users.Find(userId).IdEmpresa;

            var x = await db.CON_Contratos.FindAsync(IdContrato);

            CON_ContratosView contrato = new CON_ContratosView()
            {
                IdContrato = x.IdContrato,
                CodigoContrato = x.CodigoContrato,
                Activo = x.Activo,
                DescripcionContrato = x.DescripcionContrato,
                Empresa_Contratista = x.Empresa_Contratista.RazonSocial,
                Empresa_Fiscalizadora = x.Empresa_Fiscalizadora.RazonSocial,
                IdDireccionContrato = x.IdDireccionContrato,
                EstadoContrato = x.EstadoContrato,
                FechaAdjudicacion = x.FechaAdjudicacion,
                FechaCreacionContrato = x.FechaCreacionContrato,
                FechaInicioContrato = x.FechaInicioContrato,
                MontoInicialContrato = x.MontoInicialContrato,
                NombreContrato = x.NombreContrato,
                PlazoInicialContrato = x.PlazoInicialContrato,
                RutaImagenContrato = x.RutaImagenContrato,
                DireccionMOP = x.NombreDireccion
                
            };

            return Ok(contrato);
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
