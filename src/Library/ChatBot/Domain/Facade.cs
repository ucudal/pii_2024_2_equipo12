using System.Text;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase actúa como fachada para manejar las acciones relacionadas con las batallas Pokémon y la lista de espera. 
/// Es un singleton y se utiliza para desacoplar la lógica del dominio del resto de las clases.
/// </summary>
public class Facade
{
    private static Facade? _instance;
    private BattlesList _battlesList;

    // Este constructor privado impide que otras clases puedan crear instancias
    // de esta.
    private Facade()
    {
        this.WaitingList = new WaitingList();
        this.BattlesList = new BattlesList();
        _battlesList = new BattlesList();
    }

    /// <summary>
    /// Obtiene la única instancia de la clase <see cref="Facade"/>.
    /// </summary>
    public static Facade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Facade();
            }

            return _instance;
        }
    }

    /// <summary>
    /// Obtiene la lista de batallas activas.
    /// </summary>
    /// <returns>Una instancia de <see cref="BattlesList"/> con las batallas actuales.</returns>
    public BattlesList GetBattlesList()
    {
        return _battlesList;
    }

    /// <summary>
    /// Resetea la instancia del singleton. Solo debe usarse en los tests.
    /// </summary>
    public static void Reset()
    {
        _instance = null;
    }

    private WaitingList WaitingList { get; }

    private BattlesList BattlesList { get; }
    
    private int UseCounter { get; set; }

    /// <summary>
    /// Agrega un entrenador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre del entrenador.</param>
    /// <returns>Un mensaje indicando el resultado de la operación.</returns>
    public string AddTrainerToWaitingList(string displayName)
    {
        if (this.WaitingList.AddTrainer(displayName))
        {
            Trainer? player = WaitingList.FindTrainerByDisplayName(displayName);
            player.Stage = 1;
            return $"{displayName} agregado a la lista de espera";
        }

        return $"{displayName} ya está en la lista de espera";
    }

    /// <summary>
    /// Remueve un entrenador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre del entrenador a remover.</param>
    /// <returns>Un mensaje indicando el resultado de la operación.</returns>
    public string RemoveTrainerFromWaitingList(string displayName)
    {
        if (this.WaitingList.RemoveTrainer(displayName))
        {
            return $"{displayName} removido de la lista de espera";
        }
        else
        {
            return $"{displayName} no está en la lista de espera";
        }
    }

    /// <summary>
    /// Obtiene una lista de todos los entrenadores en espera.
    /// </summary>
    /// <returns>Un mensaje con la lista de entrenadores en espera o un mensaje indicando que la lista está vacía.</returns>
    public string GetAllTrainersWaiting()
    {
        if (this.WaitingList.Count == 0)
        {
            return "No hay nadie esperando";
        }

        string result = "Esperan: ";
        foreach (Trainer trainer in this.WaitingList.GetAllWaiting())
        {
            result = result + trainer.DisplayName + "; ";
        }

        return result;
    }

    /// <summary>
    /// Verifica si un entrenador está en la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre del entrenador.</param>
    /// <returns>Un mensaje indicando si el entrenador está esperando.</returns>
    public string TrainerIsWaiting(string displayName)
    {
        Trainer? trainer = this.WaitingList.FindTrainerByDisplayName(displayName);
        if (trainer == null)
        {
            return "No estás esperando!";
        }

        return "Estás esperando!";
    }

    /// <summary>
    /// Crea una batalla entre dos entrenadores.
    /// </summary>
    /// <param name="player">El primer entrenador.</param>
    /// <param name="opponent">El oponente.</param>
    /// <returns>Un mensaje indicando el inicio de la batalla.</returns>
    private string CreateBattle(Trainer player, Trainer opponent)
    {
        this.WaitingList.RemoveTrainer(player.DisplayName);
        this.WaitingList.RemoveTrainer(opponent.DisplayName);
        player.Stage = 2;
        opponent.Stage = 2;

        BattlesList.AddBattle(player, opponent);
        return $"Comienza {player.DisplayName} vs {opponent.DisplayName}";
    }

    /// <summary>
    /// Crea una nueva batalla entre dos entrenadores, seleccionando un oponente automáticamente si no se proporciona.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del entrenador que inicia la batalla.</param>
    /// <param name="opponentDisplayName">El nombre del oponente, o null si se selecciona automáticamente.</param>
    /// <returns>Un mensaje indicando el resultado de la operación.</returns>
    public string CreateNewBattle(string playerDisplayName, string? opponentDisplayName)
    {
        Trainer? opponent;
        Trainer player = this.WaitingList.FindTrainerByDisplayName(playerDisplayName);

        if (!OpponentProvided() && !SomebodyIsWaiting())
        {
            return "No hay nadie esperando";
        }

        if (!OpponentProvided())
        {
            opponent = this.WaitingList.GetAnyoneWaiting();
            return this.CreateBattle(player, opponent!);
        }

        opponent = this.WaitingList.FindTrainerByDisplayName(opponentDisplayName!);

        if (!OpponentFound())
        {
            return $"{opponentDisplayName} no está esperando";
        }

        return this.CreateBattle(player, opponent);

        // Funciones locales a continuación para mejorar la legibilidad

        bool OpponentProvided()
        {
            return !string.IsNullOrEmpty(opponentDisplayName);
        }

        bool SomebodyIsWaiting()
        {
            return this.WaitingList.Count != 0;
        }

        bool OpponentFound()
        {
            return opponent != null;
        }
    }

    /// <summary>
    /// Se imprime la lista de pociones disponibles para el jugador.
    /// </summary>
    /// <param name="playerDisplayName">El nombre para mostrar del jugador.</param>
    /// <returns>Un mensaje con los items disponibles.</returns>
    public (string message, string? OpponentDisplayName) GetAvailableItems(string displayName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(displayName);
        Battle? battle = BattlesList.GetBattleByPlayer(displayName);
        var result = InitialVerifications(player, opponent, battle, null);
        if (result != null)
        {
            return result.Value;
        }
        if (BattlesList.GetPlayerInBattle (displayName).Items != null)
        {
            foreach (Item item in BattlesList.GetPlayerInBattle(displayName).Items)
            {
                var sb = new StringBuilder();
                sb.AppendLine(item.Name);
                return ($"Lista de pociones disponibles: \n {result}", null);
            }
        }
        return ("No hay pociones disponibles", null);
    }

    /// <summary>
    /// Cambia el pokemon actual del jugador por otro de su lista de pokemones.
    /// </summary>
    /// <param name="displayName">El nombre para mostrar del jugador.</param>
    /// <param name="pokemonName">El nombre del pokemon a cambiar.</param>
    /// <returns>Un mensaje de confirmación del cambio del Pokemon.</returns>
    public string ChangePokemon(string displayName, string pokemonName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(displayName);
        Battle? battle = BattlesList.GetBattleByPlayer(displayName);
        var result = InitialVerifications(player, opponent, battle, null);
        if (result != null)
        {
            return result.Value.message;
        }

        // Verificación del Pokémon del jugador
        Pokemon? playerPokemon = player.PokemonList.Find(pokemon => pokemon.Name == pokemonName);
        if (!playerPokemon.IsAlive)
        {
            return ($"❌ {playerPokemon.Name} ha muerto. Usa el comando !change para cambiar a tu próximo Pokémon.");
        }

        player.ActualPokemon = playerPokemon;
        string? battleFinished = battle.ChangeTurn(player);
        if (battleFinished != null)
        {
            return battleFinished;
        }
        return $"✨🔁 Cambiaste tu Pokemon actual a {pokemonName} ✨🔁\n{GetPokemonAttacks(displayName, pokemonName)}";
    }

    /// <summary>
    /// Usuario gasta su turno eligiendo una poción luego de seleccionarla de 
    /// su lista de pociones impresas en la consola.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="potionName">El nombre de la poción seleccionada.</param>
    /// <returns>Un mensaje con el resultado de la acción.</returns>
    public (string message, string? OpponentDisplayName) UsePotion(string playerDisplayName, string potionName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(playerDisplayName);
        Battle? battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        var result = InitialVerifications(player, opponent, battle, null);
        if (result != null)
        {
            return result.Value;
        }

        Item? potion = player.Items.Find(item => item.Name == potionName);
        if (potion != null)
        {
            if (potion is RevivePotion)
            {
                if (player.ActualPokemon.IsAlive)
                {
                    return ("❌ No puedes usar Revive en un Pokémon que ya está vivo.", null);
                }

                player.UseItem(potion, player.ActualPokemon);
            }
            else if (potion is SuperPotion)
            {
                if (!player.ActualPokemon.IsAlive)
                {
                    return ("❌ No puedes usar SuperPotion en un Pokémon que está muerto.", null);
                }

                player.UseItem(potion, player.ActualPokemon);
            }
            else if (potion is TotalCure)
            {
                if (!player.ActualPokemon.IsAlive)
                {
                    return ("❌ No puedes usar la poción de Cura Total en un Pokémon que está muerto.", null);
                }

                if (player.ActualPokemon.State == null)
                {
                    return ("❌ No puedes usar la poción de Cura Total en un Pokémon que no tiene estados inducidos.",
                        null);
                }

                player.UseItem(potion, player.ActualPokemon);
            }

            // Cambiar turno o finalizar la batalla
            string? battleFinished = battle.ChangeTurn(player);
            if (battleFinished != null)
            {
                return (battleFinished, null);
            }

            return ($"✨🧙 Usaste {potionName} en tu pokemon {player.ActualPokemon.Name} ✨🧙", null);
        }

        return ("❌ No tienes esa poción", null);
    }

    /// <summary>
    /// Usuario gasta su turno atacando al oponente con un ataque de su pokemon.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="attackName">El nombre del ataque.</param>
    /// <param name="opponentDisplayName">El nombre del oponente.</param>
    /// <returns>Un mensaje de confirmación del ataque.</returns>
    public (string message, string? OpponentDisplayName) AttackPokemon(string playerDisplayName, string attackName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(playerDisplayName);
        Battle? battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        var result = InitialVerifications(player, opponent, battle, null);
        if (result != null)
        {
            return result.Value;
        }

        // Verificación del Pokémon del jugador
        Pokemon playerPokemon = player.ActualPokemon;
        if (!playerPokemon.IsAlive)
        {
            return ($"❌ {playerPokemon.Name} ha muerto. Usa el comando !change para cambiar a tu próximo Pokémon.", null);
        }

        // Verificación del ataque
        Attack? attack = playerPokemon.AttackList.Find(a => a.Name == attackName && !a.IsSpecial);
        if (attack == null)
        {
            return ("❌ No tienes ese ataque o es un ataque especial", null);
        }

        // Ejecutar ataque
        string? isOpponentPokemonDead = playerPokemon.Attack(player, opponent.ActualPokemon, playerPokemon, attack);

        // Cambiar turno o finalizar la batalla
        string? battleFinished = battle.ChangeTurn(player);
        if (battleFinished != null)
        {
            return (battleFinished, null);
        }

        if (isOpponentPokemonDead != null)
        {
            return (isOpponentPokemonDead, opponent.DisplayName);
        }

        return ($"✨🔥 {playerPokemon.Name} atacó a {opponent.ActualPokemon.Name} con su ataque {attack.Name} 🔥✨",
            null);
    }

    /// <summary>
    /// Usuario gasta su turno atacando al oponente con un ataque especial de su pokemon.
    /// </summary>
    /// <param name="playerDisplayName">El nombre para mostrar del jugador.</param>
    /// <param name="specialAttackName">El nombre del ataque especial.</param>
    /// <param name="opponentDisplayName">El nombre del oponente.</param>
    /// <returns>Un mensaje de confirmación del ataque especial.</returns>
    public (string message, string? OpponentDisplayName) SpecialAttackPokemon(string playerDisplayName,
        string specialAttackName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(playerDisplayName);
        Battle? battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        var result = InitialVerifications(player, opponent, battle, null);
        if (result != null)
        {
            return result.Value;
        }

        if (player.CoolDown == 0)
        {
            // Verificación del Pokémon del jugador
            Pokemon playerPokemon = player.ActualPokemon;

            // Verificación del ataque especial
            Attack? specialAttack =
                playerPokemon.AttackList.Find(selectedAttack => selectedAttack.Name == specialAttackName);

            if (specialAttack != null && specialAttack.IsSpecial == true) // Descartar los ataques normales
            {
                // Ejecuta el Ataque
                string? isOpponentPokemonDead =
                    playerPokemon.Attack(player, opponent.ActualPokemon, playerPokemon, specialAttack);

                // Aumenta el cooldown a 2 turnos
                player.CoolDown += 2;

                string? battleFinished = battle.ChangeTurn(player);
                if (battleFinished != null)
                {
                    return (battleFinished, null);
                }

                if (isOpponentPokemonDead != null)
                {
                    return (isOpponentPokemonDead, opponent.DisplayName);
                }

                return (
                    $"✨🔥 {playerPokemon.Name} atacó a {opponent.ActualPokemon.Name} con su ataque especial {specialAttack.Name} 🔥✨",
                    null);
            }

            return ("❌ No tienes ese ataque especial", null);
        }

        return ($"❌ No puedes lanzar un ataque especial, tienes un cooldown de {player.CoolDown} turnos", null);
    }

    /// <summary>
    /// Determina el tipo de ataque y ejecuta el correspondiente: normal o especial.
    /// </summary>
    /// <param name="displayName">El nombre del jugador que ataca.</param>
    /// <param name="attackName">El nombre del ataque.</param>
    /// <returns>Un mensaje con el resultado del ataque.</returns>
    public (string message, string? OpponentDisplayName) DetermineAttack(string displayName, string attackName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(displayName);
        Battle? battle = BattlesList.GetBattleByPlayer(displayName);
        var result = InitialVerifications(player, opponent, battle, null);
        if (result != null)
        {
            return result.Value;
        }

        // Verificación del Pokémon del jugador
        Pokemon playerPokemon = player.ActualPokemon;
        if (!playerPokemon.IsAlive)
        {
            return ($"❌ {playerPokemon.Name} está muerto. Usa el comando !change para cambiar tu pokemon.", null);
        }

        // Determina si el ataque es normal o especial
        if (playerPokemon.AttackList.Any(a => a.Name == attackName && !a.IsSpecial))
        {
            return AttackPokemon(displayName, attackName);
        }

        return SpecialAttackPokemon(displayName, attackName);
    }

    /// <summary>
    /// Verifica si un jugador está en una batalla.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un valor booleano que indica si el jugador está en una batalla.</returns>
    public bool IsPlayerInGame(string playerDisplayName)
    {
        if (BattlesList.GetPlayerInBattle(playerDisplayName) == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifica si un jugador tiene los Pokémon necesarios para combatir.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un mensaje indicando si el jugador tiene los 6 Pokémon o null si el jugador no está en una batalla.</returns>
    public string? PlayerWithPokemon(string playerDisplayName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);

        if (player == null)
        {
            return "Comienza una batalla para elegir tus Pokémon!";
        }

        if (player.PokemonList.Count == 6)
        {
            return "Ya tienes seleccionados tus Pokémon, comienza a pelear!";
        }

        return null;
    }

    /// <summary>
    /// Gestiona la selección de Pokémon por parte de un jugador, validando
    /// los índices y asegurándose de que los Pokémon seleccionados sean válidos.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador que realiza la selección.</param>
    /// <param name="indices">
    /// Una cadena de texto que contiene los índices separados por espacios
    /// correspondientes a los Pokémon seleccionados por el jugador.
    /// </param>
    /// <returns>
    /// Una tupla que contiene un mensaje de confirmación o error y
    /// un mensaje opcional que indica si la batalla está lista para comenzar.
    /// </returns>
    public (string message, string? ReadyForBattleMessage, Trainer? InitialTurn) PokemonSelection(string displayName,
        string indices)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(displayName);
        Battle? battle = BattlesList.GetBattleByPlayer(displayName);

        if (opponent == null || battle == null)
        {
            return ("❌ Debes tener un oponente y una batalla empezada para poder realizar esta acción", null, null);
        }

        if (player.Stage != 2)
        {
            return ("❌ No puedes seleccionar Pokémon en este momento.", null, null);
        }

        // Divide los índices proporcionados y verifica si tienen exactamente 6.
        var selectedIndices = indices.Split(' ')
            .Select(i => int.TryParse(i, out int index) ? index - 1 : -1)
            .ToList();

        if (selectedIndices.Any(index => index < 0))
        {
            return (
                "❌ Uno o más índices proporcionados no son válidos. Por favor, usa números enteros positivos que estén dentro del catálogo.",
                null, null);
        }

        var catalog = Enum.GetValues(typeof(PokemonCatalog.Catalog)).Cast<PokemonCatalog.Catalog>().ToList();
        var result = new StringBuilder();

        foreach (var index in selectedIndices)
        {
            if (index < 0 || index >= catalog.Count)
            {
                result.AppendLine($"❌ Índice {index} no es válido, haz la selección de nuevo.");
                continue;
            }

            var catalogEntry = catalog[index];
            var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
            result.AppendLine(player.AddPokemon(pokemon));
        }

        if (battle.ReadyForBattle())
        {
            battle.BattleStarted = true;
            player.Stage = 3;
            return (result.ToString(), $"{displayName} y {opponent.DisplayName} tienen 6 Pokémon, comienza la batalla!",
                battle.InitialTurn());
        }

        return (result.ToString(), null, null);
    }

    /// <summary>
    /// Verifica si la batalla está lista para iniciar.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <returns>Un valor booleano que indica si la batalla está lista.</returns>
    public bool CheckToStartBattle(string displayName)
    {
        Battle battle = BattlesList.GetBattleByPlayer(displayName);

        if (battle != null && battle.BattleStarted)
        {
            battle.BattleStarted = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Obtiene el oponente del jugador pasado como parámetro.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un objeto de tipo <see cref="Trainer"/> que representa al oponente del jugador.</returns>
    public Trainer? GetOpponent(string displayName)
    {
        Battle battle = BattlesList.GetBattleByPlayer(displayName);

        if (battle.Player1.DisplayName == displayName)
        {
            return battle.Player2;
        }

        return battle.Player1;
    }

    /// <summary>
    /// Asigna el Pokémon actual de un jugador para la batalla.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <param name="pokemonName">El nombre del Pokémon que se va a asignar.</param>
    /// <returns>Una tupla con un mensaje de éxito o error y el nombre del oponente si es necesario.</returns>
    public (string message, string? OpponentDisplayName) AssignActualPokemon(string displayName, string pokemonName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(displayName);
        Battle? battle = BattlesList.GetBattleByPlayer(displayName);
        var result = InitialVerifications(player, opponent, battle, false);
        if (result != null)
        {
            return result.Value;
        }

        Pokemon foundPokemon = player.GetPokemon(pokemonName);
        
        if (foundPokemon == null)
        {
            return ($"❌ {displayName} no tiene a {pokemonName} en su lista de pokemon.", null);
        }
        player.ActualPokemon = foundPokemon;
        UseCounter++;
        return ($"✅ {pokemonName} esta listo para la batalla.\n{GetPokemonAttacks(displayName, pokemonName)} ", null);
    }

    /// <summary>
    /// Obtiene los ataques disponibles de un Pokémon.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <param name="pokemonName">El nombre del Pokémon cuyos ataques se desean obtener.</param>
    /// <returns>Una cadena de texto con los ataques disponibles del Pokémon.</returns>
    public string GetPokemonAttacks(string displayName, string pokemonName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Pokemon actualPokemon = player.GetPokemon(pokemonName);
        var attackString = new StringBuilder();

        foreach (var attack in actualPokemon.AttackList)
        {
            attackString.AppendLine(attack.AttackInfo());
        }

        return $"Puntos de vida: {actualPokemon.Hp} \n" + "\nLista de ataques:\n" + attackString.ToString();
    }

    /// <summary>
    /// Obtiene el resultado de la batalla entre dos jugadores.
    /// </summary>
    /// <param name="player1DisplayName">El nombre del primer jugador.</param>
    /// <param name="player2DisplayName">El nombre del segundo jugador.</param>
    /// <returns>Un mensaje indicando el ganador de la batalla o null si la batalla aún está en progreso.</returns>
    public string? GetBattleResult(string player1DisplayName, string player2DisplayName)
    {
        // Obtener la batalla actual entre los dos jugadores
        var battle = BattlesList.GetBattleByPlayer(player1DisplayName);

        if (battle == null)
        {
            return null; // No hay batalla activa entre estos jugadores
        }

        // Verificar si uno de los jugadores ha perdido todos sus Pokémon
        if (battle.Player1.GetTotalPokemonLife() == 0)
        {
            return $"{battle.Player2.DisplayName} ha ganado la batalla";
        }

        if (battle.Player2.GetTotalPokemonLife() == 0)
        {
            return $"{battle.Player1.DisplayName} ha ganado la batalla";
        }

        return null; // La batalla aún está en progreso
    }

    /// <summary>
    /// Realiza las verificaciones iniciales para asegurar que la acción es válida.
    /// </summary>
    /// <param name="player">El jugador que realiza la acción.</param>
    /// <param name="opponent">El oponente del jugador.</param>
    /// <param name="battle">La batalla en curso.</param>
    /// <param name="forChange">Indica si la acción es para cambiar Pokémon.</param>
    /// <returns>Una tupla con un mensaje de error si alguna verificación falla, o null si todas las verificaciones son correctas.</returns>
    public (string message, string? OpponentDisplayName)? InitialVerifications(Trainer player, Trainer? opponent,
        Battle? battle, bool? forChange)
    {
        if (opponent == null || battle == null)
        {
            return ("❌ Debes tener un oponente y una batalla empezada para poder realizar esta acción", null);
        }

        if (!battle.BattleStarted)
        {
            return ("❌ La batalla aún no empezó, seleccionen sus Pokémon!", null);
        }
        
        if (forChange != null)
        {
            if (UseCounter >= 2)
            {
                return ($"❌ Ya elegiste tu pokémon inicial. Si necesitas cambiarlo usa !change, recuerda que esto te gastará un turno!", null);
            }
            if (forChange.Value && (player.ActualPokemon == null || opponent.ActualPokemon == null))
            {
                return ($"❌ Los dos jugadores deben tener seleccionado un Pokémon. Usa el comando !use.", null);
            }
        }
        else
        {
            if (player.ActualPokemon == null || opponent.ActualPokemon == null )
            {
                return ($"❌ Los dos jugadores deben tener seleccionado un Pokémon. Usa el comando !use.", null);
            }
            
            if (battle.Turn != player)
            {
                return ($"❌ No puedes realizar esta acción, es el turno de {opponent.DisplayName}", null);
            }
        }
        
        return null;
    }

}

