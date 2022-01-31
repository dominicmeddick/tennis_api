using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Repository;

namespace APITechTest.Match
{
    // Controller for match related endpoints.
    [Route("match")]
    public class MatchController : Controller
    {
        private readonly TennisContext _context;

        public MatchController(TennisContext context)
        {
            _context = context;
        }

        // Endpoint for registering a match's winner and loser. Will update
        // participant players' points and total game values.
        [HttpPost]
        public IActionResult RegisterMatch([FromQuery] RegisterMatchParameters query)
        {
            var playersContext = _context.Players;

            var winnerQuery = playersContext.Where(
                p => p.FirstName.Equals(query.WinnerFirstName) && p.LastName.Equals(query.WinnerLastName)
            );

            if (!winnerQuery.Any())
            {
                return NotFound();
            }

            var loserQuery = playersContext.Where(
               p => p.FirstName.Equals(query.LoserFirstName) && p.LastName.Equals(query.LoserLastName)
            );

            if (!loserQuery.Any())
            {
                return NotFound();
            }

            var winner = winnerQuery.First();
            var loser = loserQuery.First();

            PointsCalculator.ApplyMatch(winner, loser);
            _context.SaveChanges();

            return Accepted();
        }
    }
}
