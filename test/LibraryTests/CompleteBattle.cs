using NUnit.Framework;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Tests
{
    [TestFixture]
    public class BattleSimulationTest
    {
        [Test]
        public void SimulateFullBattle()
        {
            // Crear instancias de comandos
            var joinCommand = new JoinCommand();
            var battleCommand = new BattleCommand();
            var selectPokemonCommand = new SelectPokemonCommand();
            var useInitialPokemonCommand = new UseInitialPokemonCommand();
            var attackCommand = new AttackCommand();
            var usePotionCommand = new UsePotionCommand();
            var changePokemonCommand = new ChangePokemonCommand();

            // Simular nombres de jugadores
            string player1 = "Marto";
            string player2 = "Kike";

            // Paso 1: Ambos jugadores se unen a la lista de espera
            Facade.Instance.AddTrainerToWaitingList(player1);
            Facade.Instance.AddTrainerToWaitingList(player2);

            // Paso 2: Crear una batalla entre los dos jugadores
            var battleResult = Facade.Instance.CreateNewBattle(player1, player2);
            Assert.That(battleResult, Is.Not.Null, "La batalla no se pudo iniciar correctamente");

            // Paso 3: Seleccionar Pokémon para cada jugador
            string player1Selection = "1 2 3 4 5 6"; // Índices simulados
            string player2Selection = "7 8 9 10 11 12"; // Índices simulados
            var player1SelectResult = Facade.Instance.PokemonSelection(player1, player1Selection);
            var player2SelectResult = Facade.Instance.PokemonSelection(player2, player2Selection);
            Assert.That(player1SelectResult.ReadyForBattleMessage, Is.Not.Null, "Player1 no seleccionó 6 Pokémon");
            Assert.That(player2SelectResult.ReadyForBattleMessage, Is.Not.Null, "Player2 no seleccionó 6 Pokémon");

            // Paso 4: Elegir Pokémon inicial
            Facade.Instance.AssignActualPokemon(player1, "Bulbasaur");
            Facade.Instance.AssignActualPokemon(player2, "Charmander");

            // Paso 5: Simular turnos hasta que uno de los jugadores gane
            string winner = null;
            while (winner == null)
            {
                // Turno de Player1
                var player1Attack = Facade.Instance.AttackPokemon(player1, "Tackle");
                Assert.That(player1Attack, Is.Not.Null, "El ataque de Player1 no se ejecutó correctamente");

                // Comprobar si alguien ganó
                winner = Facade.Instance.GetBattleResult(player1, player2);
                if (winner != null) break;

                // Turno de Player2
                var player2Attack = Facade.Instance.AttackPokemon(player2, "Ember");
                Assert.That(player2Attack, Is.Not.Null, "El ataque de Player2 no se ejecutó correctamente");

                // Comprobar si alguien ganó
                winner = Facade.Instance.GetBattleResult(player1, player2);
            }

            // Validar que la batalla terminó y hay un ganador
            Assert.That(winner, Is.Not.Null, "La batalla no tiene un ganador");
            Console.WriteLine($"El ganador es: {winner}");
        }
    }
}
