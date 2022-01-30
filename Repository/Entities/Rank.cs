using System;
namespace Repository.Entities
{
    public class Rank
    {
        public int Id { get; private set; }
        public int MinPoints { get; private set; }
        public int MaxPoints { get; private set; }

        public string Name { get; private set; }

        public Rank(int id, string name, int minPoints, int maxPoints)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(minPoints)} value must be at least 0.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Must provide a valid rank name.");
            }

            if (maxPoints < minPoints)
            {
                throw new ArgumentException($"{nameof(maxPoints)} value must be greater than {nameof(minPoints)} value.");
            }

            Id = id;
            Name = name;
            MinPoints = minPoints;
            MaxPoints = maxPoints;
        }
    }
}
