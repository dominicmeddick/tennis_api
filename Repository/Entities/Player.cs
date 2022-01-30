using System;
using System.Collections.Generic;

namespace Repository.Entities
{
    public class Player
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public int NationalityId { get; private set; }

        public Nationality Nationality { get; private set; }

        public DateTime BirthDate { get; private set; }

        public int Points { get; private set; }

        public int Games { get; private set; }

        public Player
        (
            int id,
            string firstName,
            string lastName,
            int nationalityId,
            DateTime birthDate,
            int points,
            int games
        ): this(firstName, lastName, nationalityId, birthDate)
        {
            Id = id;
            Points = points;
            Games = games;
        }

        public Player
        (
            string firstName,
            string lastName,
            int nationalityId,
            DateTime birthDate
        )
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("Must provide a valid first name.");
            }

            firstName = firstName.Trim();

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("Must provide a valid last name.");
            }

            lastName = lastName.Trim();

            if (nationalityId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nationalityId));
            }

            if (CalculcateAge(birthDate) < 16)
            {
                throw new ArgumentException("Player must be at least 16 years old.");
            }
            
            if (IsDuplicateName(firstName, lastName))
            {
                throw new ArgumentException("A player with the same first or last name is already registered.");
            }

            FirstName = firstName;
            LastName = lastName;
            NationalityId = nationalityId;
            BirthDate = birthDate;

            // Default value. Gets overriden if constructor with points param
            // is used.
            Points = 1200;
        }

        private bool IsDuplicateName(string firstName, string lastName)
        {
            return false;
        }

        // Todo: account for leap years
        public static int CalculcateAge(DateTime birthDate)
        {
            TimeSpan diff = DateTime.Today - birthDate;
            return diff.Days / 365;
        }
    }
}
