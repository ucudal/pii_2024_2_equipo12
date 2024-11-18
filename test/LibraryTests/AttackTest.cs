using NUnit.Framework;
using Poke.Clases;
using Type = Poke.Clases.Type;

namespace LibraryTests;

[TestFixture]
public class AttackTest
{
    
    /*  Como jugador, quiero atacar en mi turno y hacer daño basado en la efectividad de los tipos de Pokémon.
        Criterios de aceptación:
        El jugador puede seleccionar un ataque que inflige daño basado en la efectividad de tipos.
        El sistema aplica automáticamente la ventaja o desventaja del tipo de ataque. */
    
    [Test]
    public void TestDamageBasedOnTypeEffectiveness()
    {
        // Crear Pokémon
        Pokemon firePokemon = new Pokemon("Charmander", 100, 1, null, Type.PokemonType.Fire);
        Pokemon waterPokemon = new Pokemon("Squirtle", 100, 1, null, Type.PokemonType.Water);
        Pokemon plantPokemon = new Pokemon("Bulbasaur", 100, 1, null, Type.PokemonType.Plant);

        // Crear ataques
        NormalAttack fireAttack = new NormalAttack("Fire Blast", 40,  Type.PokemonType.Fire, false);
        NormalAttack waterAttack = new NormalAttack("Water Gun", 40, Type.PokemonType.Water, false );
        NormalAttack plantAttack = new NormalAttack("Vine Whip", 40, Type.PokemonType.Plant, false );

        // Aplicar ventaja de tipo: Fire > Plant
        double expectedFireDamage = fireAttack.CalculateDamage(plantPokemon);
        plantPokemon.RecibeDamage(expectedFireDamage);
        Assert.That(plantPokemon.GetHp(), Is.EqualTo(20));

        // Aplicar ventaja de tipo: Water > Fire
        double expectedWaterDamage = waterAttack.CalculateDamage(firePokemon);
        firePokemon.RecibeDamage(expectedWaterDamage);
        Assert.That(firePokemon.GetHp(), Is.EqualTo(20));

        // Aplicar desventaja de tipo: Water < Plant
        double expectedPlantDamage = plantAttack.CalculateDamage(waterPokemon);
        waterPokemon.RecibeDamage(expectedPlantDamage);
        Assert.That(waterPokemon.GetHp(), Is.EqualTo(20));
    }
}


[TestFixture]
public class AvailableAttacksTest
{
    private OriginalTrainer jugador;
    private Pokemon pokemon;
    private Attack ataqueBasico;
    private Attack ataqueEspecial;
    private Battle battle; // Declaramos la instancia de Battle

    [SetUp]
    public void SetUp()
    {
        ataqueBasico = new Attack("Ataque Básico", 10, Type.PokemonType.Water, false);
        ataqueEspecial = new Attack("Ataque Especial", 20, Type.PokemonType.Electric, true);
        pokemon = new Pokemon("Pikachu", 100, 10, "1", Type.PokemonType.Electric);
        pokemon.AttackList = new List<Attack> { ataqueBasico, ataqueEspecial };
        jugador = new OriginalTrainer("Jugador1", pokemon);

        // Creación de la instancia de Battle
        battle = new Battle(pokemon, new Pokemon("Charmander", 100, 10, "2", Type.PokemonType.Fire));
    }

    [Test]
    public void MostrarAtaquesDisponibles_Test()
    {
        // Verificar que se muestran los ataques para el turno actual
        List<Attack> ataquesDisponibles = pokemon.AttackList;
        Assert.That(ataquesDisponibles, Does.Contain(ataqueBasico));
        Assert.That(ataquesDisponibles, Does.Contain(ataqueEspecial));
    }

    /*[Test]
    public void AtaqueEspecial_RestriccionDeTurnos_Test()
    {
        // Simulamos el turno 1
        battle.Turn = 1;
        battle.PlayTurn(jugador, new Trainer("Jugador2", new Pokemon("Charmander", 100, 10, "3", Type.PokemonType.Fire)));
        // Ahora verificamos
        if (battle.ActualTurn == 1)
        {
            bool puedeUsarEspecialTurno1 = false;
            Assert.That(puedeUsarEspecialTurno1, Is.False, "No debe poder usar ataque especial en el primer turno");
        }
        else
        {
            bool puedeUsarEspecialTurno1 = true;
        }

        // Simulamos el turno 2
        battle.Turn = 2;
        ataqueBasico = new Attack("Ataque Básico", 10, Type.PokemonType.Water, false);
        ataqueEspecial = new Attack("Ataque Especial", 20, Type.PokemonType.Electric, true);
        battle.PlayTurn(jugador, new Trainer("Jugador2", new Pokemon("Charmander", 100, 10, "3", Type.PokemonType.Fire, new List<Attack> { ataqueBasico, ataqueEspecial })));
        
        // Ahora verificamos si puede usar el ataque especial en el segundo turno
        if (battle.ActualTurn == 1)
        {
            bool puedeUsarEspecialTurno2 = false;
        }
        else
        {
            bool puedeUsarEspecialTurno2 = true;
            Assert.That(puedeUsarEspecialTurno2, Is.True, "Debe poder usar ataque especial en el segundo turno");
        }
    }
    */
}

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
    
    
    [TestFixture]
    public class Pick6PokemonTest
    {
        private OriginalTrainer jugador;
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

            jugador = new OriginalTrainer("Jugador 1", catalogoPokemon[0]);
        }

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
    
    public class PlayersWaitListTest
    {
        /* Como entrenador, quiero ver la lista de jugadores esperando por un oponente.
            Criterios de aceptación:
            En la pantalla se ve la lista de jugadores que se unieron a la lista de espera. */
    }
    
    [TestFixture]
    public class PokemonsHealthTest
    {
        private Pokemon pokemon1;
        private Pokemon pokemon2;
        private Attack attack;

        [SetUp]
        public void Setup()
        {
            pokemon1 = new Pokemon("Pikachu", 100, 50, null, Type.PokemonType.Electric);
            pokemon2 = new Pokemon("Bulbasaur", 80, 40, null, Type.PokemonType.Plant);
            attack = new Attack("Thunderbolt", 25, Type.PokemonType.Electric, false);
        }

        [Test]
        public void Test_PokemonHpUpdatesAfterAttack()
        {
            // Asegurarse de que el HP es correcto
            Assert.That(pokemon2.Hp, Is.EqualTo(80));

            // Pikachu ataca a Bulbasaur
            pokemon1.Attack(pokemon2, pokemon1, attack);
            Assert.That(pokemon2.Hp, Is.EqualTo(55), "Los puntos de vida de Bulbasaur no se actualizaron correctamente después del ataque.");
        }

        [Test]
        public void Test_PokemonIsAliveAfterTakingDamage()
        {
            // Realizar un ataque y reducir los HP de Pikachu
            pokemon1.RecibeDamage(30);
            Assert.That(pokemon1.IsAlive(), Is.EqualTo("El Pokemon está vivo"));
        }

        [Test]
        public void Test_PokemonDiesWhenHpIsZero()
        {
            // Reducir los puntos de vida de Pikachu hasta que esté muerto
            pokemon1.RecibeDamage(100);
            Assert.That(pokemon1.IsAlive(), Is.EqualTo("El Pokemon está muerto"));
        }

        [Test]
        public void Test_PokemonHpNotBelowZero()
        {
            // Reducir los puntos de vida de Pikachu a un valor negativo
            pokemon1.RecibeDamage(200);
            Assert.That(pokemon1.Hp, Is.EqualTo(0), "Los puntos de vida de Pikachu no deberían ser negativos.");
        }
    }
    public class TurnInfoTest
    {
        private OriginalTrainer jugador;
        private OriginalTrainer oponente;
        private Battle batalla;

        [SetUp]
        public void SetUp()
        {
            var pokemon1 = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            var pokemonOponente = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            jugador = new OriginalTrainer("Jugador1", pokemon1);
            oponente = new OriginalTrainer("Jugador2", pokemonOponente);
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
        private OriginalTrainer jugador;
        private OriginalTrainer oponente;
        private Battle batalla;
        private SuperPotion pocion;

        [SetUp]
        public void SetUp()
        {
            var pokemon1 = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            var pokemonOponente = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            jugador = new OriginalTrainer("Jugador1", pokemon1);
            oponente = new OriginalTrainer("Jugador2", pokemonOponente);
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
    public class WaitTest
    {
        /* Como entrenador, quiero unirme a la lista de jugadores esperando por un oponente.
            Criterios de aceptación:
            El jugador recibe un mensaje confirmando que fue agregado a la lista de espera. */
    }

    public class WinBattleTest
    {
        private OriginalTrainer jugador;
        private OriginalTrainer oponente;
        private Pokemon pokemonJugador;
        private Pokemon pokemonOponente;
        private Battle batalla;

        [SetUp]
        public void SetUp()
        {
            pokemonJugador = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
            pokemonOponente = new Pokemon("Charmander", 0, 10, "2", Poke.Clases.Type.PokemonType.Fire); // Vida en cero para simular el fin de la batalla
            jugador = new OriginalTrainer("Jugador1", pokemonJugador);
            oponente = new OriginalTrainer("Jugador2", pokemonOponente);
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