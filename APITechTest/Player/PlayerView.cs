using System;
using System.Collections.Generic;

using Repository.Entities;

namespace APITechTest
{
    public class PlayerView
    {
        public int Position { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
        public string Nationality { get; }
        public string Rank { get; }
        public int Points { get; }

        public PlayerView(Player player, int position)
        {
            Position = position;
            FirstName = player.FirstName;
            LastName = player.LastName;
            Age = Player.CalculcateAge(player.BirthDate);
            // Todo: get name
            Nationality = player.NationalityId.ToString();

            // Todo:
            // Rank = player.
            Points = player.Points;
        }

        public static List<PlayerView> GetViews(IEnumerable<Player> players)
        {
            List<PlayerView> views = new List<PlayerView>();

            int pos = 1;
            foreach (Player player in players)
            {
                views.Add(new PlayerView(player, pos++));
            }

            return views;
        }
    }
}
