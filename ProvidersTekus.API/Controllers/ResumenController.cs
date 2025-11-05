using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;

namespace ProvidersTekus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Authorize]

    public class ResumenController : ControllerBase
    {
        private readonly IServiciosService _serviciosServicio;
        private readonly IProveedorService _proveedorServicio;

        public ResumenController(IServiciosService serviciosServicio, IProveedorService proveedorServicio)
        {
            _serviciosServicio = serviciosServicio;
            _proveedorServicio = proveedorServicio;
        }


        [HttpGet]
        [Route("ServicesForContries")]

        public async Task<IActionResult> ServicesForContries()
        {
            var rsp = new Utilidad.Response<List<CountServicesDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _serviciosServicio.ServicesForCountries();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }


        [HttpGet]
        [Route("ProvidersForContries")]

        public async Task<IActionResult> ProvidersForContries()
        {
            var rsp = new Utilidad.Response<List<CountProvidersForCountriesDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _proveedorServicio.ProviderForCountries();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            return Ok(rsp);
        }

    }
}
