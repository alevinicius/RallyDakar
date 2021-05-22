using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using RallyDakar.Dominio.DbContexto;
using System;
namespace RallyDakar.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Seta o arquivo onde serão gravados os logs
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();

            //Colocado pra gerar log na inicialização do aplicativo, inclusive em caso de erro
            logger.Info("Iniciando o aplicativo");
            try
            {
                //CreateHostBuilder(args).Build().Run();

                //O Build() cria o servidor embutido no processo do AspNetCore, antes de colocar no IIS
                var host = CreateHostBuilder(args).Build();

                //O bloco "using" serve para criar um objeto em seu início que é destruído automaticamente ao finalizar o bloco.
                //no caso a variável em questão é "scope"
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    BaseDados.CargaInicial(services);
                }

                //Coloca o servidor em execução
                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, "A aplicação parou de rodar");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            };
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>()
                    .UseNLog(); //Colocado pra utilizar o NLog
                });
    }
}
