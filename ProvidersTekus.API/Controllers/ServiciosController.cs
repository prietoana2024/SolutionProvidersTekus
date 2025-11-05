using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidersTekus.API.Utilidad;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;

namespace ProvidersTekus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class ServiciosController : ControllerBase
    {
        private readonly IServiciosService _serviciosServicio;

        public ServiciosController(IServiciosService serviciosServicio)
        {
            _serviciosServicio = serviciosServicio;
        }


        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Utilidad.Response<List<ServicioDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _serviciosServicio.Lista();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPost]
        [Route("Guardar")]

        public async Task<IActionResult> Guardar([FromBody] ServicioDTO estado)
        {
            var rsp = new Utilidad.Response<ServicioDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _serviciosServicio.Crear(estado);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ServicioDTO estado)
        {
            var rsp = new Utilidad.Response<bool>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _serviciosServicio.Editar(estado);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Utilidad.Response<bool>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _serviciosServicio.Eliminar(id);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }


    }
}
