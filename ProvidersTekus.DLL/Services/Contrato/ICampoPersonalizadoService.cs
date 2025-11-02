using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DLL.Services.Contrato
{
    public interface ICampoPersonalizadoService
    {

        Task<List<CamposPersonalizado>> ObtenerTodosAsync();
        Task<CamposPersonalizado> CrearAsync(CrearCampoDTO dto);
        Task<bool> EliminarAsync(int id);



    }
}
