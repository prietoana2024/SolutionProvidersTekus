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
            if (campos == null || !campos.Any())
                return;

            var camposDefinidos = await _context.CamposPersonalizados.ToListAsync();

            foreach (var kvp in campos)
            {
                var nombreCampo = kvp.Key.Trim();
                var valorCampo = kvp.Value ?? string.Empty;

                var campoExistente = camposDefinidos
                    .FirstOrDefault(c => c.NombreCampo.Equals(nombreCampo, StringComparison.OrdinalIgnoreCase));

                if (campoExistente == null)
                {
                    campoExistente = new CamposPersonalizado
                    {
                        NombreCampo = nombreCampo,
                        Etiqueta = nombreCampo,  
                        TipoDato = "Texto",             
                        Activo = true,
                        Orden = camposDefinidos.Count + 1
                    };

                    _context.CamposPersonalizados.Add(campoExistente);
                    await _context.SaveChangesAsync(); 
                    camposDefinidos.Add(campoExistente);
                }

                // Ahora creamos el valor del campo
                var valor = new ProveedorCamposValore
                {
                    ProveedorId = proveedorId,
                    CampoPersonalizadoId = campoExistente.Id,
                    Valor = valorCampo
                };

                _context.ProveedorCamposValores.Add(valor);
            }

            await _context.SaveChangesAsync();
        }


        public async Task<Proveedore> CrearAsync(ProveedorDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nit))
                throw new ArgumentException("El NIT es requerido");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre es requerido");

            bool existe = await _context.Proveedores.AnyAsync(p => p.Nit == dto.Nit);
            if (existe)
                throw new InvalidOperationException($"Ya existe un proveedor con el NIT {dto.Nit}");

            var proveedor = new Proveedore
            {
                Nit = dto.Nit,
                Nombre = dto.Nombre,
                Email = dto.Email,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            await GuardarCamposPersonalizadosAsync(proveedor.Id, dto.CamposPersonalizados);

            if (dto.Servicios != null && dto.Servicios.Any())
            {
                foreach (var servicio in dto.Servicios)
                {
                    if (servicio.Id == 0)
                        throw new ArgumentException("Cada servicio debe tener un Id válido.");

                    var proveedorServicio = new ProveedorServicio
                    {
                        ProveedorId = proveedor.Id,
                        ServicioId = servicio.Id
                    };

                    _context.ProveedorServicios.Add(proveedorServicio);
                }

                await _context.SaveChangesAsync();
            }

            return await _context.Proveedores
                .Include(p => p.ProveedorServicios)
                    .ThenInclude(ps => ps.Servicio)
                .Include(p => p.ProveedorCamposValores)
                    .ThenInclude(pcv => pcv.CampoPersonalizado)
                .FirstAsync(p => p.Id == proveedor.Id);
        }
        public async Task<bool> EliminarAsync(int id)
        {

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<ProveedorResponseDTO> ObtenerPorIdAsync(int id)
        {
            var proveedor = await _context.Proveedores
                .Include(p => p.ProveedorServicios)
                    .ThenInclude(ps => ps.Servicio)
                .Include(p => p.ProveedorCamposValores)
                    .ThenInclude(pcv => pcv.CampoPersonalizado)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor == null)
                throw new KeyNotFoundException($"No se encontró el proveedor con ID {id}");

            return _mapper.Map<ProveedorResponseDTO>(proveedor);
        }
        public async Task<List<ProveedorResponseDTO>> ObtenerTodosAsync()
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.ProveedorServicios)
                    .ThenInclude(ps => ps.Servicio)
                .Include(p => p.ProveedorCamposValores)
                    .ThenInclude(pcv => pcv.CampoPersonalizado)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

            // Mapea todos los proveedores a su DTO usando AutoMapper
            var resultado = _mapper.Map<List<ProveedorResponseDTO>>(proveedores);

            return resultado;
        }
        public async Task<List<CountProvidersForCountriesDTO>> ProviderForCountries()
        {

            
            using var conn = new SqlConnection(_dataBase);
            var providers = await conn.QueryAsync<CountProvidersForCountriesDTO>(SP.SP_COUNT_CLIENTS_FOR_COUNTRIES, commandType: CommandType.StoredProcedure);

            await conn.CloseAsync();
            await conn.DisposeAsync();

            return _mapper.Map<List<CountProvidersForCountriesDTO>>(providers);
        }
    }
}
