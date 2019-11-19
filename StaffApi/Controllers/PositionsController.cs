using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffApi.Data;
using StaffApi.Models;
using StaffApi.DTO;

namespace StaffApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]        
    public class PositionsController : ControllerBase
    {
        private readonly IPositionRepository _positionRepository;

        public PositionsController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        /// <summary>
        /// Gets list of all available positions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDTO>>> GetPositions()
        {
            var positions = await _positionRepository.GetPositionsAsync();
            return positions.Select(p => new PositionDTO(p)).ToList();
        }

        /// <summary>
        /// Gets specific position
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PositionDTO>> GetPosition(int id)
        {
            var position = await _positionRepository.FindAsync(id);

            if (position == null)
            {
                return NotFound();
            }

            return new PositionDTO(position);
        }

        /// <summary>
        /// Updates positions details
        /// </summary>       
        /// Sample request:
        ///
        ///     PUT /Positions
        ///     {
        ///        "id": 0,
        ///        "name": "Position name",
        ///        "grade": 9
        ///     }
        ///
        /// NOTE: grade must be between 1 and 15
        /// <param name="id"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Position position)
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

        /// <summary>
        /// Creates new position
        /// Sample request:
        ///
        ///     POST /Positions
        ///     {
        ///        "id": 0,
        ///        "name": "Position name",
        ///        "grade": 9
        ///     }
        ///
        /// NOTE: grade must be between 1 and 15
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Position>> Create(Position position)
        {
            await _positionRepository.CreateAsync(position);

            return CreatedAtAction("GetPosition", new { id = position.Id }, position);
        }

        /// <summary>
        /// Deletes specified position
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Position>> Delete(int id)
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
