using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ProvidersTekus.DAL.Repository.Interfaces;
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
    public class ServiciosService : IServiciosService
    {


        private readonly IGenericRepository<Servicio> _servicioRepositorio;
        private readonly string? _dataBase;

        private readonly IMapper _mapper;

        private readonly IMemoryCache _cache;

        public ServiciosService(IGenericRepository<Servicio> servicioRepositorio, IMapper mapper, IMemoryCache cache, IConfiguration configuration)
        {
            _dataBase = configuration.GetConnectionString(AppSettings.DB_CONNECTION)
                               ?? throw new InvalidOperationException("No se encontró la cadena de conexión en appsettings.json.");
            _servicioRepositorio = servicioRepositorio;
            _mapper = mapper;
            _cache = cache;
        }




        public async Task<List<ServicioDTO>> Lista()
        {
            try
            {

                var servicioCreate = await _servicioRepositorio.GetAll();
                return _mapper.Map<List<ServicioDTO>>(servicioCreate.ToList());

            }
            catch
            {
                throw;
            }
        }

        public async Task<ServicioDTO> Crear(ServicioDTO modelo)
        {
            try
            {
                var servicioCreate = await _servicioRepositorio.Create(_mapper.Map<Servicio>(modelo));
                if (servicioCreate.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el banco");
                }
                return _mapper.Map<ServicioDTO>(servicioCreate);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ServicioDTO modelo)
        {
            try
            {
                var servicioModelo = _mapper.Map<Servicio>(modelo);
                var servicioEncontrado = await _servicioRepositorio.Get(u => u.Id == servicioModelo.Id);
                if (servicioEncontrado == null)
                {
                    throw new TaskCanceledException("No existe el servicio");
                }
                servicioEncontrado.Nombre = servicioModelo.Nombre;
                servicioEncontrado.Paises = servicioModelo.Paises;
                servicioEncontrado.ValorHora = servicioModelo.ValorHora;

                bool respuesta = await _servicioRepositorio.Update(servicioEncontrado);
                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo editar");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var servicioEncontrado = await _servicioRepositorio.Get(p => p.Id == id);
                if (servicioEncontrado == null)
                {
                    throw new TaskCanceledException("El servicio no existe");
                }
                bool respuesta = await _servicioRepositorio.Delete(servicioEncontrado);
                if (respuesta == false)
                {
                    throw new TaskCanceledException("El servicio no se elimino con exito");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<CountServicesDTO>> ServicesForCountries()
        {
            
            using var conn = new SqlConnection(_dataBase);
            var servicios = await conn.QueryAsync<CountServicesDTO>(
                SP.SP_COUNT_SERVICES,
                commandType: CommandType.StoredProcedure
            );
            await conn.CloseAsync();
            await conn.DisposeAsync();

            return _mapper.Map<List<CountServicesDTO>>(servicios);
        }

    }
}
