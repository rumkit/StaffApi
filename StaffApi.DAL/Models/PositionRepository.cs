using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StaffApi.Data;

namespace StaffApi.Models
{
    public class PositionRepository : IPositionRepository
    {
        private readonly StaffApiContext _context;

        public async Task<Position> FindAsync(int id)
        {
            return await _context.Positions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Position>> GetPositionsAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task CreateAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Position position)
        {
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Position position)
        {
            _context.Entry(position).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool PositionExists(int id)
        {
            return _context.Positions.Any(p => p.Id == id);
        }


        public PositionRepository(StaffApiContext context)
        {
            _context = context;
        }
    }
}