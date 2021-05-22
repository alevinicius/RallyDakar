using RallyDakar.Dominio.Entidades;
using System.Collections.Generic;

namespace RallyDakar.Dominio.Interfaces
{
    public interface IEquipeRepositorio
    {
        void Adicionar(Equipe equipe);
        void Atualizar(Equipe equipe);
        void Deletar(Equipe equipe);
        bool Existe(int equipeId);
        Equipe Obter(int equipeId);
        IEnumerable<Equipe> ObterTodos();
        IEnumerable<Equipe> ObterTodosEquipes(string nome);
    }
}
