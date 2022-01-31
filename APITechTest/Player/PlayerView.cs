using System;
using System.Collections.Generic;

using Repository.Entities;

namespace APITechTest.Players
{
    // Converts data from the Player passed into the constructor into a
    // displayable format.
    public class DisplayablePlayer
    {
        public string Position { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }
        public string Nationality { get; }
        public string RankName { get; }
        public int Points { get; }

        public DisplayablePlayer(Player player)
        {
            Position = player.Position < 0 ? "Unknown" : player.Position.ToString();
            FirstName = player.FirstName;
            LastName = player.LastName;
            Age = Player.CalculcateAge(player.BirthDate);
            Nationality = player.Nationality.Name;
            Points = player.Points;

            RankName = (player.Games < 3) ? Rank.UnrankedName : Rank.GetRank(player.Points).Name;
        }

        // Return a list of all Players as DisposablePlayers, in the same order. 
        public static List<DisplayablePlayer> Convert(IEnumerable<Player> players)
        {
            var views = new List<DisplayablePlayer>();

            foreach (var player in players)
            {
                views.Add(new DisplayablePlayer(player));
            }

            return views;
        }
    }
}
