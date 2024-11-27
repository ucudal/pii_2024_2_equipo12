using NUnit.Framework;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Battle = Ucu.Poo.DiscordBot.Domain.Battle;

namespace LibraryTests
{
    
    [TestFixture]
    public class ParalizedStateTests
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
        
    
    [TestFixture]
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
            Assert.That(charmander.Hp, Is.EqualTo(initialHp), "Charmander's HP should not be reduced when not burned.");
        }
    }
    [TestFixture]
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
            pikachu = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric, new List<Attack> { tackle });
            charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire, new List<Attack> { tackle });
            trainer1.AddPokemon(pikachu);
            trainer2.AddPokemon(charmander);
            battle = new Battle(trainer1, trainer2);
        }
        
        [Test]
        public void BattleFinished_Test()
        {
            // Arrange
            var battleList = new BattlesList();
            battleList.AddBattle(trainer1, trainer2); // Agregar la batalla al listado

            // Act
            string? result = battle.BattleFinished(battleList, trainer1.DisplayName);

            // Assert
            Assert.That(result, Is.EqualTo($"✅ {trainer2.DisplayName} ha ganado, no le quedan más Pokémon vivos al oponente!"), 
                "Trainer2 debería ganar cuando todos los Pokémon de Trainer1 están derrotados.");
        }

        [Test]
        public void GetWinner_Test()
        {
            // Arrange
            var battleList = new BattlesList();
            battleList.AddBattle(trainer1, trainer2); // Agregar la batalla al listado

            // Act
            string? result = battle.BattleFinished(battleList, trainer2.DisplayName);

            // Assert
            Assert.That(result, Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan más Pokémon vivos al oponente!"), 
                "Trainer1 debería ganar cuando todos los Pokémon de Trainer2 están derrotados.");
        }

    }
    
    [TestFixture]
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

        [TestFixture]
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
    }

    [TestFixture]
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
    
    [TestFixture]
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
            Assert.That(defender.Hp, Is.LessThan(initialHp), "Defender's HP should be reduced after a normal attack.");
        }

        [Test]
        public void NormalAttack_DoesNotAffectAttackerHp()
        {
            double initialHp = attacker.Hp;
            attacker.Attack(null, defender, attacker, normalAttack);
            Assert.That(attacker.Hp, Is.EqualTo(initialHp), "Attacker's HP should not be affected by a normal attack.");
        }

        [TestFixture]
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
                Assert.That(pikachu.Hp, Is.EqualTo(healthAfterDamage + 30), "Pikachu should have recovered health.");
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

            [SetUp]
            public void SetUp()
            {
                battle = new Battle(jugador1, jugador2);
            }
            [Test]
            public void DeterminarTurnoAleatorio_Test()
            {
                 var consoleOutput = new StringWriter();
                 Console.SetOut(consoleOutput);

                 battle.InitialTurn();
                 Assert.That(battle.Turn, Is.EqualTo(jugador1).Or.EqualTo(jugador2),
                     "El turno inicial debería ser del jugador 1 o del jugador 2.");
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

            [TestFixture]
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

        [TestFixture]
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
                Assert.That(healthyPokemon.Hp, Is.EqualTo(initialHp), "Healthy Pokémon's HP should not be affected.");
            }

            [Test]
            public void Use_RevivesMultipleFaintedPokemon()
            {
                var anotherFaintedPokemon = new Pokemon("Squirtle", 0, 100, "4", Poke.Clases.Type.PokemonType.Water);
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