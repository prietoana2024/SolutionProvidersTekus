using ProvidersTekus.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidersTekus.DLL.Services.Contrato
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> Login(LoginDTO loginDto);
        Task<LoginResponseDTO> Register(RegisterDTO registerDto);
        string GenerateJwtToken(UsuarioDTO usuario);
    }
}
