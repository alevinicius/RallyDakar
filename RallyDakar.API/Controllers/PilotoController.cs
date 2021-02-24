using Microsoft.AspNetCore.JsonPatch;
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

        public PilotoController(IPilotoRepositorio pilotoRepositorio)
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
            } catch//(Exception e)
            {
                //Exemplo de como salvar a mensagem de erro em um log, caso o tenha.
                //_logger.Info(e.ToString());

                //Mandar mensagem genérica para o cliente, caso seja empresa externa ou cliente final
                //return BadRequest("Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");

                //Também posso colocar o statuscode diretamente, passando o número e a mensagem.
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        //Espera um Id, e "Name = Obter" é o nome da rota, que é usado no método Post para..."
        //...redirecionar à este método
        [HttpGet("{id}", Name = "Obter")]
        public IActionResult Obter(int id)
        {
            try
            {
                var piloto = _pilotoRepositorio.Obter(id);
                if (piloto == null)
                {
                    return NotFound();
                }

                return Ok(piloto);
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPost]
        public IActionResult AdicionarPiloto([FromBody] Piloto piloto)
        {
            try
            {
                if (_pilotoRepositorio.Existe(piloto.Id))
                {
                    return StatusCode(409, "Já existe piloto com a mesma identificação");
                }

                _pilotoRepositorio.Adicionar(piloto);

                //O CreatedAtRoute em questão faz:
                //Informa que o recurso foi criado (StatusCode 201)
                //Redireciona para o método cujo nome da rota é "Obter"
                //Passa como parâmetro o id do piloto adicionado
                //Envia também o objeto piloto                
                return CreatedAtRoute("Obter", new { id = piloto.Id }, piloto);
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] Piloto piloto)
        {
            try
            {
                if (!_pilotoRepositorio.Existe(piloto.Id))
                {
                    return NotFound();
                }

                _pilotoRepositorio.Atualizar(piloto);

                //StatusCode 204
                //Não retorna nada além do StatusCode
                return NoContent();
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        //Conforme pode ver na declaração, está sendo utilizado recurso do pacote JsonPatch, adicionado ao projeto RallyDakar.API pelo...
        //...gerenciador de pacotes NUGET
        //Exemplo de json recebido para atualizar o nome e o sobrenome de um piloto, percebe-se que é uma lista com 2 elementos, uma pra cada campo:
        /*                
    [
        {
            "op":"replace",
            "path": "/Nome",
            "value":"Fátima Angélica"
        },
        {
            "op":"replace",
            "path": "/sobreNome",
            "value":"Aranha"
        }        
    ]
         */
        //"op" se refere a operação a ser realizada, que no caso é "replace", ou seja, substituição do valor
        //"path" se refere ao campo que sofrerá o replace (terá o valor substituido), deve ser escrito após a barra conforme o exemplo.
        //"value" se refere ao valor que será colocado no campo
        //O id da entidade a ser atualizada deve ser enviada junto
        [HttpPatch("{id}")]
        public IActionResult AtualizarParcialmente(int id, [FromBody] JsonPatchDocument<Piloto> patchPiloto)
        {
            try
            {
                if (!_pilotoRepositorio.Existe(id))
                {
                    return NotFound();
                }

                var piloto = _pilotoRepositorio.Obter(id);
                
                //Os dados atualizados que estão no patchPiloto serão atualizados no objeto piloto
                patchPiloto.ApplyTo(piloto);

                _pilotoRepositorio.Atualizar(piloto);

                return NoContent();
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                var piloto = _pilotoRepositorio.Obter(id);

                if (piloto == null)
                {
                    return NotFound();
                }
                
                _pilotoRepositorio.Deletar(piloto);

                return NoContent();
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }
    }
}
