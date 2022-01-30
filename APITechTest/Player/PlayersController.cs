using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

            if (query.Nationality != null)
            {
                // Todo: Sanitize input?
                var name = query.Nationality.ToLower();

                // Todo: More performant to store all nationality names as
                // lowercase, if there's time to write method to convert
                // lowercase back to correctly capitalized.

                var nationsQuery =
                    _context.Nationalities
                        .Where(n => n.Name.ToLower().Equals(name))
                        .Include(n => n.Players);

                if (nationsQuery.Count() == 0)
                {
                    return NotFound();
                }

                players = nationsQuery.First().Players.AsQueryable();
            }

            //if (query.Rank != null)
            //{
            //    // Todo: Sanitize input?
            //    string name = query.Rank.ToLower();
            //    IQueryable<Rank> ranksQuery =
            //        _context.Ranks.Where(n => n.Name.ToLower().Equals(name));

            //    if (ranksQuery.Count() == 0)
            //    {
            //        return NotFound();
            //    }

            //    Rank rank = ranksQuery.First();
            //    players = players.Where(p => p.Points >= rank.MinPoints && p.Points <= rank.MaxPoints);
            //}

            players = players.OrderByDescending(p => p.Points);
            return Ok(PlayerView.GetViews(players, _context.Nationalities.ToArray()));
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
