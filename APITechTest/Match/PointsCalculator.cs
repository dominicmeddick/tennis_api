using System;
using Repository.Entities;

namespace APITechTest.Match
{
    public static class PointsCalculator
    {
        public static void ApplyMatch(Player winner, Player loser)
        {
            var transferredPoints = (int) Math.Round(loser.Points * 0.1);

            winner.Points += transferredPoints;
            loser.Points -= transferredPoints;

            winner.Games += 1;
            loser.Games += 1;

            // Todo: What happens when loser's points are < 10
            // (so that 10% of that is < 1)
        }
    }
}
