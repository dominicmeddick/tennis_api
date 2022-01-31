using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc;

using Repository;
using Repository.Entities;
using System.Globalization;

namespace APITechTest
{
    // Todo: Figure out how to return messages as part of error

    [Route("api/players")]
    public class PlayersController : Controller
    {
        private readonly TennisContext _context;

        public PlayersController(TennisContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddPlayer([FromQuery] RegisterPlayerParameters query)
        {
            var nationsQuery = GetNationalities(query.Nationality);

            int nationalityId;
            if (nationsQuery.Count() == 0)
            {

                var nationality = new Nationality { Name = query.Nationality };
                _context.Nationalities.Add(nationality);
                _context.SaveChanges();

                nationalityId = nationality.Id;
            }
            else
            {
                nationalityId = nationsQuery.First().Id;
            }

            var playerQuery =
                _context.Players.Where(
                    p => p.FirstName.Equals(query.FirstName) && p.LastName.Equals(query.LastName)
                );

            if (playerQuery.Any())
            {
                return UnprocessableEntity();
            }

            Player player;
            try
            {
                player = new Player(
                    query.FirstName,
                    query.LastName,
                    nationalityId,
                    DateTime.ParseExact(query.Birthdate, "yyyyMMdd", CultureInfo.InvariantCulture),
                    query.Points ?? 1200,
                    query.Games ?? 0
                );
            }
            catch (Exception e)
            {
                return UnprocessableEntity();
            }

            _context.Players.Add(player);
            _context.SaveChanges();

           
            return Created("AddPlayer", player);
        }

        [HttpGet]
        public IActionResult GetPlayers([FromQuery] PlayersQueryParameters query)
        {
            // Got to get row index here, before any filtering, for the overall
            // player ranking position.
            var players =
                _context
                    .Players
                    .OrderByDescending(p => p.Points)
                    .ToList()
                    .Select((p, index) => new Player(p, index + 1));

            if (query.Nationality != null)
            {
                var nationsQuery = GetNationalities(query.Nationality);

                if (!nationsQuery.Any())
                {
                    return NotFound();
                }

                var nationalityId = nationsQuery.First().Id;
                players = players.Where(p => p.NationalityId == nationalityId);
            }

            if (query.Rank != null)
            {
                if (!Rank.RanksByName.TryGetValue(query.Rank.ToLower(), out Rank rank))
                {
                    return NotFound();
                }

                players = players.Where(p => p.Points >= rank.MinPoints && p.Points <= rank.MaxPoints);
            }

            // Todo: Figure out how to use player -> nationality relation to
            // populate Nationality instead. Trying to use
            // Include(player => player.Nationality) returned null values).
            var nationalities = _context.Nationalities;

            // Load players into memory so nationalities can be read
            var playersList = players.ToList().Select(p => new Player(p, nationalities.Find(p.NationalityId)));
            return Ok(PlayerView.GetViews(playersList));
        }

        private IQueryable<Nationality> GetNationalities(string nameInput)
        {
            // Todo: Sanitize input?
            var name = nameInput.ToLower();

            // Todo: More performant to store all nationality names as
            // lowercase? If there's time to write method to convert
            // lowercase back to correctly capitalized.

            return _context.Nationalities.Where(n => n.Name.ToLower().Equals(name));
        }
    }
}
