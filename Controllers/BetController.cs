using BetsApi.Services;
using BetsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BetController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetController(IBetService betService)
        {
            _betService = betService;
        }

        [HttpPost]
        public async Task<ActionResult<Bet>> PostBet(Bet bet)
        {
            try
            {
                var betCreated = await _betService.CreateBetAsync(bet);
                return Ok(betCreated);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<Bet>> GetBets()
        {
            var bets = await _betService.GetAllBetAsync();
            if(bets == null)
            {
                return Enumerable.Empty<Bet>();
            }
            return bets;            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetBetById(long id)
        {
            var bet = await _betService.GetBetAsync(id);
            if(bet == null)
            {
                return NotFound("Bet not found");
            }
            return Ok(bet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBet(long id, Bet bet)
        {
            try
            {
                await _betService.UpdateBetAsync(id, bet);
                return Ok();
            } 
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBet(long id)
        {
            try
            {
                await _betService.DeleteBetAsync(id);
                return Ok();
            } 
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
