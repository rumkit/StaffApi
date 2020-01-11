using System.Collections.Generic;
using System.Threading.Tasks;
using StaffApi.DAL.Models;

namespace StaffApi.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}
