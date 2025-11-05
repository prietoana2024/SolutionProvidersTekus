using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DLL.Services.Contrato
{
    public interface IProveedorService
    {
        Task<List<ProveedorResponseDTO>> ObtenerTodosAsync();
        Task<ProveedorResponseDTO> ObtenerPorIdAsync(int id);
        Task<Proveedore> CrearAsync(ProveedorDTO dto);
        Task<bool> ActualizarAsync(int id, ProveedorDTO dto);
        Task<bool> EliminarAsync(int id);


        Task<List<CountProvidersForCountriesDTO>> ProviderForCountries();

    }
}
