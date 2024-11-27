using Poke.Clases;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa una batalla entre dos entrenadores Pokémon.
/// </summary>
public class Battle
{
    /// <summary>
    /// Obtiene el primer jugador que participa en la batalla.
    /// </summary>
    public Trainer Player1 { get; }
    
    /// <summary>
    /// Obtiene el oponente que participa en la batalla.
    /// </summary>
    public Trainer Player2 { get; }

    /// <summary>
    /// Obtiene o establece el estado actual de la batalla.
    /// </summary>
    public string State { get; private set; }
    
    /// <summary>
    /// Obtiene o establece el entrenador al que le corresponde el turno actual.
    /// El turno inicial se determina aleatoriamente al comienzo de la batalla.
    /// </summary>
    public Trainer? Turn { get; set; }
    
    /// <summary>
    /// Obtiene o establece el número del turno actual.
    /// </summary>
    public double ActualTurn { get; set; }
    
    /// <summary>
    /// Indica si la batalla ha comenzado.
    /// Este valor se establece como <c>true</c> cuando ambos entrenadores tienen suficientes Pokémon.
    /// </summary>
    public bool BattleStarted { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Battle"/>.
    /// </summary>
    /// <param name="player1">El primer jugador de la batalla.</param>
    /// <param name="player2">El oponente de la batalla.</param>
    public Battle(Trainer player1, Trainer player2)
    {
        Player2 = player2;
        Player1 = player1;
        ActualTurn = 1;
        BattleStarted = false;
    }
    
    /// <summary>
    /// Determina aleatoriamente a quién le corresponde el turno inicial.
    /// </summary>
    /// <returns>El entrenador al que le corresponde el primer turno.</returns>
    public Trainer InitialTurn()
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
        return Turn;
    }
    
    /// <summary>
    /// Verifica si la batalla ha terminado al comprobar si alguno de los jugadores ya no tiene Pokémon vivos.
    /// </summary>
    /// <param name="battleList">La lista de batallas activas.</param>
    /// <param name="displayName">El nombre del jugador actual.</param>
    /// <returns>
    /// Una cadena que indica el ganador si la batalla ha terminado; 
    /// de lo contrario, <c>null</c>.
    /// </returns>
    public string? BattleFinished(BattlesList? battleList, string? displayName)
    {
        if (Player1.GetTotalPokemonLife() == 0)
        {
            Battle playersBattle = battleList.GetBattleByPlayer(displayName);
            
            if (battleList.RemoveBattle(playersBattle))
            {
                return $"✅ {Player2.DisplayName} ha ganado, no le quedan más Pokémon vivos al oponente!";
            }
        }
        if (Player2.GetTotalPokemonLife() == 0)
        {
            return $"✅ {Player1.DisplayName} ha ganado, no le quedan más Pokémon vivos al oponente!";
        }
        return null;
    }

    /// <summary>
    /// Verifica si ambos entrenadores están listos para la batalla.
    /// Se requiere que cada entrenador tenga exactamente 6 Pokémon.
    /// </summary>
    /// <returns><c>true</c> si ambos jugadores tienen 6 Pokémon; de lo contrario, <c>false</c>.</returns>
    public bool ReadyForBattle()
    {
        return Player1.PokemonList.Count == 6 && Player2.PokemonList.Count == 6;
    }

    /// <summary>
    /// Cambia el turno al siguiente jugador y actualiza el contador de turnos.
    /// También reduce el enfriamiento (cooldown) del entrenador actual si corresponde.
    /// </summary>
    /// <param name="player">El jugador cuyo turno acaba de terminar.</param>
    /// <param name="battleList">La lista de batallas activas.</param>
    /// <param name="displayName">El nombre del jugador actual.</param>
    /// <returns>
    /// Una cadena que indica el ganador si la batalla ha terminado;
    /// de lo contrario, <c>null</c>.
    /// </returns>
    public string? ChangeTurn(Trainer player, BattlesList? battleList, string? displayName, Pokemon pokemon)
    { 
        Turn = Turn == Player1 ? Player2 : Player1; // Cambia el turno
        ActualTurn += 1;
        pokemon.StateActualization();
        if (player.CoolDown != 0)
        {
            player.CoolDown -= 1;
        }
        return BattleFinished(battleList, displayName);
    }
}
