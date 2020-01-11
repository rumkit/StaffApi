using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StaffApi.DAL.Models
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static string GetPasswordHash(string input)
        {
            using var hashProvider = SHA256.Create();
            
            byte[] data = hashProvider.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static readonly User[] UserStorage = new[]
        {
            new User() {Id = 1, FirstName = "John", LastName = "Johnson", PasswordHash = GetPasswordHash("strong_password"), Username = "j_johnson"},
            new User() {Id = 2, FirstName = "Smith", LastName = "Smithson", PasswordHash = GetPasswordHash("another_password"), Username = "s_smithson"},
        };

        public Task<User> GetUserAsync(string username)
        {
            return Task.FromResult(UserStorage.SingleOrDefault(u => u.Username == username));
        }

        public Task<User> GetUserAsync(int id)
        {
            return Task.FromResult(UserStorage.SingleOrDefault(u => u.Id == id));
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            return Task.FromResult(UserStorage.Select(x => x));
        }
    }
}