using AutoMapper;
using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.UTILITY
{
    public class CamposPersonalizadosResolver
    : IValueResolver<Proveedore, ProveedorResponseDTO, Dictionary<string, string>>
    {
        public Dictionary<string, string> Resolve(
            Proveedore source,
            ProveedorResponseDTO destination,
            Dictionary<string, string> destMember,
            ResolutionContext context)
        {
            if (source.ProveedorCamposValores == null || !source.ProveedorCamposValores.Any())
                return new Dictionary<string, string>();

            return source.ProveedorCamposValores
                .ToDictionary(
                    v => v.CampoPersonalizado.NombreCampo,
                    v => v.Valor ?? string.Empty
                );
        }
    }
}
