using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;

namespace ProvidersTekus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProveedoresController : ControllerBase
    {

        private readonly IProveedorService _service;

        public ProveedoresController(IProveedorService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<ProveedorResponseDTO>> Get()
        {
            var resultado = await _service.ObtenerTodosAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var proveedor = await _service.ObtenerPorIdAsync(id);
            if (proveedor == null) return NotFound();
            return Ok(proveedor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProveedorDTO dto)
        {
            var proveedor = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, proveedor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProveedorDTO dto)
        {
            var actualizado = await _service.ActualizarAsync(id, dto);
            if (!actualizado) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var eliminado = await _service.EliminarAsync(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }


    }
}
