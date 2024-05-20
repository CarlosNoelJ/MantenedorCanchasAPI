using Microsoft.AspNetCore.Mvc;
using ReservaCanchasAPI.Models;
using ReservaCanchasAPI.Repositories;

namespace ReservaCanchasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanchaController : ControllerBase
    {
        private readonly IRepository<Canchas> _repository;

        public CanchaController(IRepository<Canchas> repository)
            => _repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Canchas>>> Get()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Canchas>> Get(string id)
        {
            var result = await _repository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Canchas cancha)
        {
            await _repository.Add(cancha);
            return CreatedAtAction(nameof(Get), new { id = cancha.CanchaId }, cancha);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Canchas cancha)
        {
            if (id != cancha.CanchaId)
            {
                return BadRequest();
            }
            await _repository.Update(cancha);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }
}
