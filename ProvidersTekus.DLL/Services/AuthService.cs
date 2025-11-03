using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProvidersTekus.DAL.DBContext;
using ProvidersTekus.DLL.Services.Contrato;
using ProvidersTekus.DTO;
using ProvidersTekus.MODELS;
using ProvidersTekus.UTILITY;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ProvidersTekus.DLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly BdprovidersContext _context;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthService(BdprovidersContext context, IMapper mapper, JwtSettings jwtSettings)
        {
            _context = context;
            _mapper = mapper;
            _jwtSettings = jwtSettings;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginDto)
        {
            try
            {
                // Buscar usuario por email
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (usuario == null)
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Credenciales inválidas"
                    };
                }

                bool passwordValida = BCryptNet.Verify(loginDto.Password, usuario.Pwd);

                if (!passwordValida)
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "Credenciales inválidas"
                    };
                }

                // Mapear a DTO
                var usuarioDto = _mapper.Map<UsuarioDTO>(usuario);

                // Generar token
                var token = GenerateJwtToken(usuarioDto);

                return new LoginResponseDTO
                {
                    Success = true,
                    Token = token,
                    Message = "Login exitoso",
                    Usuario = usuarioDto
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<LoginResponseDTO> Register(RegisterDTO registerDto)
        {
            try
            {
                // Verificar si el email ya existe
                var existeEmail = await _context.Usuarios
                    .AnyAsync(u => u.Email == registerDto.Email);

                if (existeEmail)
                {
                    return new LoginResponseDTO
                    {
                        Success = false,
                        Message = "El email ya está registrado"
                    };
                }

                string passwordHash = BCryptNet.HashPassword(registerDto.Password);
                var nuevoUsuario = new Usuario
                {
                    Nombre = registerDto.Nombre,
                    Email = registerDto.Email,
                    Pwd = passwordHash
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                var usuarioDto = _mapper.Map<UsuarioDTO>(nuevoUsuario);

                var token = GenerateJwtToken(usuarioDto);

                return new LoginResponseDTO
                {
                    Success = true,
                    Token = token,
                    Message = "Registro exitoso",
                    Usuario = usuarioDto
                };
            }
            catch (Exception ex)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
                if (user != null)
                {
                    _context.Usuarios.Remove(user);
                    await _context.SaveChangesAsync();
                }

                return new LoginResponseDTO
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public string GenerateJwtToken(UsuarioDTO usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
}
}
