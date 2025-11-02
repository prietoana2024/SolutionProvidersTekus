using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DTO
{
    public class CrearCampoDTO
    {

        [Required]
        public string NombreCampo { get; set; }

        [Required]
        public string Etiqueta { get; set; }

        [Required]
        public string TipoDato { get; set; } // texto, numero, email, fecha

        public bool Requerido { get; set; }
    }
}
