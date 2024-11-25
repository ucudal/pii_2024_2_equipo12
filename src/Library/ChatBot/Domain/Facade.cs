using System.Text;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase recibe las acciones y devuelve los resultados que permiten
/// implementar las historias de usuario. Otras clases que implementan el bot
/// usan esta <see cref="Facade"/> pero no conocen el resto de las clases del
/// dominio. Esta clase es un singleton.
/// </summary>
public class Facade
{
    private static Facade? _instance;

    // Este constructor privado impide que otras clases puedan crear instancias
    // de esta.
    private Facade()
    {
        this.WaitingList = new WaitingList();
        this.BattlesList = new BattlesList();
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
    /// Inicializa este singleton. Es necesario solo en los tests.
    /// </summary>
    public static void Reset()
    {
        _instance = null;
    }
    
    private WaitingList WaitingList { get; }
    
    private BattlesList BattlesList { get; }

    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
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
    /// Remueve un jugador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El jugador a remover.</param>
    /// <returns>Un mensaje con el resultado.</returns>
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
    /// Obtiene la lista de jugadores esperando.
    /// </summary>
    /// <returns>Un mensaje con el resultado.</returns>
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
    /// Determina si un jugador está esperando para jugar.
    /// </summary>
    /// <param name="displayName">El jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string TrainerIsWaiting(string displayName)
    {
        Trainer? trainer = this.WaitingList.FindTrainerByDisplayName(displayName);
        if (trainer == null)
        {
            return $"No estás esperando!";
        }
        
        return $"Estás esperando!";
    }


    private string CreateBattle(Trainer player, Trainer opponent)
    {
        // Aunque playerDisplayName y opponentDisplayName no estén en la lista
        // esperando para jugar los removemos igual para evitar preguntar si
        // están para luego removerlos.
        this.WaitingList.RemoveTrainer(player.DisplayName);
        this.WaitingList.RemoveTrainer(opponent.DisplayName);
        player.Stage = 2;
        opponent.Stage = 2;
        
        BattlesList.AddBattle(player, opponent);
        return $"Comienza {player.DisplayName} vs {opponent.DisplayName}";
    }

    /// <summary>
    /// Crea una batalla entre dos jugadores.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string CreateNewBattle(string playerDisplayName, string? opponentDisplayName)
    {
        // El símbolo ? luego de Trainer indica que la variable opponent puede
        // referenciar una instancia de Trainer o ser null.
        Trainer? opponent;
        Trainer player = this.WaitingList.FindTrainerByDisplayName(playerDisplayName);
        
        if (!OpponentProvided() && !SomebodyIsWaiting())
        {
            return "No hay nadie esperando";
        }
        
        if (!OpponentProvided()) // && SomebodyIsWaiting
        {
            opponent = this.WaitingList.GetAnyoneWaiting();
            
            // El símbolo ! luego de opponent indica que sabemos que esa
            // variable no es null. Estamos seguros porque SomebodyIsWaiting
            // retorna true si y solo si hay usuarios esperando y en tal caso
            // GetAnyoneWaiting nunca retorna null.
            return this.CreateBattle(player, opponent!);
        }

        // El símbolo ! luego de opponentDisplayName indica que sabemos que esa
        // variable no es null. Estamos seguros porque OpponentProvided hubiera
        // retorna false antes y no habríamos llegado hasta aquí.
        
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
    /// <returns>Un mensaje con los items disponibles.</returns>
    public string GetAvailableItems(string playerDisplayName)
    {
        Console.WriteLine("Pociones disponibles: ");
        if (this.WaitingList.FindTrainerByDisplayName(playerDisplayName).Items != null)
        {
            foreach (Item item in this.WaitingList.FindTrainerByDisplayName(playerDisplayName).Items)
            {
                Console.WriteLine($"{item.Name}");
                return "Se imprimió la lista de pociones disponibles";
            }
        }
        return "No hay pociones disponibles";
    }
    
    /// <summary>
    /// Cambia el pokemon actual del jugador por otro de su lista de pokemones.
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="pokemonName"></param>
    /// <returns>Un mensaje de comfirmacion del cambio del Pokemon.</returns>

    public string ChangePokemon(string displayName, string pokemonName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(displayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(displayName);
        Battle? battle = BattlesList.GetBattleByPlayer(displayName);
        if (battle == null)
        {
            return "❌ Tienes que estar en una batalla para cambiar a un pokemon.";
        }
        if (battle.BattleStarted)
        {
            if (player.ActualPokemon != null && opponent.ActualPokemon != null)
            {
                if (battle.Turn == player)
                {
                    Pokemon? pokemon = player.PokemonList.Find(pokemon => pokemon.Name == pokemonName);
                    if (pokemon != null)
                    {
                        if (pokemon.IsAlive)
                        {
                            player.ActualPokemon = pokemon;
                            string? battleFinished = battle.ChangeTurn();
                            if (battleFinished != null)
                            {
                                return battleFinished;
                            }
                            return $"✨🔁 Cambiaste tu Pokemon actual a {pokemonName} ✨🔁";
                        }
                        return $"❌ {pokemon.Name} esta muerto, no puedes elegir este pokemon.";
                    }
                    return "❌ No tienes ese pokemon";
                }
                return $"❌ No puedes cambiar pokemon, es el turno de {opponent.DisplayName}";
            }
            return $"❌ Los dos jugadores deben tener seleccionado un pokemon, usa el comando !use";
        }
        return "❌ La batalla aun no empezó, selecciona tus pokemon!";
    }
    
    /// <summary>
    /// Usuario gasta su turno eligiendo una poción luego de seleccionarla de 
    /// su lista de pociones impresas en la consola.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="potionName">El nombre de la poción.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string UsePotion(string playerDisplayName, string potionName)
    {
        Trainer player = BattlesList.GetPlayerInBattle(playerDisplayName);
        Battle battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        if (player == null || battle == null)
        {
            return "❌ No puedes usar pociones en este momento";
        }

        Item? potion = player.Items.Find(item => item.Name == potionName);
        if (potion != null)
        {
            player.UseItem(potion, player.ActualPokemon);
            player.Items.Remove(potion);
            return $"✨🧙 Usaste {potionName} en tu pokemon {player.ActualPokemon.Name} ✨🧙";
        }
        return "❌ No tienes esa poción";
        
    }

    /// <summary>
    /// Usuario gasta su turno atacando al oponente con un ataque de su pokemon
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="attackName">El nombre del ataque.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns> Un mensaje de confirmación del ataque </returns>
    public (string message, string? OpponentDisplayName) AttackPokemon(string playerDisplayName, string attackName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
        Trainer? opponent = BattlesList.GetOpponnentInBattle(playerDisplayName);
        Battle? battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        if (opponent == null || battle == null)
        {
            return ("❌ Debes tener un oponente y una batalla empezada para poder atacar", null);
        }

        if (battle.BattleStarted)
        {
            if (player.ActualPokemon != null && opponent.ActualPokemon != null)
            {
                if (battle.Turn == player)
                {
                    Pokemon playerPokemon = player.ActualPokemon;
                    if (playerPokemon.IsAlive)
                    {
                        Attack? attack = playerPokemon.AttackList.Find(selectedAttack => selectedAttack.Name == attackName);
                        if (attack != null && attack.IsSpecial == false ) // Descartar los ataques especiales
                        {
                            string? isOpponentPokemonDead = playerPokemon.Attack(player, opponent.ActualPokemon, playerPokemon, attack);
                            string? battleFinished = battle.ChangeTurn();
                            if (battleFinished != null)
                            {
                                return (battleFinished, null);
                            }
                            if (isOpponentPokemonDead != null)
                            {
                                return (isOpponentPokemonDead, opponent.DisplayName);
                            }
                            return ($"✨🔥 {playerPokemon.Name} atacó a {opponent.ActualPokemon.Name} con su ataque {attack.Name} 🔥✨", null);
                        }
                        return ("❌ No tienes ese ataque", null);
                    }
                    return ($"❌ {playerPokemon.Name} ha muerto, usa el comando !change para cambiar a tu próximo pokemon.", null);
                }
                return ($"❌ No puedes atacar, es el turno de {opponent.DisplayName}", null);
            }
            return ($"❌ Los dos jugadores deben tener seleccionado un pokemon, usa el comando !use", null);
        }
        return ("❌ La batalla aun no empezó, selecciona tus pokemon!", null);
    }
    
    /// <summary>
    /// Usuario gasta su turno atacando al oponente con un ataque especial de su pokemon
    /// </summary>
    /// <param name="playerDisplayName"></param>
    /// <param name="specialAttackName"></param>
    /// <param name="opponentDisplayName"></param>
    /// <returns> Un mensaje de confirmación del ataque especial </returns>
    public string SpecialAttackPokemon(string playerDisplayName, string specialAttackName, string opponentDisplayName)
    {
        Trainer player = this.WaitingList.FindTrainerByDisplayName(playerDisplayName);
        Trainer opponent = this.WaitingList.FindTrainerByDisplayName(opponentDisplayName);
        Battle battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        if (player.Stage != 2 || battle.Turn != player || battle.ActualTurn % 2 != 0) // si Actualturn es impar, no puede atacar
        {
            return "❌ No puedes atacar en este momento";
        }
        else
        {
            Pokemon playerPokemon = player.ActualPokemon;
            Attack? specialAttack = playerPokemon.AttackList.Find(selectedAttack => selectedAttack.Name == specialAttackName);
            if (specialAttack != null && specialAttack.IsSpecial == true ) // Descartar los ataques normales
            {
                playerPokemon.Attack(null, opponent.ActualPokemon, playerPokemon, specialAttack);
                battle.ActualTurn += 1;
                battle.ChangeTurn();
                return $"✨🔥 {playerPokemon.Name} atacó a {opponent.ActualPokemon.Name} con su ataque especial {specialAttack.Name} 🔥✨";
            }
            return "❌ No tienes ese ataque especial";
            
        }
    }
    
    /// <summary>
    /// Verifica si un jugador está en una batalla.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un booleano indicando si el jugador pertenece a una batalla.</returns>
    public bool IsPlayerInGame(string playerDisplayName)
    {
        if (BattlesList.GetPlayerInBattle(playerDisplayName) == null)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifica si un jugador cuenta con los pokemon necesarios para combatir.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un mensaje indicando si el jugador tiene los 6 pokemon o null en el caso que el jugador no este en una batalla.</returns>
    public string? PlayerWithPokemon(string playerDisplayName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
        
        if (player == null)
        {
            return "Comienza una batalla para elegir tus pokemon!";
        } 
        if (player.PokemonList.Count == 6)
        {
            return "Ya tienes seleccionados tus pokemon, comienza a pelear!";
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
    /// Una cadena que contiene mensajes de confirmación o error relacionados
    /// con el proceso de selección.
    /// </returns> 
    public (string ListaPokemon, string? ReadyForBattleMessage, Trainer? InitialTurn) 
        PokemonSelection(string playerDisplayName, string indices)
    {
    Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
    Battle? battle = BattlesList.GetBattleByPlayer(playerDisplayName);
        if (player.Stage != 2)
        {
            return ("❌ No puedes seleccionar pokemones en este momento.", null, null);
        }
        
        // Limpia cualquier selección previa del jugador.
        player.PokemonList.Clear();
        
        // Divide los índices proporcionados y verifica si tienen exactamente 6.
        var selectedIndices = indices.Split(' ')
                                     .Select(i => int.TryParse(i, out int index) ? index - 1 : -1)
                                     .ToList();

        if (selectedIndices.Any(index => index < 0))
        {
            return (
                "❌ Uno o más índices proporcionados no son válidos. Por favor, usa números enteros positivos que esten dentro del catálogo.",
                null, null);
        }

        var catalog = Enum.GetValues(typeof(PokemonCatalog.Catalog)).Cast<PokemonCatalog.Catalog>().ToList();
        var result = new StringBuilder();

        foreach (var index in selectedIndices)
        {
            if (index < 0 || index >= catalog.Count)
            {
                result.AppendLine($"❌ Índice {index} no es válido, haz la seleccion de nuevo.");
                continue;
            }

            var catalogEntry = catalog[index];
            var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
            result.AppendLine(player.AddPokemon(pokemon));
        }

        if (battle.ReadyForBattle())
        {
            battle.BattleStarted = true;
            return (result.ToString(), $"{playerDisplayName} y {battle.Player2.DisplayName} tienen 6 pokemon, comienza la batalla!", battle.InitialTurn());
        }
        return (result.ToString(), null, null);
    }
    
    /*public string InitializePokemons()
    {
        allPokemons = new List<Pokemon>();
        
        allPokemons.Add(new Pokemon("Bulbasaur", 200, 1, null, Poke.Clases.Type.PokemonType.Plant, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Bulbasaur]));
        allPokemons.Add(new Pokemon("Charmander", 200, 1, null, Poke.Clases.Type.PokemonType.Fire, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Charmander]));
        allPokemons.Add(new Pokemon("Squirtle", 200, 1, null, Poke.Clases.Type.PokemonType.Water, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Squirtle]));
        allPokemons.Add(new Pokemon("Pikachu", 200, 1, null, Poke.Clases.Type.PokemonType.Electric, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Pikachu]));
        allPokemons.Add(new Pokemon("Jigglypuff", 200, 1, null, Poke.Clases.Type.PokemonType.Psychic, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Jigglypuff]));
        allPokemons.Add(new Pokemon("Eevee", 190, 1, null, Poke.Clases.Type.PokemonType.Normal, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Eevee]));
        allPokemons.Add(new Pokemon("Psyduck", 210, 1, null, Poke.Clases.Type.PokemonType.Water, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Psyduck]));
        allPokemons.Add(new Pokemon("Geodude", 220, 1, null, Poke.Clases.Type.PokemonType.Rock, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Geodude]));
        allPokemons.Add(new Pokemon("Machop", 230, 1, null, Poke.Clases.Type.PokemonType.Fighter, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Machop]));
        allPokemons.Add(new Pokemon("Gastly", 190, 1, null, Poke.Clases.Type.PokemonType.Ghost, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Gastly]));
        allPokemons.Add(new Pokemon("Oddish", 180, 1, null, Poke.Clases.Type.PokemonType.Plant, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Oddish]));
        allPokemons.Add(new Pokemon("Growlithe", 200, 1, null, Poke.Clases.Type.PokemonType.Fire, PokemonCatalog.PokemonAttacks[PokemonCatalog.Catalog.Growlithe]));

        return "Pokomons iniciados."; 
    }*/
    
    /// <summary>
    /// Verifica si un jugador está listo para combatir.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un mensaje indicando si el jugador está listo o qué le falta para estarlo.</returns>
    /*public string CheckIfPlayerIsReady(string playerDisplayName)
    {
        // Obtener la lista de Pokémon seleccionados por el jugador
        var selectedPokemons = UserPokemonSelectionService.GetUserSelections(playerDisplayName);

        // Verificar si tiene exactamente 6 Pokémon seleccionados
        if (selectedPokemons.Count == 6)
        {
            Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
            if (player != null)
            {
                player.Stage = 2;
            }
            return $"✅ {playerDisplayName} está listo para combatir con los siguientes Pokémon:\n" +
                   string.Join("\n", selectedPokemons.Select((pokemon, index) => $"{index + 1}. {pokemon.Name}"));
        }

        // Determinar cuántos Pokémon faltan para llegar a 6
        int pokemonsFaltantes = 6 - selectedPokemons.Count;
        return $"❌ {playerDisplayName} aún no está listo para combatir. Le faltan {pokemonsFaltantes} Pokémon.";
    }*/
    
    /// <summary>
    /// Verifica si la batalla esta lista para iniciar.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <returns>Un booleano indicando si la batalla está lista.</returns>
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
    /// Obtiene el opponente del jugador pasado como parametro.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un entrenador oponente.</returns>
    public Trainer? GetOpponent(string displayName)
    {
        Battle battle = BattlesList.GetBattleByPlayer(displayName);
        
        if (battle.Player1.DisplayName == displayName)
        {
            return battle.Player2;
        }
        
        return battle.Player1;
    }

    public string? AssignActualPokemon(string displayName, string pokemonName)
    {
        Trainer player = BattlesList.GetPlayerInBattle(displayName);
        Pokemon foundPokemon = player.GetPokemon(pokemonName);

        if (foundPokemon == null)
        {
            return $"❌ {displayName} no tiene a {pokemonName} en su lista de pokemon.";
        }
        
        player.ActualPokemon = foundPokemon;
        return $"✅ {pokemonName} esta listo para la batalla.";
    }

    public string GetPokemonAttacks(string displayName, string pokemonName)
    {
        Trainer player = BattlesList.GetPlayerInBattle(displayName);
        Pokemon actualPokemon = player.GetPokemon(pokemonName);
        var result = new StringBuilder();

        foreach (var attack in actualPokemon.AttackList)
        {
            result.AppendLine(attack.AttackInfo());
        }

        return "Lista de ataques:\n" + result.ToString();
    }
    
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

}

