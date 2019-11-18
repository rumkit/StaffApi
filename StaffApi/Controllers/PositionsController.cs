using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffApi.Data;
using StaffApi.Models;
using StaffApi.ViewModels;

namespace StaffApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionRepository _positionRepository;

        public PositionsController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionViewModel>>> GetPositions()
        {
            var positions = await _positionRepository.GetPositionsAsync();
            return positions.Select(p => new PositionViewModel(p)).ToList();
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PositionViewModel>> GetPosition(int id)
        {
            var position = await _positionRepository.FindAsync(id);

            if (position == null)
            {
                return NotFound();
            }

            return new PositionViewModel(position);
        }

        // PUT: api/Positions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, Position position)
        {
            if (id != position.Id)
            {
                return BadRequest();
            }

            try
            {
                await _positionRepository.UpdateAsync(position);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Positions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(Position position)
        {
            await _positionRepository.CreateAsync(position);

            return CreatedAtAction("GetPosition", new { id = position.Id }, position);
        }

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Position>> DeletePosition(int id)
        {
            var position = await _positionRepository.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            await _positionRepository.RemoveAsync(position);

            return position;
        }

        private bool PositionExists(int id)
        {
            return _positionRepository.PositionExists(id);
        }
    }
}
