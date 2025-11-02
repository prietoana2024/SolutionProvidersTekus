using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DTO
{
    public class ProveedorDTO
    {

        public int? Id { get; set; }

        public string Nit { get; set; }

        public string Nombre { get; set; }
        public string Email { get; set; }
        public Dictionary<string, string> CamposPersonalizados { get; set; }

    }
}
