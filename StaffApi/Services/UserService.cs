using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StaffApi.DAL.Models;
using StaffApi.DAL.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace StaffApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;
        private readonly AppSettings _appSettings;

        public UserService(IUserRepository usersRepository, IOptions<AppSettings> appSettings)
        {
            _usersRepository = usersRepository;
            _appSettings = appSettings.Value ;
        }
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _usersRepository.GetUserAsync(username);

            // return null if user not found
            if (user == null)
                return null;

            if (!CheckUserPassword(user, password))
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        private bool CheckUserPassword(User user, string password)
        {
            using var hashProvider = SHA256.Create();
            
            byte[] inputHash = hashProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < inputHash.Length; i++)
            {
                sBuilder.Append(inputHash[i].ToString("x2"));
            }
            var inputHashString = sBuilder.ToString();
            return inputHashString == user.PasswordHash;
        }

        public Task<IEnumerable<User>> GetAll()
        {
           return _usersRepository.GetUsersAsync();
        }
    }
}