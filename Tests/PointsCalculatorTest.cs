using System;
using NUnit.Framework;
using APITechTest.Match;
using Repository.Entities;

namespace Tests
{
    public class PointsCalculatorTest
    {
        [Test]
        public void WinsAgainstLowerRank()
        {
            var birthdate = new DateTime(2000, 1, 1);
            var winner = new Player("Luca", "Smith", 1, birthdate, 1000, 4);
            var loser = new Player("Brendan", "Potts", 1, birthdate, 900, 4);

            PointsCalculator.ApplyMatch(winner, loser);
            Assert.That(winner.Points, Is.EqualTo(1090));
            Assert.That(loser.Points, Is.EqualTo(810));
        }

        [Test]
        public void WinsAgainstHigherRank()
        {
            var birthdate = new DateTime(2000, 1, 1);
            var winner = new Player("Daniel", "Craig", 1, birthdate, 700, 4);
            var loser = new Player("James", "Bond", 1, birthdate, 1200, 4);

            PointsCalculator.ApplyMatch(winner, loser);
            Assert.That(winner.Points, Is.EqualTo(820));
            Assert.That(loser.Points, Is.EqualTo(1080));
        }
    }
}
