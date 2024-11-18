using NUnit.Framework;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Domain;
using Battle = Poke.Clases.Battle;
using Type = Poke.Clases.Type;

namespace LibraryTests
{
    [TestFixture]
    public class PlayersWaitListTest
    {
        private Trainer jugador1;
        private Trainer jugador2;
        private WaitList waitList;

        [SetUp]
        public void SetUp()
        {
            jugador1 = new Trainer("Jugador1");
            jugador1.AddPokemon(new Pokemon("Pikachu", 100, 10, "1", Type.PokemonType.Electric));
    
            jugador2 = new Trainer("Jugador2");
            jugador2.AddPokemon(new Pokemon("Charmander", 100, 10, "2", Type.PokemonType.Fire));
    
            waitList = new WaitList();
        }
        
        [Test]
        public void VerListaDeEspera_Test()
        {
            // Capturar la salida de la consola para verificar el mensaje
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Agregar jugadores a la lista de espera
            waitList.AddToWaitList(jugador1);
            waitList.AddToWaitList(jugador2);

            // Verificar que los jugadores fueron agregados a la lista de espera
            var playersInWaitList = waitList.CheckIn();
            Assert.That(playersInWaitList, Contains.Item(jugador1), "El jugador1 debería estar en la lista de espera.");
            Assert.That(playersInWaitList, Contains.Item(jugador2), "El jugador2 debería estar en la lista de espera.");

            // Verificar el mensaje de confirmación
            string output = consoleOutput.ToString();
            Assert.That(output, Contains.Substring("Jugador1 ha sido añadido a la lista de espera"), "El mensaje de confirmación debería indicar que el jugador1 fue agregado a la lista de espera.");
            Assert.That(output, Contains.Substring("Jugador2 ha sido añadido a la lista de espera"), "El mensaje de confirmación debería indicar que el jugador2 fue agregado a la lista de espera.");
        }
    }

    [TestFixture]
    public class IniciateBattleWithWaitListOpponentTest
    {
        private Trainer jugador1;
        private Trainer jugador2;
        private Battle battle;

        [SetUp]
        public void SetUp()
        {
            Attack ataque = new Attack("relámpago", 30, Type.PokemonType.Electric, false);
            List<Attack> attackList = new List<Attack> { ataque };

            jugador1 = new Trainer("Jugador 1");
            jugador1.AddPokemon(new Pokemon("Pikachu", 1, 10, "1", Type.PokemonType.Electric, attackList));
            jugador1.SetActualPokemon(jugador1.Pokemons[0]);

            jugador2 = new Trainer("Jugador 2");
            jugador2.AddPokemon(new Pokemon("Charizard", 1, 10, "2", Type.PokemonType.Fire, attackList));
            jugador2.SetActualPokemon(jugador2.Pokemons[0]);

            waitList = new WaitList(jugador1, jugador2);
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
                new Pokemon("Pikachu", 100, 10, "1", Type.PokemonType.Bug),
                new Pokemon("Charmander", 100, 10, "2", Type.PokemonType.Fire),
                new Pokemon("Bulbasaur", 100, 10, "3", Type.PokemonType.Plant),
                new Pokemon("Squirtle", 100, 10, "4", Type.PokemonType.Water),
                new Pokemon("Eevee", 100, 10, "5", Type.PokemonType.Normal),
                new Pokemon("Snorlax", 100, 10, "6", Type.PokemonType.Normal),
                new Pokemon("Jigglypuff", 100, 10, "7", Type.PokemonType.Flying)
            };

            jugador = new Trainer("Jugador 1");
            jugador.AddPokemon(catalogoPokemon[0]);        }

        [Test]
        public void Elegir6PokemonsTest()
        {
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
            Console.WriteLine("Selecciona tus 6 Pokémon:");
            while (jugador.Pokemons.Count < 6)
            {
                Console.WriteLine("Selecciona un Pokémon. Ingrese el numero del catalogo del Pokemon:");
                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine($"{i + 1}. {catalogoPokemon[i].Name}");
                }
                    var input = Console.ReadLine();
                    var inputSinEspacios = input.Replace(" ", "");
                    if (int.TryParse(inputSinEspacios, out int numeroPokemon))
                    {
                        if (numeroPokemon >= 1 && numeroPokemon <= catalogoPokemon.Count)
                        {
                            jugador.Pokemons.Add(catalogoPokemon[numeroPokemon - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Número inválido. Intente de nuevo.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Número inválido. Intente de nuevo.");
                    }
            }
            Assert.That(jugador.Pokemons.Count, Is.EqualTo(6), "El jugador debería tener 6 Pokémon.");
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
    
            batalla = new Battle(pokemon1, pokemonOponente);
        }
        
        [Test]
        public void IndicarDeQuienEsElTurno_Test()
        {
            // Inicializa el turno
            batalla.InitialTurn();

            // Verifica que el turno inicial sea del jugador 1 o 2
            Assert.That(batalla.Turn, Is.EqualTo(1).Or.EqualTo(2), "El turno inicial debe ser del jugador 1 o 2.");

            // Ejecuta el primer turno
            double turnoInicial = batalla.Turn;
            batalla.PlayTurn(jugador, oponente);

            // Verifica que el turno cambió al otro jugador
            Assert.That(batalla.Turn, Is.Not.EqualTo(turnoInicial), "El turno debería haber cambiado al otro jugador.");

            // Ejecuta otro turno y verifica que vuelve al jugador inicial
            batalla.PlayTurn(jugador, oponente);
            Assert.That(batalla.Turn, Is.EqualTo(turnoInicial), "El turno debería haber vuelto al jugador inicial.");
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
            jugador = new Trainer("Jugador1", pokemon1);
            oponente = new Trainer("Jugador2", pokemonOponente);
            pocion = new SuperPotion();
            jugador.AddItem(pocion);  // Suponiendo que el entrenador tiene una lista de ítems
            batalla = new Battle(pokemon1, pokemonOponente);
        }

        [Test]
        public void UsarItem_PierdeTurno_Test()
        {
            // Establece el turno inicial del jugador
            batalla.InitialTurn();
            double turnoInicial = batalla.Turn;

            // Asegura que es el turno del jugador antes de usar el ítem
            if (batalla.Turn == 1) // Si es el turno del jugador 1
            {
                // El jugador usa un ítem
                batalla.PlayTurn(jugador, oponente);
                pocion.Use(oponente.ActualPokemon);
                Assert.That(batalla.Turn, Is.EqualTo(2), "El turno debería pasar al oponente después de usar el ítem.");
            }
            else // Si es el turno del jugador 2
            {
                // El oponente usa un ítem
                pocion.Use(jugador.ActualPokemon);
                Assert.That(batalla.Turn, Is.EqualTo(1), "El turno debería pasar al jugador después de usar el ítem.");
            }
        }
    }

    [TestFixture]
    public class WaitTest
    {
        private Trainer jugador;
        private WaitList waitList;

        [SetUp]
        public void SetUp()
        {
            jugador = new Trainer("Jugador1", new Pokemon("Pikachu", 100, 10, "1", Type.PokemonType.Electric));
            waitList = new WaitList();
        }

        [Test]
        public void UnirseAListaDeEspera_Test()
        {
            // Capturar la salida de la consola para verificar el mensaje
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Agregar jugador a la lista de espera
            waitList.AddToWaitList(jugador);

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
            pokemonOponente = new Pokemon("Charmander", 0, 10, "2", Poke.Clases.Type.PokemonType.Fire); // Vida en cero para simular el fin de la batalla
            jugador = new Trainer("Jugador1", pokemonJugador);
            oponente = new Trainer("Jugador2", pokemonOponente);
            batalla = new Battle(pokemonJugador, pokemonOponente);
        }

        [Test]
        public void Batalla_Termina_Cuando_Vida_Oponente_Es_Cero_Test()
        {
            // Capturar la salida de la consola para verificar el mensaje final
            using (var consoleOutput = new StringWriter())
            {
                Console.SetOut(consoleOutput);

                // Ejecuta la batalla
                batalla.CompleteBattle(jugador, oponente);

                // Verificar que la batalla ha terminado
                Assert.That(batalla.BattleFinished(jugador, oponente) == true, "La batalla debería haber terminado porque la vida del oponente es cero.");

                // Verificar el mensaje de ganador
                string output = consoleOutput.ToString();
                Assert.That(output == ("El jugador 1 ha ganado"), "El mensaje final debería indicar que el jugador 1 ha ganado la batalla.");
            }
        }
    }
}