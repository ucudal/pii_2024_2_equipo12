using NUnit.Framework;
using Poke.Clases;
using Type = Poke.Clases.Type;

namespace LibraryTests
{
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
}