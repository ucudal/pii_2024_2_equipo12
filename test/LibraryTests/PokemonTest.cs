using NUnit.Framework;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Battle = Ucu.Poo.DiscordBot.Domain.Battle;

namespace LibraryTests
{
    //Debido a los cambios en el contructor de Battle y otros, ahora muchos test tienen error.
    public class PoisonTests
    {
        private Poisoned poisonAttack;
        private Pokemon targetPokemon;

        [SetUp]
        public void SetUp()
        {
            poisonAttack = new Poisoned("Toxic", 0, Poke.Clases.Type.PokemonType.Poison, true, "Poisoned");
            targetPokemon = new Pokemon("Bulbasaur", 100, 100, "1", Poke.Clases.Type.PokemonType.Plant);
        }

        [Test]
        public void ApplyEffect_DamageIsApplied()
        {
            double initialHp = targetPokemon.Hp;
            poisonAttack.Envenenar(targetPokemon);
            targetPokemon.StateActualization();
            Assert.That(targetPokemon.Hp, Is.LessThan(initialHp), "The Pokémon should lose HP due to poison.");
        }
    }

    public class BattleFinishedTests
    {
        private Trainer trainer1;
        private Trainer trainer2;
        private Battle battle;
        private Pokemon pikachu;
        private Pokemon charmander;
        private Attack tackle;
        private BattlesList battlesList;

        [SetUp]
        public void SetUp()
        {
            trainer1 = new Trainer("Ash");
            trainer2 = new Trainer("Gary");
            tackle = new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false, null);
            pikachu = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric, new List<Attack> { tackle });
            charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire, new List<Attack> { tackle });
            trainer1.AddPokemon(pikachu);
            trainer2.AddPokemon(charmander);
            battle = new Battle(trainer1, trainer2);
            battlesList = new BattlesList();
            battlesList.AddBattle(trainer1, trainer2);
        }

        /*[Test]
        public void BattleFinished_Test()
        {
            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName), Is.Null,
                "Battle should not be finished initially.");
            charmander.RecibeDamage(trainer1, charmander.Hp);
            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName),
                Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!"),
                "Battle should be finished when one trainer's Pokémon are all fainted.");
        }*/

       /* [Test]
        public void GetWinner_Test()
        {
            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName), Is.Null,
                "There should be no winner initially.");
            charmander.RecibeDamage(trainer1, charmander.Hp);
            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName),
                Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!"),
                "Trainer1 should be the winner when trainer2's Pokémon are all fainted.");
        }*/
    }

    public class AsleepTests
    {
        private Asleep sleepAttack;
        private Pokemon targetPokemon;

        [SetUp]
        public void SetUp()
        {
            sleepAttack = new Asleep("Sleep Powder", 0, Poke.Clases.Type.PokemonType.Plant, true, null);
            targetPokemon = new Pokemon("Bulbasaur", 100, 100, "1", Poke.Clases.Type.PokemonType.Plant);
        }

        [Test]
        public void Sleep_SetsStateToAsleep()
        {
            sleepAttack.Sleep(targetPokemon);
            Assert.That(targetPokemon.State, Is.EqualTo("Dormido"), "The Pokémon should be in an Asleep state.");
        }
    }

    public class BurnedTests
    {
        private Burned burnedAttack;
        private Pokemon targetPokemon;

        [SetUp]
        public void SetUp()
        {
            burnedAttack = new Burned("Flamethrower", 0, Poke.Clases.Type.PokemonType.Fire, true, "Burned");
            targetPokemon = new Pokemon("Bulbasaur", 100, 100, "1", Poke.Clases.Type.PokemonType.Plant);
        }

        [Test]
        public void Burn_DamageIsApplied()
        {
            double initialHp = targetPokemon.Hp;
            burnedAttack.Burn(targetPokemon);
            targetPokemon.StateActualization();
            Assert.That(targetPokemon.Hp, Is.LessThan(initialHp), "The Pokémon should lose HP due to burn.");
        }
    }

    public class ParalizedTests
    {
        private Paralized paralizedAttack;
        private Pokemon targetPokemon;

        [SetUp]
        public void SetUp()
        {
            paralizedAttack = new Paralized("Thunder Wave", 0, Poke.Clases.Type.PokemonType.Electric, true, "Paralized");
            targetPokemon = new Pokemon("Bulbasaur", 100, 100, "1", Poke.Clases.Type.PokemonType.Plant);
        }

        [Test]
        public void Paralize_SetsStateToParalized()
        {
            paralizedAttack.Paralize(targetPokemon);
            Assert.That(targetPokemon.State, Is.EqualTo("Paralizado"),
                "The Pokémon should be in a Paralized state.");
        }

        [Test]
        public void Paralize_AttackCapacityIsSet()
        {
            paralizedAttack.Paralize(targetPokemon);
            Assert.That(targetPokemon.State, Is.EqualTo("Paralizado"),
                "The Pokémon should be in a Paralized state.");
        }
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

        public class ParalizedTests
        {
            private Paralized paralizedAttack;
            private Pokemon targetPokemon;

            [SetUp]
            public void SetUp()
            {
                paralizedAttack = new Paralized("Thunder Wave", 0, Poke.Clases.Type.PokemonType.Electric, true, "Paralized");
                targetPokemon = new Pokemon("Bulbasaur", 100, 100, "1", Poke.Clases.Type.PokemonType.Plant);
            }

            [Test]
            public void Paralize_SetsStateToParalized()
            {
                paralizedAttack.Paralize(targetPokemon);
                Assert.That(targetPokemon.State, Is.EqualTo("Paralizado"),
                    "The Pokémon should be in a Paralized state.");
            }

            [Test]
            public void Paralize_AttackCapacityIsSet()
            {
                paralizedAttack.Paralize(targetPokemon);
                Assert.That(targetPokemon.State, Is.EqualTo("Paralizado"),
                    "The Pokémon should be in a Paralized state.");
            }
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

    public class AttackTests
    {
        private Attack attack;

        [SetUp]
        public void SetUp()
        {
            attack = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Electric, false, null);
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
        public void AttackIsSpecial_ShouldBeBoolean()
        {
            var specialAttack = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Electric, true, "Paralized");
            Assert.That(specialAttack.IsSpecial, Is.True, "Attack should be special.");
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
            tackle = new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false, null);
            pikachu = new Pokemon("Pikachu", 100, 10, "1", Poke.Clases.Type.PokemonType.Electric,
                new List<Attack> { tackle });
            charmander = new Pokemon("Charmander", 100, 10, "2", Poke.Clases.Type.PokemonType.Fire,
                new List<Attack> { tackle });
            trainer1.AddPokemon(pikachu);
            trainer2.AddPokemon(charmander);
            battle = new Battle(trainer1, trainer2);
        }


        /*[Test]
        public void BattleFinished_Test()
        {
            var battlesList = new BattlesList();
            battlesList.AddBattle(trainer1, trainer2);

            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName), Is.Null, "Battle should not be finished initially.");
            charmander.RecibeDamage(trainer1, charmander.Hp);
            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName), Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!"), "Battle should be finished when one trainer's Pokémon are all fainted.");
        }*/


        /*[Test]
        public void GetWinner_Test()
        {

            var battlesList = new BattlesList();
            battlesList.AddBattle(trainer1, trainer2);

            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName), Is.Null, "Battle should not be finished initially.");
            charmander.RecibeDamage(trainer1, charmander.Hp);
            Assert.That(battle.BattleFinished(battlesList, trainer1.DisplayName), Is.EqualTo($"✅ {trainer1.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!"), "Trainer1 should be the winner when trainer2's Pokémon are all fainted.");
        }*/

    

    [Test]
        public void InitialTurn_Test()
        {
            battle.InitialTurn();
            Assert.That(battle.Turn, Is.EqualTo(trainer1).Or.EqualTo(trainer2),
                "The initial turn should be either trainer1 or trainer2.");
        }

        [Test]
        public void ReadyForBattle_Test()
        {
            for (int i = 0; i < 6; i++)
            {
                trainer1.AddPokemon(new Pokemon($"Pokemon{i}", 100, 10, $"{i}", Poke.Clases.Type.PokemonType.Normal));
                trainer2.AddPokemon(new Pokemon($"Pokemon{i + 6}", 100, 10, $"{i + 6}",
                    Poke.Clases.Type.PokemonType.Normal));
            }

            Assert.That(battle.ReadyForBattle(), Is.True, "Both trainers should have 6 Pokémon each.");
        }

        [Test]
        public void ChangeTurn_Test()
        {
            trainer1.AddPokemon(pikachu);
            var battlesList = new BattlesList();
            battlesList.AddBattle(trainer1, trainer2);

            battle.InitialTurn();
            var initialTurn = battle.Turn;
            battle.ChangeTurn(initialTurn, battlesList, initialTurn.DisplayName, pikachu);
            Assert.That(battle.Turn, Is.Not.EqualTo(initialTurn), "The turn should change to the other trainer.");
        }

        public class TotalCureTests
        {
            private TotalCure totalCure;
            private Pokemon damagedPokemon;
            private Pokemon statusAffectedPokemon;
            private Attack attack;
            [SetUp]
            public void SetUp()
            {
                attack = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Electric, false, null);
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
            normalAttack = new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false, "Normal");
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
                attack = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Electric, false, "Normal");
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
                thunderbolt = new Attack("Thunderbolt", 40, Poke.Clases.Type.PokemonType.Bug, false, null);
                ember = new Attack("Ember", 30, Poke.Clases.Type.PokemonType.Dragon, true, null);
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
                var newAttack = new Attack("Quick Attack", 20, Poke.Clases.Type.PokemonType.Electric, false, null);
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

                var consoleOutput = new StringWriter();
                Console.SetOut(consoleOutput);
                waitList.AddTrainer(jugador1.DisplayName);
                waitList.AddTrainer(jugador2.DisplayName);

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
                Attack ataque = new Attack("relámpago", 30, Poke.Clases.Type.PokemonType.Electric, false, null);
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

                for (int i = 0; i < 6; i++)
                {
                    jugador.AddPokemon(catalogoPokemon[i]);
                }

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
                batalla.InitialTurn();
                Assert.That(batalla.Turn, Is.EqualTo(jugador).Or.EqualTo(oponente),
                    "El turno inicial debe ser del jugador o del oponente.");

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

    public class RestrictionsTest
    {
        private List<string> RestrictedItems;
        private List<int> RestrictedPokemonIndexes;
        private List<PokemonCatalog.Catalog> RestrictedTypes;
        private string itemString;
        
        [SetUp]
        public void SetUp()
        {
            RestrictedItems = new List<string>();
            RestrictedPokemonIndexes = new List<int>();
            RestrictedTypes = new List<PokemonCatalog.Catalog>();
            itemString = "SuperPotion RevivePotion TotalCure";
        }

        [Test]
        public void AddItemRestrictionTest()
        {
            Facade.Instance.AddItemRestriction(itemString);
            Facade.Instance.CreateNewBattle("player1", "player2");
            Battle batallaCreada = Facade.Instance.
        }
    }
}
