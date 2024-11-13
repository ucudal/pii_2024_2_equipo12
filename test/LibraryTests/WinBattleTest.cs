using NUnit.Framework;
using Poke.Clases;
using Type = System.Type;

namespace LibraryTests;

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