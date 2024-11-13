using NUnit.Framework;
using Poke.Clases;
using Type = Poke.Clases.Type;
using System.IO;
using System.Collections.Generic;

namespace LibraryTests
{
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
}
