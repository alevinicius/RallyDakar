using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RallyDakar.API.Modelo;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;

namespace RallyDakar.API.Controllers
{
    [ApiController]
    [Route("api/pilotos")]
    public class PilotoController : ControllerBase
    {
        private readonly IPilotoRepositorio _pilotoRepositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<PilotoController> _logger;

        public PilotoController(IPilotoRepositorio pilotoRepositorio, IMapper mapper, ILogger<PilotoController> logger)
        {
            _pilotoRepositorio = pilotoRepositorio;
            _mapper = mapper;
            _logger = logger;
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

                var pilotoModeloRetorno = _mapper.Map<PilotoModelo>(piloto);

                return Ok(pilotoModeloRetorno);
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPost]
        public IActionResult AdicionarPiloto([FromBody] PilotoModelo pilotoModelo)
        {
            try
            {
                //Adiciona informação ao Log
                _logger.LogInformation("Adicionando um piloto novo");

                //Cria o objeto piloto e passa os dados de forma igual o do objeto pilotoModelo
                //Isso é usado para não expor a entidade piloto, ao invés disso ele é substituído por um modelo
                var piloto = _mapper.Map<Piloto>(pilotoModelo);

                if (_pilotoRepositorio.Existe(piloto.Id))
                {
                    return StatusCode(409, "Já existe piloto com a mesma identificação");
                }

                _pilotoRepositorio.Adicionar(piloto);

                
                //converte objeto da classe Piloto em objeto da classe PilotoModelo, é o oposto do que é feito no início do método
                var pilotoModeloRetorno = _mapper.Map<PilotoModelo>(piloto);

                //O CreatedAtRoute em questão faz:
                //Informa que o recurso foi criado (StatusCode 201)
                //Redireciona para o método cujo nome da rota é "Obter"
                //Passa como parâmetro o id do piloto adicionado
                //Envia também o objeto piloto              
                return CreatedAtRoute("Obter", new { id = piloto.Id }, pilotoModeloRetorno);
            }
            catch//(Exception e)
            {
                //_logger.info(e.ToString())
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] PilotoModelo pilotoModelo)
        {
            try
            {
                if (!_pilotoRepositorio.Existe(pilotoModelo.Id))
                {
                    return NotFound();
                }

                var piloto = _mapper.Map<Piloto>(pilotoModelo);

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
        public IActionResult AtualizarParcialmente(int id, [FromBody] JsonPatchDocument<PilotoModelo> patchPilotoModelo)
        {
            try
            {
                if (!_pilotoRepositorio.Existe(id))
                {
                    return NotFound();
                }

                var piloto = _pilotoRepositorio.Obter(id);
                var pilotoModelo = _mapper.Map<PilotoModelo>(piloto);
               
                //Os dados atualizados que estão no patchPiloto serão atualizados no objeto piloto
                patchPilotoModelo.ApplyTo(pilotoModelo);

                //Esse comando do Mapper não instancia uma nova entidade, ele usa a entidade já existente de piloto
                //Isso é diferente do comando que utilizaria <Piloto>(pilotoModelo)
                piloto = _mapper.Map(pilotoModelo, piloto);
                
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

        [HttpOptions]
        public IActionResult ListarOperacoesPermitidas()
        {
            Response.Headers.Add("Allow", 
                "GET, POST, PUT, DELETE, PATCH, OPTIONS");

            return Ok();
        }
    }
}
