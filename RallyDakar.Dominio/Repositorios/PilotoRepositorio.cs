using RallyDakar.Dominio.DBContexto;
using RallyDakar.Dominio.Entidades;
using RallyDakar.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RallyDakar.Dominio.Repositorios
{
    public class PilotoRepositorio : IPilotoRepositorio
    {
        private readonly RallyDBContexto _rallyDBContexto;

        public PilotoRepositorio(RallyDBContexto rallyDBContexto)
        {
            _rallyDBContexto = rallyDBContexto;
        }

        public void Adicionar(Piloto piloto)
        {
            _rallyDBContexto.Pilotos.Add(piloto);
            _rallyDBContexto.SaveChanges();
        }

        public IEnumerable<Piloto> ObterTodos()
        {
            return _rallyDBContexto.Pilotos.ToList();
        }

        public IEnumerable<Piloto> ObterTodosPilotos(string nome)
        {
            return _rallyDBContexto.Pilotos
                .Where(p => p.Nome.Contains(nome))
                .ToList();
        }
    }
}
