using RallyDakar.Dominio.Entidades;
using System.Collections.Generic;

namespace RallyDakar.Dominio.Interfaces
{
    public interface IPilotoRepositorio
    {
        void Adicionar(Piloto piloto);
        IEnumerable<Piloto> ObterTodos();
        Piloto Obter(int pilotoId);
        bool Existe(int pilotoId);
        void Atualizar(Piloto piloto);
        void Deletar(Piloto piloto);
    }
}
