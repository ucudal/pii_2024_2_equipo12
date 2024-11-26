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
    public Battle AddBattle(Trainer player1, Trainer player2)
    {
        var battle = new Battle(player1, player2);
        this.battles.Add(battle);
        return battle;
    }
    
    /// <summary>
    /// Funcion para conseguir el entrenador asociado al playerDisplayName
    /// Lo busca en las batallas y de no encontrarlo devuelve null
    /// </summary>
    /// <returns>Entrenador asociado al playerDisplayName.</returns>
    public Trainer? GetPlayerInBattle(String playerDisplayName)
    {
        foreach (var battle in battles)
        {
            if (battle.Player1.DisplayName == playerDisplayName)
            {
                return battle.Player1;
                
            }
            if (battle.Player2.DisplayName == playerDisplayName)
            {
                return battle.Player2;
            }
        }
        return null;
    }

    public Battle? GetBattleByPlayer(string playerDisplayName)
    {
        foreach (var battle in battles)
        {
            if (battle.Player1.DisplayName == playerDisplayName || battle.Player2.DisplayName == playerDisplayName)
            {
                return battle;
            }
        }
        return null;
    }

    public Trainer? GetOpponnentInBattle(string playerDisplayName)
    {
        foreach (var battle in battles)
        {
            if (battle.Player1.DisplayName == playerDisplayName)
            {
                return battle.Player2;
                
            }
            if (battle.Player2.DisplayName == playerDisplayName)
            {
                return battle.Player1;
            }
        }
        return null;
    }

    public bool RemoveBattle(Battle battle)
    {
        return battles.Remove(battle);
    }
}
