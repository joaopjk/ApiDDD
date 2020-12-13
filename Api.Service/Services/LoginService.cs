using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Secutiry;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;
        private SigningConfiguration _signingConfiguration;
        private IConfiguration _configuration;
        public LoginService(IUserRepository repository, SigningConfiguration signingConfiguration, IConfiguration configuration)
        {
            _repository = repository;
            _signingConfiguration = signingConfiguration;
            _configuration = configuration;
        }
        public async Task<object> FindByLogin(LoginDto user)
        {
            var baseUser = new UserEntity();

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {
                baseUser = await _repository.FindByLogin(user.Email);
                if (baseUser == null)
                {
                    return new
                    {
                        authenticate = false,
                        message = "Falha au autenticar"
                    };
                }
                else
                {
                    var identity = new ClaimsIdentity(
                        new GenericIdentity(user.Email),
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName,user.Email)
                        }
                    );
                    DateTime createDate = DateTime.Now;
                    DateTime expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToInt32(Environment.GetEnvironmentVariable("Seconds")));
                    var handler = new JwtSecurityTokenHandler();
                    var token = CreateToken(identity, createDate, expirationDate, handler);

                    return SucessObject(createDate, expirationDate, token, user);
                }
            }
            else
            {
                return new
                {
                    authenticate = false,
                    message = "Falha ao autenticar"
                };
            }
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = Environment.GetEnvironmentVariable("Issuer"),
                Audience = Environment.GetEnvironmentVariable("Audience"),
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }
        private object SucessObject(DateTime createDate, DateTime expireDate, string token, LoginDto user)
        {
            return new
            {
                authenticate = true,
                created = createDate.ToString("yyy-MM-dd HH:mm:ss"),
                expiration = expireDate.ToString("yyy-MM-dd HH:mm:ss"),
                accessToken = token,
                userName = user.Email,
                message = "Usuário logado com sucesso !"
            };
        }
    }

}