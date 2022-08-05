
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LODApi.ModelsView
{
    public class MAE_SubtipoComunicacionView
    {
        public int IdTipoSub { get; set; }
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int IdTipoCom { get; set; }

        public string TipoComunicacion { get; set; }


        public bool Activo { get; set; }

    }
}