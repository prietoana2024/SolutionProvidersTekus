using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ProvidersTekus.DAL.Repository.Interfaces;
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
    public class UsuarioService:IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        private readonly IMemoryCache _cache;

        public UsuarioService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper, IMemoryCache cache)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {

                var usuarioCreate = await _usuarioRepositorio.GetAll();
                return _mapper.Map<List<UsuarioDTO>>(usuarioCreate.ToList());

            }
            catch
            {
                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioCreate = await _usuarioRepositorio.Create(_mapper.Map<Usuario>(modelo));
                if (usuarioCreate.Id == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el banco");
                }
                return _mapper.Map<UsuarioDTO>(usuarioCreate);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepositorio.Get(u => u.Id == usuarioModelo.Id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("No existe el usuario");
                }
                usuarioEncontrado.Nombre = usuarioModelo.Nombre;
                usuarioEncontrado.Pwd = usuarioModelo.Pwd;
                usuarioEncontrado.Email = usuarioModelo.Email;

                bool respuesta = await _usuarioRepositorio.Update(usuarioEncontrado);
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
                var usuarioEncontrado = await _usuarioRepositorio.Get(p => p.Id == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }
                bool respuesta = await _usuarioRepositorio.Delete(usuarioEncontrado);
                if (respuesta == false)
                {
                    throw new TaskCanceledException("El usuario no se elimino con exito");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
