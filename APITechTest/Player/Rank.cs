using System;
namespace APITechTest
{
    public class Rank
    {
        private static readonly Rank[] Ranks = new Rank[]
        {
            new Rank("Unranked", int.MinValue, -1),
            new Rank("Bronze", 0, 2999),
            new Rank("Silver", 3000, 4999),
            new Rank("Gold", 5000, 9999),
            new Rank("Supersonic Legend", 10000, int.MaxValue)
        };

        private int _minPoints;
        private int _maxPoints;

        public string Name { get; private set; }

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
            _minPoints = minPoints;
            _maxPoints = maxPoints;
        }

        public static Rank GetRank(int points)
        {
            foreach (Rank rank in Ranks)
            {
                if (points <= rank._maxPoints)
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
