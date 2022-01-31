using System;
using NUnit.Framework;
using Repository.Entities;

namespace Tests
{
    public class PlayerTest
    {
        [Test]
        public void ConstructPlayerWithRequiredValues()
        {
            int nationalityId = 1;
            DateTime birthday = new DateTime(1994, 12, 4);
            Player player = new Player("Dominic", "Meddick", nationalityId, birthday, 1300, 4);

            Assert.NotNull(player);
            Assert.That(player.FirstName, Is.EqualTo("Dominic"));
            Assert.That(player.LastName, Is.EqualTo("Meddick"));
            Assert.That(player.NationalityId, Is.EqualTo(nationalityId));
            Assert.That(player.BirthDate, Is.EqualTo(birthday));
            Assert.That(player.Points, Is.EqualTo(1300));
            Assert.That(player.Games, Is.EqualTo(4));
        }

        [Test]
        public void ConstructPlayerWithoutRequiredValues()
        {
            int nationalityId = 1;
            DateTime birthday = new DateTime(1994, 12, 4);

            Assert.Throws<ArgumentException>
            (
                () => new Player(null, "Meddick", nationalityId, birthday, 80, 100)
            );
        }

        [Test]
        public void ConstructTooYoungPlayer()
        {
            int nationalityId = 1;
            DateTime birthday = new DateTime(2006, 12, 4);

            Assert.Throws<ArgumentException>
            (
                () => new Player("Dom", "Meddick", nationalityId, birthday, 9000, 10)
            );
        }
    }
}
