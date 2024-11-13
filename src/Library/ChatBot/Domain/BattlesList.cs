namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa la lista de batallas en curso.
/// </summary>
public class BattlesList
{
    private List<Battle> battles = new List<Battle>();

    /// <summary>
    /// Crea una nueva batalla entre dos jugadores.
    /// </summary>
    /// <param name="player1">El primer jugador.</param>
    /// <param name="player2">El oponente.</param>
    /// <returns>La batalla creada.</returns>
    public Battle AddBattle(string player1, string player2)
    {
        var battle = new Battle(player1, player2);
        this.battles.Add(battle);
        return battle;
    }
    
    /// <summary>
    /// Muestra todas las batallas en curso.
    /// </summary>
    /// <returns>Un mensaje con el resultado.</returns>
    public bool GetBattle(String playerDisplayName)
    {
        foreach (Battle battle in this.battles)
        {
            if (battle.Player1 == playerDisplayName || battle.Player2 == playerDisplayName)
            {
                return true;
            }
        }
        return false;
    }
}
