using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApi.Models
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetPositionsAsync();
        Task<Position> FindAsync(int id);
        Task CreateAsync(Position position);
        Task RemoveAsync(Position position);
        Task UpdateAsync(Position position);
        bool PositionExists(int id);
    }
}