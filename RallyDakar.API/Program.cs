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
            //Seta o arquivo onde ser�o gravados os logs
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();

            //Colocado pra gerar log na inicializa��o do aplicativo, inclusive em caso de erro
            logger.Info("Iniciando o aplicativo");
            try
            {
                //CreateHostBuilder(args).Build().Run();

                //O Build() cria o servidor embutido no processo do AspNetCore, antes de colocar no IIS
                var host = CreateHostBuilder(args).Build();

                //O bloco "using" serve para criar um objeto em seu in�cio que � destru�do automaticamente ao finalizar o bloco.
                //no caso a vari�vel em quest�o � "scope"
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    BaseDados.CargaInicial(services);
                }

                //Coloca o servidor em execu��o
                host.Run();
            }
            catch (Exception e)
            {
                logger.Error(e.Message, "A aplica��o parou de rodar");
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
