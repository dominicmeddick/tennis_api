using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class Nationality
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
