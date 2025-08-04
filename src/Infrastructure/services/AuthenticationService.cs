using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.dto.request;
using Application.interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationServiceOptions _options;
        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticationServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }

        public User? ValidateUser(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.Email) || string.IsNullOrEmpty(authenticationRequest.Password))
            {
                return null;
            }

            var user = _userRepository.GetByEmail(authenticationRequest.Email);
            if (user == null) return null;

            if (user != null)
            {
                if (user.Password == authenticationRequest.Password) return user;

            }
            return null;
        }

        public string? Authenticate(AuthenticationRequest authenticationRequest)
        {

            var user = ValidateUser(authenticationRequest) ?? null;
            if (user == null)
            {
                return null;
            }


            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);


            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.IdUser.ToString()));
            claimsForToken.Add(new Claim("name", user.Name));
            claimsForToken.Add(new Claim("lastName", user.LastName));
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("phoneNumber", user.PhoneNumber));




            var jwtSecurityToken = new JwtSecurityToken(
                _options.Issuer, 
                _options.Audience, 
                claimsForToken, 
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1), 
                credentials); 

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);


            return tokenToReturn.ToString();
        }

        public class AuthenticationServiceOptions
        {
            public const string AuthenticationService = "AuthenticationService";
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string SecretForKey { get; set; }
        }
    }
}
