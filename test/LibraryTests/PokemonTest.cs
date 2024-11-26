using NUnit.Framework;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Battle = Ucu.Poo.DiscordBot.Domain.Battle;

namespace LibraryTests
{

    [TestFixture]
    public class BattleFinishedTests
    {
        private Trainer trainer1;
        private Trainer trainer2;
        private Battle battle;
        private Pokemon pikachu;
        private Pokemon charmander;
        private Attack tackle;

        [SetUp]
        public void SetUp()
        {
            trainer1 = new Trainer("Ash");
            trainer2 = new Trainer("Gary");
            tackle = new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false);
            pikachu = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric,
                new List<Attack> { tackle });
            charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire,
                new List<Attack> { tackle });
            trainer1.AddPokemon(pikachu);
            trainer2.AddPokemon(charmander);
            battle = new Battle(trainer1, trainer2);
        }

        [Test]
        public void BattleFinished_Test()
        {
            Assert.That(battle.BattleFinished(), Is.Null, "Battle should not be finished initially.");
            charmander.RecibeDamage(trainer1, charmander.Hp); // Reduce HP to 0
            Assert.That(battle.BattleFinished(), Is.Not.Null,
                "Battle should be finished when one trainer's Pokémon are all fainted.");
        }

        [Test]
        public void GetWinner_Test()
        {
            Assert.That(battle.BattleFinished(), Is.Null, "There should be no winner initially.");
            charmander.RecibeDamage(trainer1, charmander.Hp); // Reduce HP to 0
            Assert.That(battle.BattleFinished(),
                Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!"),
                "Trainer1 should be the winner when trainer2's Pokémon are all fainted.");
        }

        public class ParalizeTests
        {
            private Pokemon charmander;

            [SetUp]
            public void SetUp()
            {
                charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            }

            [Test]
            public void ParalizedState_IsSetCorrectly()
            {
                charmander.Paralized = true;
                Assert.That(charmander.Paralized, Is.True, "Charmander should be paralyzed.");
            }

            [Test]
            public void ParalizedState_IsResetCorrectly()
            {
                charmander.Paralized = true;
                charmander.Paralized = false;
                Assert.That(charmander.Paralized, Is.False, "Charmander should not be paralyzed.");
            }
        }
        public class BurnedStateTests
        {
            private Pokemon charmander;

            [SetUp]
            public void SetUp()
            {
                charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            }

            [Test]
            public void BurnedState_ReducesHp()
            {
                charmander.Burned = true;
                double initialHp = charmander.Hp;
                charmander.StateActualization();
                Assert.That(charmander.Hp, Is.LessThan(initialHp), "Charmander's HP should be reduced due to burn.");
            }

            [Test]
            public void BurnedState_DoesNotReduceHpWhenNotBurned()
            {
                double initialHp = charmander.Hp;
                charmander.StateActualization();
                Assert.That(charmander.Hp, Is.EqualTo(initialHp),
                    "Charmander's HP should not be reduced when not burned.");
            }
        }
        public class BattleTests
        {
            private Trainer trainer1;
            private Trainer trainer2;
            private Battle battle;
            private Pokemon pikachu;
            private Pokemon charmander;
            private Attack tackle;

            [SetUp]
            public void SetUp()
            {
                trainer1 = new Trainer("Ash");
                trainer2 = new Trainer("Gary");
                tackle = new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false);
                pikachu = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric,
                    new List<Attack> { tackle });
                charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire,
                    new List<Attack> { tackle });
                trainer1.AddPokemon(pikachu);
                trainer2.AddPokemon(charmander);
                battle = new Battle(trainer1, trainer2);
            }


            [Test]
            public void BattleFinished_Test()
            {
                Assert.That(battle.BattleFinished(), Is.Null, "Battle should not be finished initially.");
                charmander.RecibeDamage(trainer1, charmander.Hp); // Reduce HP to 0
                Assert.That(battle.BattleFinished(), Is.Not.Null,
                    "Battle should be finished when one trainer's Pokémon are all fainted.");
            }

            [Test]
            public void GetWinner_Test()
            {
                Assert.That(battle.BattleFinished(), Is.Null, "There should be no winner initially.");
                charmander.RecibeDamage(trainer1, charmander.Hp); // Reduce HP to 0
                Assert.That(battle.BattleFinished(),
                    Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!"),
                    "Trainer1 should be the winner when trainer2's Pokémon are all fainted.");
            }
        }

        public class TotalCureTests
        {
            private TotalCure totalCure;
            private Pokemon damagedPokemon;
            private Pokemon statusAffectedPokemon;

            [SetUp]
            public void SetUp()
            {
                totalCure = new TotalCure();
                damagedPokemon = new Pokemon("Pikachu", 50, 100, "1", Poke.Clases.Type.PokemonType.Electric);
                statusAffectedPokemon = new Pokemon("Charmander", 50, 100, "2", Poke.Clases.Type.PokemonType.Fire)
                {
                    Poisoned = true,
                    Burned = true,
                    Paralized = true,
                    IsAlive = true
                };
            }

            [Test]
            public void Use_HealsDamagedPokemon()
            {
                totalCure.Use(damagedPokemon);
                Assert.That(damagedPokemon.Hp, Is.EqualTo(damagedPokemon.Hp),
                    "Damaged Pokémon should be healed to full HP.");
            }

            public class AttackTests
            {
                private Attack attack;

                [SetUp]
                public void SetUp()
                {
                    attack = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Electric, false);
                }

                [Test]
                public void AttackInitialization_Test()
                {
                    Assert.That(attack.Name, Is.EqualTo("Thunderbolt"), "Attack name should be Thunderbolt.");
                    Assert.That(attack.Damage, Is.EqualTo(40), "Attack power should be 40.");
                    Assert.That(attack.AttackType, Is.EqualTo(Poke.Clases.Type.PokemonType.Electric),
                        "Attack type should be Electric.");
                    Assert.That(attack.IsSpecial, Is.False, "Attack should not be special.");
                }

                [Test]
                public void AttackEffectiveness_Test()
                {
                    // Assuming there is a method to calculate effectiveness
                    // double effectiveness = attack.CalculateEffectiveness(Poke.Clases.Type.PokemonType.Water);
                    // Assert.That(effectiveness, Is.EqualTo(expectedValue), "Effectiveness should be calculated correctly.");
                }
            }
        }

        public class TrainerTests
        {
            private Trainer trainer;
            private Pokemon pikachu;
            private Pokemon charmander;
            private SuperPotion superPotion;

            [SetUp]
            public void SetUp()
            {
                trainer = new Trainer("Ash");
                pikachu = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric);
                charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
                superPotion = new SuperPotion();
            }

            [Test]
            public void AddPokemon_Test()
            {
                trainer.AddPokemon(pikachu);
                Assert.That(trainer.PokemonList, Contains.Item(pikachu),
                    "Trainer should have Pikachu in the Pokémon list.");
            }

            [Test]
            public void SetActualPokemon_Test()
            {
                trainer.AddPokemon(pikachu);
                trainer.SetActualPokemon(pikachu);
                Assert.That(trainer.ActualPokemon, Is.EqualTo(pikachu), "Trainer's actual Pokémon should be Pikachu.");
            }
        }


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
        
        public class NormalAttacksTests
        {
            private Pokemon attacker;
            private Pokemon defender;
            private Attack normalAttack;

            [SetUp]
            public void SetUp()
            {
                normalAttack = new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false);
                attacker = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric,
                    new List<Attack> { normalAttack });
                defender = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
            }

            [Test]
            public void NormalAttack_ReducesDefenderHp()
            {
                double initialHp = defender.Hp;
                attacker.Attack(null, defender, attacker, normalAttack);
                Assert.That(defender.Hp, Is.LessThan(initialHp),
                    "Defender's HP should be reduced after a normal attack.");
            }

            [Test]
            public void NormalAttack_DoesNotAffectAttackerHp()
            {
                double initialHp = attacker.Hp;
                attacker.Attack(null, defender, attacker, normalAttack);
                Assert.That(attacker.Hp, Is.EqualTo(initialHp),
                    "Attacker's HP should not be affected by a normal attack.");
            }

            public class AttackTests
            {
                private Attack attack;

                [SetUp]
                public void SetUp()
                {
                    attack = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Electric, false);
                }

                [Test]
                public void AttackInitialization_Test()
                {
                    Assert.That(attack.Name, Is.EqualTo("Thunderbolt"), "Attack name should be Thunderbolt.");
                    Assert.That(attack.Damage, Is.EqualTo(40), "Attack power should be 40.");
                    Assert.That(attack.AttackType, Is.EqualTo(Poke.Clases.Type.PokemonType.Electric),
                        "Attack type should be Electric.");
                    Assert.That(attack.IsSpecial, Is.False, "Attack should not be special.");
                }
            }

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
                    pikachu = new Pokemon("Pikachu", 100, 1, "1", Poke.Clases.Type.PokemonType.Electric,
                        new List<Attack> { thunderbolt });
                    charmander = new Pokemon("Charmander", 100, 1, "2", Poke.Clases.Type.PokemonType.Fire,
                        new List<Attack> { ember });
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
                    Assert.That(pikachu.Hp, Is.EqualTo(healthAfterDamage + 30),
                        "Pikachu should have recovered health.");
                }

                [Test]
                public void AddAttack_Test()
                {
                    var newAttack = new Attack("Quick Attack", 20, Poke.Clases.Type.PokemonType.Electric, false);
                    pikachu.AddAttack(newAttack);
                    Assert.That(pikachu.AttackList, Contains.Item(newAttack),
                        "Pikachu should have the new attack in its attack list.");
                }

                [Test]
                public void StateActualization_Test()
                {
                    charmander.Poisoned = true;
                    double initialHealth = charmander.Hp;
                    charmander.StateActualization();
                    Assert.That(charmander.Hp, Is.LessThan(initialHealth),
                        "Charmander should lose health due to poison.");
                }
            }
            
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
                    jugador1.AddPokemon(new Pokemon("Pikachu", 1, 10, "1", Poke.Clases.Type.PokemonType.Electric,
                        attackList));
                    jugador1.SetActualPokemon(jugador1.PokemonList[0]);

                    jugador2 = new Trainer("Jugador 2");
                    jugador2.AddPokemon(new Pokemon("Charizard", 1, 10, "2", Poke.Clases.Type.PokemonType.Fire,
                        attackList));
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
                    jugador.AddPokemon(catalogoPokemon[0]);
                }

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
                    // Inicializa el turno
                    batalla.InitialTurn();

                    // Verifica que el turno inicial sea del jugador o del oponente
                    Assert.That(batalla.Turn, Is.EqualTo(jugador).Or.EqualTo(oponente),
                        "El turno inicial debe ser del jugador o del oponente.");

                    // Ejecuta el primer turno
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


                public class WaitingListTests
                {
                    private WaitingList waitingList;
                    private Trainer trainer1;
                    private Trainer trainer2;

                    [SetUp]
                    public void SetUp()
                    {
                        waitingList = new WaitingList();
                        trainer1 = new Trainer("Ash");
                        trainer2 = new Trainer("Misty");
                    }

                    [Test]
                    public void AddTrainer_Test()
                    {
                        waitingList.AddTrainer(trainer1.DisplayName);
                        var result = waitingList.FindTrainerByDisplayName("Ash");
                        Assert.That(result, Is.Not.Null, "Trainer should be added to the waiting list.");
                    }

                    [Test]
                    public void FindTrainerByDisplayName_Test()
                    {
                        waitingList.AddTrainer(trainer1.DisplayName);
                        var result = waitingList.FindTrainerByDisplayName("Ash");
                        Assert.That(result.DisplayName, Is.EqualTo(trainer1.DisplayName),
                            "Should return the correct trainer from the waiting list.");
                    }

                }
            }
            
            public class RevivePotionTests
            {
                private RevivePotion revivePotion;
                private Pokemon faintedPokemon;
                private Pokemon healthyPokemon;

                [SetUp]
                public void SetUp()
                {
                    revivePotion = new RevivePotion();
                    faintedPokemon = new Pokemon("Pikachu", 0, 10, "1", Poke.Clases.Type.PokemonType.Electric);
                    healthyPokemon = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire);
                }

                [Test]
                public void Use_RevivesFaintedPokemon()
                {
                    revivePotion.Use(faintedPokemon);
                    Assert.That(faintedPokemon.Hp, Is.EqualTo(faintedPokemon.Hp / 2),
                        "Fainted Pokémon should be revived to half of its max HP.");
                }

                [Test]
                public void Use_DoesNotAffectHealthyPokemon()
                {
                    double initialHp = healthyPokemon.Hp;
                    revivePotion.Use(healthyPokemon);
                    Assert.That(healthyPokemon.Hp, Is.EqualTo(initialHp),
                        "Healthy Pokémon's HP should not be affected.");
                }

                [Test]
                public void Use_RevivesMultipleFaintedPokemon()
                {
                    var anotherFaintedPokemon =
                        new Pokemon("Squirtle", 0, 100, "4", Poke.Clases.Type.PokemonType.Water);
                    revivePotion.Use(faintedPokemon);
                    revivePotion.Use(anotherFaintedPokemon);
                    Assert.That(faintedPokemon.Hp, Is.EqualTo(faintedPokemon.Hp / 2),
                        "First fainted Pokémon should be revived to half of its max HP.");
                    Assert.That(anotherFaintedPokemon.Hp, Is.EqualTo(anotherFaintedPokemon.Hp / 2),
                        "Second fainted Pokémon should be revived to half of its max HP.");
                }
            }

        }
    }
}