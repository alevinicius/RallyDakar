using Microsoft.EntityFrameworkCore;
using RallyDakar.Dominio.Entidades;

namespace RallyDakar.Dominio.DBContexto
{
    public class RallyDBContexto : DbContext
    {
        //O DBSet cria as tabelas no banco de dados e
        //nos dá acesso à fazer consultas através do Linq
        public DbSet<Temporada> Temporadas { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Piloto> Pilotos { get; set; }
        public DbSet<Telemetria> Temporada { get; set; }

        //Construtor, cria a instância do DBContext com as propriedades inseridas acima
        //Exige o parâmetro options, e com o base passa esse parâmetro para a classe Pai (DBContext)      
        public RallyDBContexto(DbContextOptions<RallyDBContexto> options) : base(options)
        {

        }
    }
}
