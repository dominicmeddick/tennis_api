using System;
using NUnit.Framework;
using Repository.Entities;
using APITechTest.Players;

namespace Tests
{
    public class RankTest
    {
        Player _player;

        [OneTimeSetUp]
        public void CreatePlayer()
        {
            int nationalityId = 1;
            DateTime birthday = new DateTime(1994, 12, 4);
            _player = new Player("Dominic", "Meddick", nationalityId, birthday, 1200, 4);
        }

        [Test]
        public void GetRankWithMiddlePoint()
        {
            _player.Points = 5550;
            Assert.That(Rank.GetRank(_player.Points).Name, Is.EqualTo("Gold"));
        }

        [Test]
        public void GetRankWithMaxPoint()
        {
            _player.Points = 4999;
            Assert.That(Rank.GetRank(_player.Points).Name, Is.EqualTo("Silver"));
        }

        [Test]
        public void GetRankWithMinPoint()
        {
            _player.Points = 0;
            Assert.That(Rank.GetRank(_player.Points).Name, Is.EqualTo("Bronze"));

        }
    }
 }
