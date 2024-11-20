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
    /// Representa a quien le corresponde el turno actual, determinándose random al inicio de la batalla.
    /// </summary>
    public Trainer? Turn { get; set; }
    
    /// <summary>
    /// Contador del turno actual.
    /// </summary>
    public double ActualTurn { get; set; }
    
    /// <summary>
    /// Booleano que se vuelve true cuando los dos entrenadores tienen los pokemones suficientes para la batalla.
    /// </summary>
    public bool ReadyToStart { get; set; }
    
    public bool BattleStarted { get; set; }

    /// <summary>
    /// Inicializa una instancia de la clase <see cref="Battle"/> con los
    /// valores recibidos como argumento.
    /// </summary>
    /// <param name="player1">El primer jugador.</param>
    /// <param name="player2">El oponente.</param>
    public Battle(Trainer player1, Trainer player2)
    {
        Player2 = player2;
        Player1 = player1;
        State = "NotStarted";
        ActualTurn = 1;
        ReadyToStart = false;
    }
    
    public void InitialTurn()
    {
        Random random = new Random();
        var initialTurn = random.Next(1, 3); // 1 para el primer jugador, 2 para el segundo jugador
        if (initialTurn == 1)
        {
            Turn = Player1;
        }
        else
        {
            Turn = Player2;
        }
    }
    
    public string? BattleFinished()
    {
        if (Player1.GetTotalPokemonLife() == 0)
        {
            Console.WriteLine("El jugador 2 ha ganado");
            return $"{Player2.DisplayName} ha ganado";
        }
        if (Player2.GetTotalPokemonLife() == 0)
        {
            return $"{Player1.DisplayName} ha ganado";
        }
        return null;
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
