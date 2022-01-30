using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class Nationality
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public Nationality(int id, string name)
        {
            Id = id;
            Name = name.ToLower();
        }
    }
}
