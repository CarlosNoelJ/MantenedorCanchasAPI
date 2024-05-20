using Microsoft.AspNetCore.Mvc;
using ReservaCanchasAPI.Models;
using ReservaCanchasAPI.Repositories;

namespace ReservaCanchasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCanchaController : ControllerBase
    {
        private readonly IRepository<TipoCancha> _repository;

        public TipoCanchaController(IRepository<TipoCancha> repository)
            => _repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCancha>>> Get()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCancha>> Get(string id)
        {
            var result = await _repository.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoCancha tipoCancha)
        {
            await _repository.Add(tipoCancha);
            return CreatedAtAction(nameof(Get), new { id = tipoCancha.TCanchaId }, tipoCancha);
        }

        [HttpPut]
        public async Task<ActionResult> Put(string id, [FromBody] TipoCancha tipoCancha)
        {
            if (id != tipoCancha.TCanchaId)
                return BadRequest();
            await _repository.Update(tipoCancha);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }
}
