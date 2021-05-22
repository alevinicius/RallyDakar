using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RallyDakar.Dominio.Entidades;
using System;

namespace RallyDakar.Dominio.DbContexto
{
    public class BaseDados
    {
        public static void CargaInicial(IServiceProvider serviceProvider)
        {
            using(var context = new RallyDbContexto(serviceProvider.GetRequiredService<DbContextOptions<RallyDbContexto>>()))
            {
                var temporada = new Temporada
                {
                    Id = 1,
                    Nome = "Temporada2020",
                    //temporada.DataInicio = DateTime.Now.AddDays(20); Inicia daqui a 20 dias
                    DataInicio = DateTime.Now
                };

                var equipe = new Equipe
                {
                    Id = 1,
                    Nome = "Azul",
                    CodigoIdentificador = "AZL"
                };

                var pilotoPedro = new Piloto
                {
                    Id = 1,
                    Nome = "Pedro"
                };


                var pilotoCarlos = new Piloto
                {
                    Id = 2,
                    Nome = "Carlos"
                };

                equipe.AdicionarPiloto(pilotoPedro);
                equipe.AdicionarPiloto(pilotoCarlos);

                temporada.AdicionarEquipe(equipe);

                context.Temporadas.Add(temporada);
                context.SaveChanges();

                Telemetria telemetria = new Telemetria
                {
                    Id = 1,
                    EquipeId = equipe.Id,
                    Data = DateTime.Now,
                    DataServidor = DateTime.Now
                };

                context.Telemetria.Add(telemetria);
                context.SaveChanges();
                



            }
        }
    }
}
