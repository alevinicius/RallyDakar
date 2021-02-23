using Microsoft.EntityFrameworkCore.Internal;
using RallyDakar.Dominio.DBContexto;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RallyDakar.Dominio.Repositorios
{
    public class PilotoRepositorio : IPilotoRepositorio
    {
        private readonly RallyDBContexto _rallyDbContexto;
        public PilotoRepositorio(RallyDBContexto rallyDbContexto)
        {
            _rallyDbContexto = rallyDbContexto;
        }

        public void Adicionar(Piloto piloto)
        {
            _rallyDbContexto.Pilotos.Add(piloto);
            _rallyDbContexto.SaveChanges();
        }

        public void Atualizar(Piloto piloto)
        {

            //O objeto piloto foi recebido pelo cliente, portanto se trata de uma instância não gerenciada...
            //...pelo EntityFrameweork, por ser apenas uma instância que está em memória que veio de fora
            //O Attach faz com que a instância passe a ser gerenciada pelo EntityFramework
            //Sem o Attach o EntityFramework entenderia como um objeto a ser adicionado ao executar o SaveChanges
            _rallyDbContexto.Attach(piloto);

            //Está mudando o estado de todas as propriedades da instância para "Modified"
            _rallyDbContexto.Entry(piloto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            
            _rallyDbContexto.SaveChanges();
        }

        public void Deletar(Piloto piloto)
        {
            _rallyDbContexto.Pilotos.Remove(piloto);
            _rallyDbContexto.SaveChanges();
        }

        public bool Existe(int pilotoId)
        {
            return _rallyDbContexto.Pilotos.Any(p => p.Id == pilotoId);
        }

        public Piloto Obter(int pilotoId)
        {
            return _rallyDbContexto.Pilotos.FirstOrDefault(p => p.Id == pilotoId);
        }

        public IEnumerable<Piloto> ObterTodos()
        {
            return _rallyDbContexto.Pilotos.ToList();
        }


        public IEnumerable<Piloto> ObterTodosPilotos(string nome)
        {
            return _rallyDbContexto.Pilotos
                .Where(p => p.Nome.Contains(nome))
                .ToList();
        }


    }
}
