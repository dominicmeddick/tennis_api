using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Repository;
using Repository.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APITechTest
{
    [Route("api/players")]
    public class PlayersController : Controller
    {
        private readonly TennisContext _context;

        public PlayersController(TennisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPlayers([FromQuery] PlayersQueryParameters query)
        {
            IQueryable<Player> players = _context.Players;

            if (query.NationalityName != null)
            {
                // Todo: Sanitize input
                string name = query.NationalityName.ToLower();

                IQueryable<Nationality> nationsResult =
                    _context.Nationalities.Where(n => n.Name.Equals(name));

                if (nationsResult.Count() == 0)
                {
                    return NotFound();
                }

                Nationality nationality = nationsResult.First();
                players = players.Where(p => p.Nationality.Id == nationality.Id);
            }

            if (query.RankName != null)
            {
                // Todo
            }

            players = players.OrderByDescending(p => p.Points);
            return Ok(PlayerView.GetViews(players));
        }

        [HttpGet("{id}")]
        public IActionResult GetPlayer(int id)
        {
            var player = _context.Players.Find(id);

            if (player != null)
            {
                return Ok(player);
            }

            return NotFound();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
