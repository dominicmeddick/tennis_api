using System;
using Repository.Entities;

namespace APITechTest.Match
{
    public static class PointsCalculator
    {
        // Loser gives 10% of their points to the winner. 
        public static void ApplyMatch(Player winner, Player loser)
        {
            var transferredPoints = (int) Math.Round(loser.Points * 0.1);

            winner.Points += transferredPoints;
            loser.Points -= transferredPoints;

            winner.Games += 1;
            loser.Games += 1;

            // Todo: What's supposed to happen when loser's points are < 5
            // (because 5 * 0.1 == 0.5 and a lower number is rounded to 0)?
        }
    }
}
