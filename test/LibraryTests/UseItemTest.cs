using NUnit.Framework;
using Poke.Clases;
using Type = System.Type;

namespace LibraryTests;

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
