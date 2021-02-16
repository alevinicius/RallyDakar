using RallyDakar.Dominio.DBContexto;
using RallyDakar.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RallyDakar.Dominio.Repositorios
{
    public class PilotoRepositorio
    {
        private readonly RallyDBContexto _rallyDBContexto;

        public PilotoRepositorio(RallyDBContexto rallyDBContexto)
        {
            _rallyDBContexto = rallyDBContexto;
        }

        public void AdicionarPiloto(Piloto piloto)
        {
            _rallyDBContexto.Pilotos.Add(piloto);
            _rallyDBContexto.SaveChanges();
        }

        public IEnumerable<Piloto> ObterTodosPilotos()
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
