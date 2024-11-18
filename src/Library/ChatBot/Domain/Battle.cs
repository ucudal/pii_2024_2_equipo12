namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa una batalla entre dos jugadores.
/// </summary>
public class Battle
{
    /// <summary>
    /// Obtiene un valor que representa el primer jugador.
    /// </summary>
    public Trainer Player1 { get; }
    
    /// <summary>
    /// Obtiene un valor que representa al oponente.
    /// </summary>
    public Trainer Player2 { get; }

    /// <summary>
    /// Obtiene o establece el estado de la batalla.
    /// </summary>
    public string State { get; private set; }

    /// <summary>
    /// Inicializa una instancia de la clase <see cref="Battle"/> con los
    /// valores recibidos como argumento.
    /// </summary>
    /// <param name="player1">El primer jugador.</param>
    /// <param name="player2">El oponente.</param>
    ///

    public Battle(Trainer player1, Trainer player2)
    {
        Player2 = player2;
        Player1 = player1;
        State = "NotStarted";

        Player1.Stage = 1;
        Player2.Stage = 1;
    }

    /// <summary>
    /// Inicia la batalla.
    /// </summary>
    ///
    public void StartBattle()
    {
        this.State = "Started";
    }
    
    /// <summary>
    /// Determina el ganador de la batalla.
    /// </summary>
    /// <returns>El nombre del jugador ganador.</returns>
    /*public string DetermineWinner()
    {
        // Lógica para determinar el ganador
        // Por simplicidad, se elige un ganador aleatorio
        Random random = new Random();
        return random.Next(2) == 0 ? Player1 : Player2;
    }*/

    public void EndBattle()
    {
        this.State = "Finished";
    }
}
