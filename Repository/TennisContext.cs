using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using System;

namespace Repository
{
    public class TennisContext : DbContext
    {
        public DbSet<Nationality> Nationalities { get; set; }

        public DbSet<Player> Players { get; set; }

        public TennisContext(DbContextOptions<TennisContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration["ConnectionString"]);
        }

        // Pre-populate db with nationalities and players
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var serbianNationality = new Nationality { Id = 1, Name = "Serbia" };
            var russianNationality = new Nationality { Id = 2, Name = "Russia" };
            var germanNationality = new Nationality { Id = 3, Name = "Germany" };
            var greekNationality = new Nationality { Id = 4, Name = "Greece" };
            var spanishNationality = new Nationality { Id = 5, Name = "Spain" };
            var polishNationality = new Nationality { Id = 6, Name = "Poland" };

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.ToTable("Nationalities", "dbo");

                entity.HasData
                (
                    serbianNationality,
                    russianNationality,
                    germanNationality,
                    greekNationality,
                    spanishNationality,
                    polishNationality
                );
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Players", "dbo");

                entity
                    .HasOne(player => player.Nationality)
                    .WithMany(n => n.Players);

                entity.HasData(
                    new Player
                    (
                        id: 1,
                        firstName: "Rafael",
                        lastName: "Nadal",
                        nationalityId: spanishNationality.Id,
                        birthDate: DateTime.Parse("1986-06-03"),
                        points: 4875,
                        games: 1240
                    ),
                    new Player
                    (
                        id: 2,
                        firstName: "Novak",
                        lastName: "Djokovic",
                        nationalityId: serbianNationality.Id,
                        birthDate: DateTime.Parse("1987-05-22"),
                        points: 11015,
                        games: 1188
                    ),
                    new Player
                    (
                        id: 3,
                        firstName: "Roberto Bautista",
                        lastName: "Agut",
                        nationalityId: spanishNationality.Id,
                        birthDate: DateTime.Parse("1988-04-14"),
                        points: 2385,
                        games: 546
                    ),
                    new Player
                    (
                        id: 4,
                        firstName: "Jan-Lennard",
                        lastName: "Struff",
                        nationalityId: germanNationality.Id,
                        birthDate: DateTime.Parse("1990-04-25"),
                        points: 1149,
                        games: 359
                    ),
                    new Player
                    (
                        id: 5,
                        firstName: "Dusan",
                        lastName: "Lajovic",
                        nationalityId: serbianNationality.Id,
                        birthDate: DateTime.Parse("1990-06-30"),
                        points: 1346,
                        games: 351
                    ),
                    new Player
                    (
                        id: 6,
                        firstName: "Pablo Carreno",
                        lastName: "Busta",
                        nationalityId: spanishNationality.Id,
                        birthDate: DateTime.Parse("1991-07-12"),
                        points: 2305,
                        games: 419
                    ),
                    new Player
                    (
                        id: 7,
                        firstName: "Filip",
                        lastName: "Krajinovic",
                        nationalityId: serbianNationality.Id,
                        birthDate: DateTime.Parse("1992-02-27"),
                        points: 1427,
                        games: 206
                    ),
                    new Player
                    (
                        id: 8,
                        firstName: "Asla",
                        lastName: "Karatsev",
                        nationalityId: russianNationality.Id,
                        birthDate: DateTime.Parse("1993-09-04"),
                        points: 2553,
                        games: 71
                    ),
                    new Player
                    (
                        id: 9,
                        firstName: "Dominik",
                        lastName: "Koepfer",
                        nationalityId: germanNationality.Id,
                        birthDate: DateTime.Parse("1994-04-29"),
                        points: 1096,
                        games: 74
                    ),
                    new Player
                    (
                        id: 10,
                        firstName: "Daniil",
                        lastName: "Medvedev",
                        nationalityId: russianNationality.Id,
                        birthDate: DateTime.Parse("1996-02-11"),
                        points: 8935,
                        games: 325
                    ),
                    new Player
                    (
                        id: 11,
                        firstName: "Hubert",
                        lastName: "Hurkacz",
                        nationalityId: polishNationality.Id,
                        birthDate: DateTime.Parse("1997-02-11"),
                        points: 3336,
                        games: 166
                    ),
                    new Player
                    (
                        id: 12,
                        firstName: "Alexander",
                        lastName: "Zverev",
                        nationalityId: germanNationality.Id,
                        birthDate: DateTime.Parse("1997-04-20"),
                        points: 7970,
                        games: 453
                    ),
                    new Player
                    (
                        id: 13,
                        firstName: "Andrey",
                        lastName: "Rublev",
                        nationalityId: russianNationality.Id,
                        birthDate: DateTime.Parse("1997-10-20"),
                        points: 4785,
                        games: 297
                    ),
                    new Player
                    (
                        id: 14,
                        firstName: "Stefanos",
                        lastName: "Tsitsipas",
                        nationalityId: greekNationality.Id,
                        birthDate: DateTime.Parse("1998-08-12"),
                        points: 6540,
                        games: 285
                    )
                );
            });
        }

        private readonly IConfiguration configuration;
    }
}
