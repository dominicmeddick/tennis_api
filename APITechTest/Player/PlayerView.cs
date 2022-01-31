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

        public PlayerView(Player player)
        {
            Position = player.Position;
            FirstName = player.FirstName;
            LastName = player.LastName;
            Age = Player.CalculcateAge(player.BirthDate);
            Nationality = player.Nationality.Name;
            Rank = APITechTest.Rank.GetRank(player.Points).Name;
            Points = player.Points;
        }

        public static List<PlayerView> GetViews(IEnumerable<Player> players)
        {
            List<PlayerView> views = new List<PlayerView>();

            foreach (Player player in players)
            {
                PlayerView view = new PlayerView(player);
                views.Add(view);
            }

            return views;
        }
    }
}
