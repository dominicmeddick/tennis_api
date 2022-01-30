﻿using System;
using System.Globalization;

namespace Repository.Entities
{
    public class Player
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int NationalityId { get; set; }

        public Nationality Nationality { get; set; }

        public DateTime BirthDate { get; set; }

        public int Points { get; set; }

        public int Games { get; set; }

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
            string birthDate
        ): this(firstName, lastName, nationalityId, ParseBirthday(birthDate))
        {

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
            var diff = DateTime.Today - birthDate;
            return diff.Days / 365;
        }

        public static DateTime ParseBirthday(string dateString)
        {
            return DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
