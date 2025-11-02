using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DTO
{
    public class ServicioDTO
    {

        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal ValorHora { get; set; }

        public string Paises { get; set; } = null!;

        public int? IdProveedor { get; set; }

        public string? NombreProveedor { get; set; }

    }
}
