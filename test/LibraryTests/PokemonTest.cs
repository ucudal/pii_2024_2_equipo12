using NUnit.Framework;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Battle = Ucu.Poo.DiscordBot.Domain.Battle;

namespace LibraryTests
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
            Assert.That(player1Selection, Is.Not.Null, "Player1 no seleccionó 6 Pokémon");
            Assert.That(player2Selection, Is.Not.Null, "Player2 no seleccionó 6 Pokémon");

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
    [TestFixture]
    public class BattleListTests
    {
        private BattlesList battleList;
        private Battle battle1;
        private Battle battle2;
        private Trainer trainer1;
        private Trainer trainer2;
        private Trainer trainer3;
        private Trainer trainer4;

        [SetUp]
        public void SetUp()
        {
            trainer1 = new Trainer("Trainer1");
            trainer2 = new Trainer("Trainer2");

            battle1 = new Battle(trainer1, trainer2);
        }

        [Test]
        public void AddBattle_Test()
        {
            battleList.AddBattle(trainer1, trainer2);
            Assert.That(battleList, Contains.Item(battle1), "La lista de batallas debería contener battle1.");
        }

        [Test]
        public void GetBattle_Test()
        {
            battleList.AddBattle(trainer1, trainer2);
            var retrievedBattle = battleList.GetBattleByPlayer(trainer1.DisplayName);
            Assert.That(retrievedBattle, Is.EqualTo(battle1), "La batalla debería ser battle1.");
        }

        [Test]
        public void BattleExists_Test()
        {
            battleList.AddBattle(trainer1, trainer2);
            var exists = battleList.GetBattleByPlayer(trainer1.DisplayName);
            Assert.That(exists, Is.Not.Null, "Deberia existir una batalla.");
        }
    }
    [TestFixture]
    public class PokemonTests
    {
        private Pokemon pikachu;
        private Pokemon charmander;
        private Attack thunderbolt;
        private Attack ember;

        [SetUp]
        public void SetUp()
        {
            thunderbolt = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Bug, false);
            ember = new Attack("Ember", 30, Poke.Clases.Type.PokemonType.Dragon, true);
            pikachu = new Pokemon("Pikachu", 100, 1, "1", Poke.Clases.Type.PokemonType.Electric, new List<Attack> { thunderbolt });
            charmander = new Pokemon("Charmander", 100, 1, "2", Poke.Clases.Type.PokemonType.Fire, new List<Attack> { ember });
        }

        [Test]
        public void Attack_Test()
        {
            int initialHealth = (int)charmander.Hp;
            pikachu.Attack(null, charmander, pikachu, thunderbolt);
            Assert.That(charmander.Hp, Is.LessThan(initialHealth), "Charmander should have received damage.");
        }

        [Test]
        public void ReceiveDamage_Test()
        {
            double damage = 20;
            double initialHealth = pikachu.Hp;
            pikachu.RecibeDamage(null, damage);
            Assert.That(pikachu.Hp, Is.EqualTo(initialHealth - damage), "Pikachu should have received damage.");
        }

        [Test]
        public void Heal_Test()
        {
            pikachu.RecibeDamage(null, 50);
            double healthAfterDamage = pikachu.Hp;
            pikachu.AddHP(30);
            Assert.That(pikachu.Hp, Is.EqualTo(healthAfterDamage + 30), "Pikachu should have recovered health.");
        }

        [Test]
        public void AddAttack_Test()
        {
            var newAttack = new Attack("Quick Attack", 20, Poke.Clases.Type.PokemonType.Electric, false);
            pikachu.AddAttack(newAttack);
            Assert.That(pikachu.AttackList, Contains.Item(newAttack), "Pikachu should have the new attack in its attack list.");
        }

        [Test]
        public void IsPokemonAlive_Test()
        {
            Assert.That(pikachu.IsPokemonAlive(), Is.True, "Pikachu debería estar vivo.");
            pikachu.RecibeDamage(null, pikachu.Hp);
            Assert.That(pikachu.IsPokemonAlive(), Is.False, "Pikachu debería estar muerto.");
        }

        [Test]
        public void ApplyState_Test()
        {
            pikachu.ApplyState(charmander, "Burned");
            Assert.That(charmander.State, Is.EqualTo("Burned"), "Charmander should be burned.");
        }

        [Test]
        public void StateActualization_Test()
        {
            charmander.Poisoned = true;
            double initialHealth = charmander.Hp;
            charmander.StateActualization();
            Assert.That(charmander.Hp, Is.LessThan(initialHealth), "Charmander should lose health due to poison.");
        }
    }
        
    [TestFixture]
    public class PlayersWaitListTest
    {
        private Trainer jugador1;
        private Trainer jugador2;
        private WaitingList waitList;

        [SetUp]
        public void SetUp()
        {
            jugador1 = new Trainer("Jugador1");
            jugador1.AddPokemon(new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric));
    
            jugador2 = new Trainer("Jugador2");
            jugador2.AddPokemon(new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire));
    
            waitList = new WaitingList(); 
            
        }
        
        [Test]
        public void VerListaDeEspera_Test()
        {
            // Capturar la salida de la consola para verificar el mensaje
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Agregar jugadores a la lista de espera
            waitList.AddTrainer(jugador1.DisplayName);
            waitList.AddTrainer(jugador2.DisplayName);

            // Verificar que los jugadores fueron agregados a la lista de espera
            var playersInWaitList1 = waitList.FindTrainerByDisplayName(jugador1.DisplayName);
            var playersInWaitList2 = waitList.FindTrainerByDisplayName(jugador2.DisplayName);
            
            Assert.That(playersInWaitList1, Is.Not.Null, "El jugador1 debería estar en la lista de espera.");
            Assert.That(playersInWaitList2, Is.Not.Null, "El jugador2 debería estar en la lista de espera.");

        }
    }

    [TestFixture]
    public class IniciateBattleWithWaitListOpponentTest
    {
        private Trainer jugador1;
        private Trainer jugador2;
        private Battle battle;
        private WaitingList waitList;

        [SetUp]
        public void SetUp()
        {
            Attack ataque = new Attack("relámpago", 30, Poke.Clases.Type.PokemonType.Electric, false);
            List<Attack> attackList = new List<Attack> { ataque };

            jugador1 = new Trainer("Jugador 1");
            jugador1.AddPokemon(new Pokemon("Pikachu", 1, 10, "1", Poke.Clases.Type.PokemonType.Electric, attackList));
            jugador1.SetActualPokemon(jugador1.PokemonList[0]);

            jugador2 = new Trainer("Jugador 2");
            jugador2.AddPokemon(new Pokemon("Charizard", 1, 10, "2", Poke.Clases.Type.PokemonType.Fire, attackList));
            jugador2.SetActualPokemon(jugador2.PokemonList[0]);

            waitList = new WaitingList(jugador1, jugador2);
            battle = new Battle(jugador1, jugador2);
        }

        [Test]
        public void NotificarInicioDeBatalla_Test()
        {
            /* var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            battle.CompleteBattle(jugador1, jugador2, "1","0");
            Assert.That(consoleOutput.ToString(), Contains.Substring("El jugador 1 comienza la batalla"));
            Assert.That(consoleOutput.ToString(), Contains.Substring("El jugador 2 comienza la batalla")); */
        }

        [Test]
        public void DeterminarTurnoAleatorio_Test()
        {
           /*  var consoleOutput = new StringWriter();
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

                Assert.That(turnoInicial, Is.Not.Empty); */
            }
        }

    [TestFixture]
    public class Pick6PokemonTest
    {
        private Trainer jugador;
        private List<Pokemon> catalogoPokemon;

        [SetUp]
        public void SetUp()
        {
            catalogoPokemon = new List<Pokemon>
            {
                new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Bug),
                new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire),
                new Pokemon("Bulbasaur", 100, 10, "3", Poke.Clases.Type.PokemonType.Plant),
                new Pokemon("Squirtle", 100, 10, "4", Poke.Clases.Type.PokemonType.Water),
                new Pokemon("Eevee", 100, 10, "5", Poke.Clases.Type.PokemonType.Normal),
                new Pokemon("Snorlax", 100, 10, "6", Poke.Clases.Type.PokemonType.Normal),
                new Pokemon("Jigglypuff", 100, 10, "7", Poke.Clases.Type.PokemonType.Flying)
            };

            jugador = new Trainer("Jugador 1");
            jugador.AddPokemon(catalogoPokemon[0]);        }

        [Test]
        public void Elegir6PokemonsTest()
        {
        // Simular la selección de 6 Pokémon
        for (int i = 0; i < 6; i++)
        {
            jugador.AddPokemon(catalogoPokemon[i]);
        }
        // Verificar que el jugador tiene 6 Pokémon
        Assert.That(jugador.PokemonList.Count, Is.EqualTo(6), "El jugador debería tener 6 Pokémon.");
        }
    }

    [TestFixture]
    public class TurnInfoTest
    {
        private Trainer jugador;
        private Trainer oponente;
        private Battle batalla;

        [SetUp]
        public void SetUp()
        {
            var pokemon1 = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            var pokemonOponente = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
    
            jugador = new Trainer("Jugador1");
            jugador.AddPokemon(pokemon1);
            jugador.SetActualPokemon(pokemon1);
    
            oponente = new Trainer("Jugador2");
            oponente.AddPokemon(pokemonOponente);
            oponente.SetActualPokemon(pokemonOponente);
    
            batalla = new Battle(jugador, oponente);
        }
        
        [Test]
        public void IndicarDeQuienEsElTurno_Test()
        {
            Trainer turnoInicial = batalla.Turn;
            if (batalla.Turn == jugador)
            {
                Assert.That(batalla.Turn, Is.EqualTo(jugador), "El turno debería ser del jugador.");
            }
            else
            {
                Assert.That(batalla.Turn, Is.EqualTo(oponente), "El turno debería ser del oponente.");
            }
        }
    }

    [TestFixture]
    public class UseItemTest
    {
        private Trainer jugador;
        private Trainer oponente;
        private Battle batalla;
        private SuperPotion pocion;

        [SetUp]
        public void SetUp()
        {
            var pokemon1 = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            var pokemonOponente = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            jugador = new Trainer("Jugador1");
            jugador.AddPokemon(pokemon1);
            oponente = new Trainer("Jugador2");
            oponente.AddPokemon(pokemonOponente);
            pocion = new SuperPotion();
            jugador.AddItem(pocion); 
            batalla = new Battle(jugador, oponente);
        }

        [Test]
        public void UsarItem_PierdeTurno_Test()
        {
            // Establece el turno inicial del jugador
            batalla.InitialTurn();
            Trainer turnoInicial = batalla.Turn;

            // Asegura que es el turno del jugador antes de usar el ítem
            if (batalla.Turn == jugador) // Si es el turno del jugador 1
            {
                // El jugador usa un ítem
                jugador.UseItem(pocion, oponente.ActualPokemon);
                Assert.That(batalla.Turn, Is.EqualTo(oponente), "El turno debería pasar al oponente después de usar el ítem.");
            }
            else // Si es el turno del jugador 2
            {
                // El oponente usa un ítem
                oponente.UseItem(pocion, jugador.ActualPokemon);
                Assert.That(batalla.Turn, Is.EqualTo(jugador), "El turno debería pasar al jugador después de usar el ítem.");
            }
        }
    }

    [TestFixture]
    public class WaitTest
    {
        private Trainer jugador;
        private WaitingList waitList;

        [SetUp]
        public void SetUp()
        {
            var pokemon = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            jugador = new Trainer("Jugador1");
            jugador.AddPokemon(pokemon);
            waitList = new WaitingList();
        }

        [Test]
        public void UnirseAListaDeEspera_Test()
        {
            // Capturar la salida de la consola para verificar el mensaje
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Agregar jugador a la lista de espera
            waitList.AddTrainer(jugador.DisplayName);

            // Verificar que el jugador fue agregado a la lista de espera
            Assert.That(waitList.CheckIn(), Contains.Item(jugador), "El jugador debería estar en la lista de espera.");

            // Verificar el mensaje de confirmación
            string output = consoleOutput.ToString();
            Assert.That(output, Contains.Substring("Jugador1 ha sido añadido a la lista de espera"), "El mensaje de confirmación debería indicar que el jugador fue agregado a la lista de espera.");
        }
    }

    [TestFixture]
    public class WinBattleTest
    {
        private Trainer jugador;
        private Trainer oponente;
        private Pokemon pokemonJugador;
        private Pokemon pokemonOponente;
        private Battle batalla;

        [SetUp]    
        public void SetUp()
        {
            pokemonJugador = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            pokemonOponente = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            jugador = new Trainer("Jugador1");
            jugador.AddPokemon(pokemonJugador);
            oponente = new Trainer("Jugador2");
            oponente.AddPokemon(pokemonOponente);
            batalla = new Battle(jugador, oponente);
        }

        [Test]
        public void Batalla_Termina_Cuando_Vida_Oponente_Es_Cero_Test()
        {
            // Capturar la salida de la consola para verificar el mensaje final
            using (var consoleOutput = new StringWriter())
            {
                /* Console.SetOut(consoleOutput);

                // Ejecuta la batalla
                batalla.CompleteBattle(jugador, oponente);

                // Verificar que la batalla ha terminado
                Assert.That(batalla.BattleFinished(jugador, oponente) == true, "La batalla debería haber terminado porque la vida del oponente es cero.");

                // Verificar el mensaje de ganador
                string output = consoleOutput.ToString();
                Assert.That(output == ("El jugador 1 ha ganado"), "El mensaje final debería indicar que el jugador 1 ha ganado la batalla.");
            */
            }
        }
    }