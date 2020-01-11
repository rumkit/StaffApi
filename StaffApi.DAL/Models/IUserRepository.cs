using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApi.DAL.Models
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username);
        Task<User> GetUserAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
    }
}