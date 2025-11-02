using AutoMapper;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.UTILITY
{
    public class ProveedorToDictionaryConverter : ITypeConverter<Proveedore, Dictionary<string, object>>
    {
        public Dictionary<string, object> Convert(Proveedore source, Dictionary<string, object> destination, ResolutionContext context)
        {
            var resultado = new Dictionary<string, object>
            {
                ["id"] = source.Id,
                ["nombre"] = source.Nombre,
                ["nit"] = source.Nit,
                ["email"] = source.Email,
                ["fechaCreacion"] = source.FechaCreacion
            };

            var camposDisponibles = context.Items["CamposDisponibles"] as List<CamposPersonalizado>;


            if (camposDisponibles != null)
            {
                foreach (var campo in camposDisponibles)
                {
                    var valor = source.ProveedorCamposValores?.FirstOrDefault(cp => cp.CampoPersonalizadoId == campo.Id);

                    resultado[campo.NombreCampo] = valor?.Valor ?? "";
                }
            }

            return resultado;
        }
    }
}
