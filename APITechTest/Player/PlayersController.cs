using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using Microsoft.AspNetCore.Mvc;

using Repository;
using Repository.Entities;

namespace APITechTest.Players
{
    // Controller for players related endpoints.
    // Todo: Return error messages along with only error codes.
    [Route("players")]
    public class PlayersController : Controller
    {
        private readonly TennisContext _context;

        public PlayersController(TennisContext context)
        {
            _context = context;
        }

        // Endpoint for registering a new player.
        [HttpPost]
        public IActionResult RegisterPlayer([FromQuery] RegisterPlayerParameters query)
        {
            var nationsQuery = GetNationalities(query.Nationality);

            int nationalityId;
            if (nationsQuery.Count() == 0)
            {
                // Create new nationality entry if no nationality of submitted name
                // already exists.
                var nationality = new Nationality { Name = query.Nationality };
                _context.Nationalities.Add(nationality);
                _context.SaveChanges();

                nationalityId = nationality.Id;
            }
            else
            {
                nationalityId = nationsQuery.First().Id;
            }

            // Check if a player with the same first and last name already exists.
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
                // Player constructor can also throw exceptions for invalid inputs.
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

            // Can't return player because of infinite loop because of
            // nationality relationship(?).
            // Wanted to calculate actual rank, but new player wouldn't show
            // up in _context.Players query(?), so just returning a PlayerView
            // with unknown rank.
            player.Position = -1;

            return Created("AddPlayer", new DisplayablePlayer(player));
        }

        // Endpoint for listing all players in the club. 
        [HttpGet]
        public IActionResult GetPlayers([FromQuery] PlayersQueryParameters query)
        {
            IEnumerable<Player> rankedPlayers = null;
            IEnumerable<Player> unrankedPlayers = null;

            var playersContext = _context.Players;

            // If no rank, or the "Unranked" rank was specified, populate the
            // unrankedPlayers collection.
            if (query.Rank == null || query.Rank.ToLower().Equals(Rank.UnrankedName.ToLower()))
            {
                unrankedPlayers =
                    playersContext
                        .Where(p => p.Games < 3)
                        .OrderByDescending(p => p.Points)
                        .ToList();
            }

            // If no rank, or a rank other than "Unranked" was specified,
            // populate the rankedPlayers collection.
            if (query.Rank == null || unrankedPlayers == null)
            {
                // Have to get row index here, before any filtering,
                // for the overall player ranking position.
                rankedPlayers =
                    playersContext
                        .Where(p => p.Games >= 3)
                        .OrderByDescending(p => p.Points)
                        .ToList()
                        .Select((p, index) => new Player(p, index + 1));

                if (query.Rank != null)
                {
                    if (!Rank.RanksByName.TryGetValue(query.Rank.ToLower(), out Rank rank))
                    {
                        return UnprocessableEntity();
                    }

                    rankedPlayers =
                        rankedPlayers.Where(p => p.Points >= rank.MinPoints && p.Points <= rank.MaxPoints);
                }
            }
            
            if (query.Nationality != null)
            {
                var nationsQuery = GetNationalities(query.Nationality);

                if (!nationsQuery.Any())
                {
                    return UnprocessableEntity();
                }

                var nationalityId = nationsQuery.First().Id;

                rankedPlayers = rankedPlayers?.Where(p => p.NationalityId == nationalityId);
                unrankedPlayers = unrankedPlayers?.Where(p => p.NationalityId == nationalityId);
            }

            // Todo: Figure out how to use player -> nationality relation to
            // populate Nationality instead. Trying to use the method
            // Include(player => player.Nationality) returned null values.
            var nationalities = _context.Nationalities;

            
            var playerViews = new List<DisplayablePlayer>();
            if (rankedPlayers != null)
            {
                // Load players into memory so nationalities table can be read.
                var rankedPlayersList =
                        rankedPlayers?
                            .ToList()
                            .Select(
                                p => new Player(p, nationalities.Find(p.NationalityId))
                            );

                playerViews.AddRange(DisplayablePlayer.Convert(rankedPlayersList));
            }

            if (unrankedPlayers != null)
            {
                var ranking = playersContext.Count();
                var unrankedPlayersList =
                        unrankedPlayers?
                            .ToList()
                            .Select(
                                p => new Player(p, nationalities.Find(p.NationalityId), ranking)
                            );

                playerViews.AddRange(DisplayablePlayer.Convert(unrankedPlayersList));
            }

            return Ok(playerViews);
        }

        private IQueryable<Nationality> GetNationalities(string nameInput)
        {
            var name = nameInput.ToLower();
            return _context.Nationalities.Where(n => n.Name.ToLower().Equals(name));
        }
    }
}
