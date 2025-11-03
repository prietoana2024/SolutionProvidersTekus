using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProvidersTekus.DAL.DBContext;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;
using ProvidersTekus.DTO.Variables;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = ProvidersTekus.DTO.Variables.Procedures;
using Dapper;



namespace ProvidersTekus.DLL.Services
{
    public class ProveedorService:IProveedorService
    {

        private readonly BdprovidersContext _context;
        private readonly IMapper _mapper;
        private readonly string? _dataBase;


        public ProveedorService(BdprovidersContext context, IMapper mapper, IConfiguration configuration)
        {
            _dataBase = configuration.GetConnectionString(AppSettings.DB_CONNECTION);

            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ActualizarAsync(int id, ProveedorDTO dto)
        {
            var proveedor = await _context.Proveedores
             .Include(p => p.ProveedorCamposValores)
             .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null) return false;

            proveedor.Nit=dto.Nit;
          //  proveedor.ProveedorServicios = dto.Servicios
            proveedor.Nombre = dto.Nombre;
            proveedor.Email = dto.Email;

            // Actualizar campos personalizados
            await ActualizarCamposPersonalizadosAsync(proveedor, dto.CamposPersonalizados);
            await _context.SaveChangesAsync();

            return true;
        }


        private async Task ActualizarCamposPersonalizadosAsync(
            Proveedore proveedor,
            Dictionary<string, string> campos)
        {
            if (campos == null) return;

            var camposDefinidos = await _context.CamposPersonalizados.ToListAsync();

            foreach (var campo in camposDefinidos)
            {
                if (campos.ContainsKey(campo.NombreCampo))
                {
                    var valorExistente = proveedor.ProveedorCamposValores
                        .FirstOrDefault(v => v.CampoPersonalizadoId == campo.Id);

                    if (valorExistente != null)
                    {
                        valorExistente.Valor = campos[campo.NombreCampo] ?? "";
                    }
                    else
                    {
                        _context.ProveedorCamposValores.Add(new ProveedorCamposValore
                        {
                            ProveedorId = proveedor.Id,
                            CampoPersonalizadoId = campo.Id,
                            Valor = campos[campo.NombreCampo] ?? ""
                        });
                    }
                }
            }
        }
        private async Task GuardarCamposPersonalizadosAsync(int proveedorId, Dictionary<string, string> campos)
        {
            if (campos == null) return;

            var camposDefinidos = await _context.CamposPersonalizados.ToListAsync();

            foreach (var campo in camposDefinidos)
            {
                if (campos.ContainsKey(campo.NombreCampo))
                {
                    _context.ProveedorCamposValores.Add(new ProveedorCamposValore
                    {
                        ProveedorId = proveedorId,
                        CampoPersonalizadoId = campo.Id,
                        Valor = campos[campo.NombreCampo] ?? ""
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task<Proveedore> CrearAsync(ProveedorDTO dto)
        {
            var proveedor = new Proveedore
            {
                Nit=dto.Nit,
                Nombre = dto.Nombre,
             //   ProveedorServicios=dto.Servicios,
                Email = dto.Email,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            // Guardar campos personalizados
            await GuardarCamposPersonalizadosAsync(proveedor.Id, dto.CamposPersonalizados);

            return proveedor;
        }

        public async Task<bool> EliminarAsync(int id)
        {

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Dictionary<string, object>> ObtenerPorIdAsync(int id)
        {
            var proveedor = await _context.Proveedores
                       .Include(p => p.ProveedorCamposValores)
                       .ThenInclude(v => v.CampoPersonalizado)
                       .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null) return null;

            var camposDisponibles = await _context.CamposPersonalizados
                .Where(c => c.Activo)
                .OrderBy(c => c.Orden)
                .ToListAsync();

            return _mapper.Map<Dictionary<string,object>>(proveedor, opts=>
            {
                opts.Items["CamposDisponibles"] = camposDisponibles;
            });
        }

        public async Task<ProveedorResponseDTO> ObtenerTodosAsync()
        {
            var proveedores = await _context.Proveedores
                .AsNoTracking()
                      .Include(p => p.ProveedorCamposValores)
                      .ThenInclude(v => v.CampoPersonalizado)
                      .ToListAsync();

            var camposDisponibles = await _context.CamposPersonalizados
                 .AsNoTracking()
                .Where(c => c.Activo)
                .OrderBy(c => c.Orden)
                .ToListAsync();

            var resultado = proveedores.Select(p => _mapper.Map<Dictionary<string,object>>(p, opts =>
            {
                opts.Items["CamposDisponibles"]=camposDisponibles;
            })).ToList();

            return new ProveedorResponseDTO
            {
                CamposDisponibles = camposDisponibles,
                Proveedores = resultado
            };
        }

        public async Task<List<ProveedorDTO>> ProviderForCountries()
        {
            
            using var conn = new SqlConnection(_dataBase);
            var providers = (await conn.QueryAsync<Proveedore>(SP.SP_COUNT_CLIENTS_FOR_COUNTRIES, commandType: CommandType.StoredProcedure)).FirstOrDefault();

            await conn.CloseAsync();
            await conn.DisposeAsync();

            return _mapper.Map<List<ProveedorDTO>>(providers);
        }
    }
}
