using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RallyDakar.Dominio.DbContexto;
using RallyDakar.Dominio.Interfaces;
using RallyDakar.Dominio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RallyDakar.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Registro a classe que herda do DBContext utilizando o mecanismo de inversão de dependência
            //Em "ServiceLifetime.Scoped" é definido o tempo de vida do DBContext, atrelado ao escopo, de forma que...
            //... a instância do DBContext dura até acabar a requisição
            services.AddDbContext<RallyDbContexto>(opt => opt.UseInMemoryDatabase("RallyDB"),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped);
                        
            //No serviço de Controller, adicionado o serviço do Newtonsoftjson, baixado no pacotes NUGET, da microsoft
            //junto também foi baixado o jsonpatch da Microsoft
            //creio que assim o controller vai ter acesso aos recursos do newtonsoftjson
            services.AddControllers()
                    .AddNewtonsoftJson();

            services.AddScoped<IPilotoRepositorio, PilotoRepositorio>();
            services.AddScoped<ITelemetriaRepositorio, TelemetriaRepositorio>();
            services.AddScoped<IEquipeRepositorio , EquipeRepositorio>();

            //IMPORTANTE, APRENDIDO DURANTE A AULA EM UMA DEPURAÇÃO
            //Durante a depuração constatei que o caminho feito da criação das instâncias é feita nessa ordem:
            //DBContext -> Repositório -> Controller
            //O Controller depende que já exista um Repositório construído/instanciado, 
            //este por sua vez depende que já exista o DBContext construído/instanciado


            //Passa a referência de todos os compilados que pertencem ao domínio para o AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
