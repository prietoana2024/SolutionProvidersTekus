using Microsoft.EntityFrameworkCore;
using ProvidersTekus.DAL.DBContext;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DLL.Services
{
    public class CampoPersonalizadoService:ICampoPersonalizadoService
    {
        private readonly BdprovidersContext _context;

        public CampoPersonalizadoService(BdprovidersContext context)
        {
            _context = context;
        }

        public async  Task<CamposPersonalizado> CrearAsync(CrearCampoDTO dto)
        {
            var existe = await _context.CamposPersonalizados
                       .AnyAsync(c => c.NombreCampo == dto.NombreCampo);

            if (existe)
                throw new InvalidOperationException($"El campo '{dto.NombreCampo}' ya existe");

            var ultimoOrden = await _context.CamposPersonalizados
                .MaxAsync(c => (int?)c.Orden) ?? 0;

            var campo = new CamposPersonalizado
            {
                NombreCampo = dto.NombreCampo.ToLower().Replace(" ", "_"),
                Etiqueta = dto.Etiqueta,
                TipoDato = dto.TipoDato,
                Requerido = dto.Requerido,
                Orden = ultimoOrden + 1,
                Activo = true,
                FechaCreacion = DateTime.UtcNow
            };

            _context.CamposPersonalizados.Add(campo);
            await _context.SaveChangesAsync();

            return campo;
        }

        public async  Task<bool> EliminarAsync(int id)
        {
            var campo = await _context.CamposPersonalizados.FindAsync(id);
            if (campo == null) return false;

            // Eliminar valores asociados
            var valores = _context.ProveedorCamposValores
                .Where(v => v.CampoPersonalizadoId == id);
            _context.ProveedorCamposValores.RemoveRange(valores);

            _context.CamposPersonalizados.Remove(campo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CamposPersonalizado>> ObtenerTodosAsync()
        {
            return await _context.CamposPersonalizados
              .Where(c => c.Activo)
              .OrderBy(c => c.Orden)
              .Include(c => c.ProveedorCamposValores)
              .ToListAsync();
        }
    }
}
