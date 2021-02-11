using Microsoft.VisualStudio.TestTools.UnitTesting;
using RallyDakar.Dominio.Entidades;
using System.Linq;

namespace RallyDakar.Dominio.Testes
{
    [TestClass]
    public class AdicionarDuasEquipesTeste
    {
        Temporada temporada;
        Equipe equipe1;
        Equipe equipe2;
        Equipe equipe3;

        [TestInitialize]
        public void Initialize()
        {
            temporada = new Temporada
            {
                Id = 1,
                Nome = "Temporada2020"
            };

            equipe1 = new Equipe
            {
                Id = 1,
                Nome = "EquipeTeste1"
            };

            equipe2 = new Equipe
            {
                Id = 2,
                Nome = "EquipeTeste2"
            };

            equipe3 = null;

            temporada.AdicionarEquipe(equipe1);
            temporada.AdicionarEquipe(equipe2);
            temporada.AdicionarEquipe(equipe3);
        }

        [TestMethod]
        public void DuasEquipesAdicionadasCorretamente()
        {
            Assert.IsTrue(temporada.Equipes.Count() == 2);
        }
    }
}
