using NUnit.Framework;
using Poke.Clases;
using Type = System.Type;

namespace LibraryTests;

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