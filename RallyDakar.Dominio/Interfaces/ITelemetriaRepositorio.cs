using RallyDakar.Dominio.Entidades;
using System.Collections.Generic;

namespace RallyDakar.Dominio.Interfaces
{
    public interface ITelemetriaRepositorio
    {
        void Adicionar(Telemetria telemetria);
        void Atualizar(Telemetria telemetria);
        void Deletar(Telemetria telemetria);
        bool Existe(int telemetriaId);
        Telemetria Obter(int telemetriaId);
        IEnumerable<Telemetria> ObterTodos();
        IEnumerable<Telemetria> ObterTodosPorEquipe(int equipeId);
        Telemetria ObterPor(int equipeId, int telemetriaId);
    }
}
