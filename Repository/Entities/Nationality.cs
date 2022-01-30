using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class Nationality
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public ICollection<Player> Players { get; set; }

        public Nationality(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
