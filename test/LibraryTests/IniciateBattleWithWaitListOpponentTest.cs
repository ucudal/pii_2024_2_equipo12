using NUnit.Framework;
using Poke.Clases;
using Type = Poke.Clases.Type;
using System.IO;

namespace LibraryTests
{
    [TestFixture]
    public class IniciateBattleWithWaitListOpponentTest
    {
        private OriginalTrainer jugador1;
        private OriginalTrainer jugador2;
        private Battle battle;

        [SetUp]
        public void SetUp()
        {
            Attack ataque = new Attack("relámpago", 30, Type.PokemonType.Electric, false);
            List<Attack> attackList = new List<Attack>();
            attackList.Add(ataque);
            jugador1 = new OriginalTrainer("Jugador 1", new Pokemon("Pikachu", 1, 10, "1", Type.PokemonType.Electric, attackList));
            jugador2 = new OriginalTrainer("Jugador 2", new Pokemon("Charizard", 1, 10, "2", Type.PokemonType.Fire, attackList));
            jugador1.Pokemons.Add(jugador1.ActualPokemon);
            jugador2.Pokemons.Add(jugador2.ActualPokemon);
            WaitList waitList = new WaitList(jugador1, jugador2);
            battle = new Battle(jugador1.ActualPokemon, jugador2.ActualPokemon, waitList);
        }

        [Test]
        public void NotificarInicioDeBatalla_Test()
        {
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            battle.CompleteBattle(jugador1, jugador2, "1","0");
            Assert.That(consoleOutput.ToString(), Contains.Substring("El jugador 1 comienza la batalla"));
            Assert.That(consoleOutput.ToString(), Contains.Substring("El jugador 2 comienza la batalla"));
        }

        [Test]
        public void DeterminarTurnoAleatorio_Test()
        {
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            string turnoInicial = "";
            for (int i = 0; i < 10; i++)
            {
                battle.CompleteBattle(jugador1, jugador2);
                if (consoleOutput.ToString().Contains("El jugador 1 comienza la batalla"))
                {
                    turnoInicial = "Jugador 1";
                }
                else if (consoleOutput.ToString().Contains("El jugador 2 comienza la batalla"))
                {
                    turnoInicial = "Jugador 2";
                }

                Assert.That(turnoInicial, Is.Not.Empty);
            }
        }
    }
}