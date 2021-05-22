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
            //Registro a classe que herda do DBContext utilizando o mecanismo de invers�o de depend�ncia
            //Em "ServiceLifetime.Scoped" � definido o tempo de vida do DBContext, atrelado ao escopo, de forma que...
            //... a inst�ncia do DBContext dura at� acabar a requisi��o
            services.AddDbContext<RallyDbContexto>(opt => opt.UseInMemoryDatabase("RallyDB"),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped);
                        
            //No servi�o de Controller, adicionado o servi�o do Newtonsoftjson, baixado no pacotes NUGET, da microsoft
            //junto tamb�m foi baixado o jsonpatch da Microsoft
            //creio que assim o controller vai ter acesso aos recursos do newtonsoftjson
            services.AddControllers()
                    .AddNewtonsoftJson();

            services.AddScoped<IPilotoRepositorio, PilotoRepositorio>();
            services.AddScoped<ITelemetriaRepositorio, TelemetriaRepositorio>();
            services.AddScoped<IEquipeRepositorio , EquipeRepositorio>();

            //IMPORTANTE, APRENDIDO DURANTE A AULA EM UMA DEPURA��O
            //Durante a depura��o constatei que o caminho feito da cria��o das inst�ncias � feita nessa ordem:
            //DBContext -> Reposit�rio -> Controller
            //O Controller depende que j� exista um Reposit�rio constru�do/instanciado, 
            //este por sua vez depende que j� exista o DBContext constru�do/instanciado


            //Passa a refer�ncia de todos os compilados que pertencem ao dom�nio para o AutoMapper
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
