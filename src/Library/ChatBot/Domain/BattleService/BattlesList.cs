namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa una lista de batallas en curso, permitiendo administrar las batallas
/// entre diferentes entrenadores Pokémon.
/// </summary>
public class BattlesList
{
    private List<Battle> battles = new List<Battle>();

    /// <summary>
    /// Crea y añade una nueva batalla a la lista de batallas en curso.
    /// </summary>
    /// <param name="player1">El primer jugador que participa en la batalla.</param>
    /// <param name="player2">El oponente que participa en la batalla.</param>
    /// <returns>La instancia de la batalla creada.</returns>
    public Battle AddBattle(Trainer player1, Trainer player2, List<String>? restrictedItems, List<int>? restrictedPokemonIndexes, List<PokemonCatalog.Catalog> restrictedTypes)
    {
        var battle = new Battle(player1, player2, restrictedItems, restrictedPokemonIndexes, restrictedTypes);
        this.battles.Add(battle);
        return battle;
    }
    
    /// <summary>
    /// Busca al entrenador asociado con el nombre mostrado proporcionado en las batallas activas.
    /// </summary>
    /// <param name="playerDisplayName">El nombre mostrado del jugador que se busca.</param>
    /// <returns>
    /// El entrenador encontrado en las batallas activas que coincide con el nombre proporcionado;
    /// de lo contrario, <c>null</c>.
    /// </returns>
    public Trainer? GetPlayerInBattle(string playerDisplayName)
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

    /// <summary>
    /// Obtiene la batalla en la que participa un jugador según su nombre mostrado.
    /// </summary>
    /// <param name="playerDisplayName">El nombre mostrado del jugador.</param>
    /// <returns>
    /// La instancia de la batalla en la que participa el jugador;
    /// de lo contrario, <c>null</c>.
    /// </returns>
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

    /// <summary>
    /// Obtiene el oponente de un jugador en una batalla activa, basado en su nombre mostrado.
    /// </summary>
    /// <param name="playerDisplayName">El nombre mostrado del jugador.</param>
    /// <returns>
    /// El entrenador que es oponente del jugador en la batalla;
    /// de lo contrario, <c>null</c>.
    /// </returns>
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

    /// <summary>
    /// Elimina una batalla de la lista de batallas en curso.
    /// </summary>
    /// <param name="battle">La batalla que se desea eliminar.</param>
    /// <returns>
    /// <c>true</c> si la batalla fue eliminada con éxito;
    /// de lo contrario, <c>false</c>.
    /// </returns>
    public bool RemoveBattle(Battle battle)
    {
        return battles.Remove(battle);
    }
}
