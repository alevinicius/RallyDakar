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
            try {

                var pilotos = _pilotoRepositorio.ObterTodos();
                if (!pilotos.Any())
                {
                    return NotFound();
                }

                return Ok(pilotos);
            }catch//(Exception e)
            {
                //Exemplo de como salvar a mensagem de erro em um log, caso o tenha.
                //_logger.Info(e.ToString());

                //Mandar mensagem genérica para o cliente, caso seja empresa externa ou cliente final
                //return BadRequest("Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");

                //Também posso colocar o statuscode diretamente, passando o número e a mensagem.
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPost]
        public IActionResult AdicionarPiloto([FromBody]Piloto piloto)
        {
            _pilotoRepositorio.Adicionar(piloto);
            return Ok();
        } 

        [HttpPut]
        public IActionResult AtualizarPiloto([FromBody] Piloto piloto)
        {
            return Ok();
        }

        [HttpPatch]
        public IActionResult AtualizarParcialmentePiloto([FromBody] Piloto piloto)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPiloto(int Id)
        {
            return Ok();
        }
    }
}
