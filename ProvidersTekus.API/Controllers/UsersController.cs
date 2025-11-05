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


    public class UsersController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServicio;

        public UsersController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Utilidad.Response<List<UsuarioDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _usuarioServicio.Lista();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        //[HttpPost]
        //[Route("Guardar")]

        //public async Task<IActionResult> Guardar([FromBody] UsuarioDTO estado)
        //{
        //    var rsp = new Utilidad.Response<UsuarioDTO>();

        //    try
        //    {
        //        rsp.Status = true;
        //        rsp.Value = await _usuarioServicio.Crear(estado);
        //    }

        //    catch (Exception ex)
        //    {
        //        rsp.Status = false;
        //        rsp.Msg = ex.Message;
        //    }
        //    //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
        //    return Ok(rsp);
        //}
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO estado)
        {
            var rsp = new Utilidad.Response<bool>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _usuarioServicio.Editar(estado);
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
                rsp.Value = await _usuarioServicio.Eliminar(id);
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
