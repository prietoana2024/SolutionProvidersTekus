using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DLL.Services.Contrato
{
    public interface IServiciosService
    {

        Task<List<ServicioDTO>> Lista();
        Task<ServicioDTO> Crear(ServicioDTO modelo);
        Task<bool> Editar(ServicioDTO modelo);
        Task<bool> Eliminar(int id);


    }
}
