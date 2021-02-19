using Microsoft.AspNetCore.Mvc;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RallyDakar.API.Controllers
{
    [ApiController]
    [Route("api/pilotos")]
    public class PilotoController : ControllerBase
    {
        readonly IPilotoRepositorio _pilotoRepositorio;

        public PilotoController ( IPilotoRepositorio pilotoRepositorio)
        {
            _pilotoRepositorio = pilotoRepositorio;
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            //var pilotos = new List<Piloto>();
            //var piloto = new Piloto();
            //piloto.Id = 1;
            //piloto.Nome = "Piloto Teste";
            //piloto.EquipeId = 0;
            //piloto.Equipe = null;
            //pilotos.Add(piloto);
            //return Ok(pilotos);

            return Ok(_pilotoRepositorio.ObterTodos());
        }

        [HttpPost]
        public IActionResult AdicionarPiloto([FromBody]Piloto piloto)
        {
            _pilotoRepositorio.Adicionar(piloto);
            return Ok();
        }
    }
}
