using AutoMapper;
using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.UTILITY
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            #region
            CreateMap<Proveedore, Dictionary<string, object>>().
                ConvertUsing < ProveedorToDictionaryConverter > ();

            CreateMap<Proveedore, ProveedorResponseDTO>()
    .ForMember(dest => dest.CamposPersonalizados,
        opt => opt.MapFrom<CamposPersonalizadosResolver>())
    .ForMember(dest => dest.Servicios,
        opt => opt.MapFrom(src => src.ProveedorServicios.Select(ps => ps.Servicio)));

            #endregion

            #region Servicio
            CreateMap<Servicio, ServicioDTO>()
                 .ForMember(destino => destino.IdProveedor,
                opt => opt.MapFrom(origen => origen.Id)
                )
                .ForMember(destino =>
                destino.NombreProveedor,
                opt => opt.MapFrom(origen => origen.Nombre)
                );

            CreateMap<ServicioDTO, Servicio>()
                .ForMember(destino =>
                destino.Id,
                opt => opt.MapFrom(origen => origen.IdProveedor))
               .ForMember(destino =>
               destino.Nombre,
               opt => opt.MapFrom(origen => origen.NombreProveedor));
            #endregion Servicio
            #region Usuario
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            #endregion Usuario




        }
    }
}
