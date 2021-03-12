using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RallyDakar.API.Modelo;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System;

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
                _logger.LogInformation($"GET / Obter / Id: {id}");
                _logger.LogInformation($"Buscando o piloto na base. Id: {id}");
                var piloto = _pilotoRepositorio.Obter(id);
                if (piloto == null)
                {
                    _logger.LogWarning($"Piloto não encontrado. Id: {id}");
                    return NotFound();
                }

                _logger.LogInformation("Mapeando piloto -> pilotoModeloRetorno");
                var pilotoModeloRetorno = _mapper.Map<PilotoModelo>(piloto);

                _logger.LogInformation($"Retorando piloto. Ok. Id: {pilotoModeloRetorno.Id}");
                _logger.LogInformation($"Nome: {pilotoModeloRetorno.Nome}");
                _logger.LogInformation($"SobreNome: {pilotoModeloRetorno.SobreNome}");
                return Ok(pilotoModeloRetorno);                
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.ToString());
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPost]
        public IActionResult AdicionarPiloto([FromBody] PilotoModelo pilotoModelo)
        {
            try
            {
                _logger.LogInformation($"POST / AdicionarPiloto / Id: {pilotoModelo.Id}");
                //Cria o objeto piloto e passa os dados de forma igual o do objeto pilotoModelo
                //Isso é usado para não expor a entidade piloto, ao invés disso ele é substituído por um modelo
                _logger.LogInformation("Mapeando pilotoModelo -> piloto");
                var piloto = _mapper.Map<Piloto>(pilotoModelo);

                _logger.LogInformation($"Verificando se existe piloto com o id: { piloto.Id}");
                if (_pilotoRepositorio.Existe(piloto.Id))
                {
                    _logger.LogWarning($"Já existe piloto com a mesma identificação: {piloto.Id}");
                    return StatusCode(409, "Já existe piloto com a mesma identificação");
                }

                _logger.LogInformation("Adicionando piloto");
                _logger.LogInformation($"Nome: { piloto.Nome}");
                _logger.LogInformation($"Sobrenome: { piloto.SobreNome}");
                _pilotoRepositorio.Adicionar(piloto);
                _logger.LogInformation("Operação 'Adicionar Piloto' ocorreu sem falhas");

                //converte objeto da classe Piloto em objeto da classe PilotoModelo, é o oposto do que é feito no início do método
                _logger.LogInformation("Mapeando piloto -> pilotoModeloRetorno");
                var pilotoModeloRetorno = _mapper.Map<PilotoModelo>(piloto);

                //O CreatedAtRoute em questão faz:
                //Informa que o recurso foi criado (StatusCode 201)
                //Redireciona para o método cujo nome da rota é "Obter"
                //Passa como parâmetro o id do piloto adicionado
                //Envia também o objeto piloto       
                _logger.LogInformation("Chamando a rota 'Obter'");
                return CreatedAtRoute("Obter", new { id = piloto.Id }, pilotoModeloRetorno);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] PilotoModelo pilotoModelo)
        {
            try
            {
                _logger.LogInformation($"PUT / Atualizar / Id: {pilotoModelo.Id}");
                _logger.LogInformation($"Verificando se existe piloto com o id: { pilotoModelo.Id}");
                if (!_pilotoRepositorio.Existe(pilotoModelo.Id))
                {
                    _logger.LogWarning($"Piloto não encontrado. Id: {pilotoModelo.Id}");
                    return NotFound();
                }

                _logger.LogInformation("Mapeando pilotoModelo -> piloto");
                var piloto = _mapper.Map<Piloto>(pilotoModelo);

                _logger.LogInformation($"Atualizando piloto. Id: {piloto.Id}");
                _logger.LogInformation($"Nome: {piloto.Nome}");
                _logger.LogInformation($"Sobrenome: {piloto.SobreNome}");
                _pilotoRepositorio.Atualizar(piloto);

                //StatusCode 204
                //Não retorna nada além do StatusCode
                _logger.LogInformation($"Atualização de piloto concluída. Id: {piloto.Id}");
                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
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
                _logger.LogInformation($"PATCH / AtualizarParcialmente / Id: {id}");
                _logger.LogInformation($"Verificando se existe piloto com o id: { id}");
                if (!_pilotoRepositorio.Existe(id))
                {
                    _logger.LogWarning($"Piloto não encontrado. Id: {id}");
                    return NotFound();
                }

                _logger.LogInformation($"Buscando o piloto na base. Id: {id}");
                var piloto = _pilotoRepositorio.Obter(id);

                _logger.LogInformation($"Mapeando piloto -> pilotoModelo: {id}");
                var pilotoModelo = _mapper.Map<PilotoModelo>(piloto);

                _logger.LogInformation($"Aplicando alterações em pilotoModelo usando o Patch. Id: { pilotoModelo.Id }");
                //Os dados atualizados que estão no patchPiloto serão atualizados no objeto piloto
                patchPilotoModelo.ApplyTo(pilotoModelo);

                //Esse comando do Mapper não instancia uma nova entidade, ele usa a entidade já existente de piloto
                //Isso é diferente do comando que utilizaria <Piloto>(pilotoModelo)
                _logger.LogInformation("Mapeando pilotoModelo -> piloto");
                piloto = _mapper.Map(pilotoModelo, piloto);

                _logger.LogInformation($"Atualizando o piloto na base. Id: {piloto.Id}");
                _logger.LogInformation($"Nome: { piloto.Nome }");
                _logger.LogInformation($"SobreNome: { piloto.SobreNome }");
                _pilotoRepositorio.Atualizar(piloto);

                _logger.LogInformation("A atualização parcial do piloto foi concluída");
                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _logger.LogInformation($"DELETE / Deletar / Id: {id}");
                _logger.LogInformation($"Buscando o piloto na base. Id: {id}");
                var piloto = _pilotoRepositorio.Obter(id);

                _logger.LogInformation($"Verificando se piloto existe. Id: {id}");
                if (piloto == null)
                {
                    _logger.LogWarning($"Piloto não encontrado. Id: {id}");
                    return NotFound();
                }

                _logger.LogInformation($"Deletando piloto. Id: {piloto.Id}");
                _logger.LogInformation($"Nome: {piloto.Nome}");
                _logger.LogInformation($"SobreNome: {piloto.SobreNome}");
                _pilotoRepositorio.Deletar(piloto);

                _logger.LogInformation($"Deleção de piloto concluída. Id: {piloto.Id}");
                return NoContent();
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500, "Ocorreu um erro interno no sistema. Por favor entre em contato com o suporte");
            }
        }

        [HttpOptions]
        public IActionResult ListarOperacoesPermitidas()
        {
            _logger.LogInformation("OPTIONS / ListarOperacoesPermitidas");
            _logger.LogInformation("Listando operações permitidas");
            Response.Headers.Add("Allow", 
                "GET, POST, PUT, DELETE, PATCH, OPTIONS");

            return Ok();
        }
    }
}
