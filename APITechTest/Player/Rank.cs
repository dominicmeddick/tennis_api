using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace APITechTest
{
    public class Rank
    {
        private static readonly Rank[] _ranks = new Rank[]
        {
            new Rank("Bronze", 0, 2999),
            new Rank("Silver", 3000, 4999),
            new Rank("Gold", 5000, 9999),
            new Rank("Supersonic Legend", 10000, int.MaxValue)
        };

        private static readonly Dictionary<string, Rank> _ranksByName =
            new Dictionary<string, Rank>
            {
                { _ranks[0].Name.ToLower(), _ranks[0] },
                { _ranks[1].Name.ToLower(), _ranks[1] },
                { _ranks[2].Name.ToLower(), _ranks[2] },
                { _ranks[3].Name.ToLower(), _ranks[3] }
            };

        public static readonly ReadOnlyDictionary<string, Rank> RanksByName =
            new ReadOnlyDictionary<string, Rank>(_ranksByName);

        public const string UnrankedName = "Unranked";

        public int MinPoints { get; }
        public int MaxPoints { get; }

        public string Name { get; }

        public Rank(string name, int minPoints, int maxPoints)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Must provide a valid rank name.");
            }

            if (maxPoints < minPoints)
            {
                throw new ArgumentException($"{nameof(maxPoints)} value must be greater than {nameof(minPoints)} value.");
            }

            Name = name;
            MinPoints = minPoints;
            MaxPoints = maxPoints;
        }

        public static Rank GetRank(int points)
        {
            foreach (Rank rank in _ranks)
            {
                if (points <= rank.MaxPoints)
                {
                    return rank;
                }
            }

            // Will never actually reach this case, because the last rank
            // has a MaxPoints value of int.MaxValue.
            return null;
        }
    }
}
