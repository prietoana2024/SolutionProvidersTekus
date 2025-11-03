using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProvidersTekus.DAL.DBContext;
using ProvidersTekus.DAL.Repository;
using ProvidersTekus.DAL.Repository.Interfaces;
using ProvidersTekus.DLL.Services;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.UTILITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
namespace ProvidersTekus.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BdprovidersContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<JwtSettings>>().Value);

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddMemoryCache();



            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<ICampoPersonalizadoService, CampoPersonalizadoService>();
            services.AddScoped<IProveedorService, ProveedorService>();
            services.AddScoped<IServiciosService, ServiciosService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<DbContext, BdprovidersContext>();
            services.AddHttpClient<CountryLayerService>();



        }

    }
}
