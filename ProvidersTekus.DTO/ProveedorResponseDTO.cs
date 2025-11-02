using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DTO
{
    public class ProveedorResponseDTO
    {
        public List<CamposPersonalizado> CamposDisponibles { get; set; }
        public List<Dictionary<string, object>> Proveedores { get; set; }
    }
}
