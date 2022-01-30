﻿using System;
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

            Player player;
            try
            {
                player = new Player(
                    query.FirstName,
                    query.LastName,
                    nationalityId,
                    query.Birthdate
                );
            }
            catch (Exception e)
            {
                // Todo: error messages
                return UnprocessableEntity();
            }

            _context.Players.Add(player);
            _context.SaveChanges();
            return Created("AddPlayer", new PlayerView(player));
        }

        [HttpGet]
        public IActionResult GetPlayers([FromQuery] PlayersQueryParameters query)
        {
            IQueryable<Player> players = null;

            if (query.Nationality != null)
            {
                var nationsQuery = GetNationalities(query.Nationality);

                if (nationsQuery.Count() == 0)
                {
                    return NotFound();
                }

                nationsQuery = nationsQuery.Include(n => n.Players);

                if (nationsQuery.Count() == 0)
                {
                    return NotFound();
                }

                players = nationsQuery
                            .First()
                            .Players
                            .AsQueryable()
                            .Include(p => p.Nationality);
            }

            if (query.Rank != null)
            {
                // Todo: Sanitize input?

                if (!Rank.RanksByName.TryGetValue(query.Rank, out Rank rank))
                {
                    return NotFound();
                }


                players = (players ?? _context.Players)
                            .Where(p => p.Points >= rank.MinPoints && p.Points <= rank.MaxPoints);
            }

            players = players.OrderByDescending(p => p.Points);
            return Ok(PlayerView.GetViews(players));
        }

        private IQueryable<Nationality> GetNationalities(string nameInput)
        {
            // Todo: Sanitize input?
            var name = nameInput.ToLower();

            // Todo: More performant to store all nationality names as
            // lowercase, if there's time to write method to convert
            // lowercase back to correctly capitalized.

            return _context.Nationalities.Where(n => n.Name.ToLower().Equals(name));
        }
    }
}
