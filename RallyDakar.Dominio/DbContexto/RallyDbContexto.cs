using Microsoft.EntityFrameworkCore;
using RallyDakar.Dominio.Entidades;

namespace RallyDakar.Dominio.DbContexto
{
    public class RallyDbContexto : DbContext
    {
        public DbSet<Temporada> Temporadas { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Piloto> Pilotos { get; set; }
        public DbSet<Telemetria> Telemetria { get; set; }

        public RallyDbContexto( DbContextOptions<RallyDbContexto> options) :base(options)
        {

        }


    }
}
