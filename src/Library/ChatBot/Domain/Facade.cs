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
            Trainer? player = BattlesList.GetPlayerInBattle(displayName);
            if (player != null)
            {
                player.Stage = 3;
            }
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
        
        BattlesList.AddBattle(player, opponent);
        return $"Comienza {player.DisplayName} vs {opponent.DisplayName}";
    }

    /// <summary>
    /// Crea una batalla entre dos jugadores.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string StartBattle(string playerDisplayName, string? opponentDisplayName)
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
        Trainer player = this.WaitingList.FindTrainerByDisplayName(displayName);
        if (player.Stage != 4)
        {
            return "❌ No puedes cambiar de pokemon en este momento";
        }
        else
        {
            Pokemon? pokemon = player.Pokemons.Find(pokemon => pokemon.Name == pokemonName);
            if (pokemon != null)
            {
                player.ActualPokemon = pokemon;
                player.Stage = 3; // Ya gasto su turno
                return $"✨🔁 Cambiaste tu Pokemon actual a {pokemonName} ✨🔁";
            }
            else
            {
                return "❌ No tienes ese pokemon";
            }
        }
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
        Trainer player = this.WaitingList.FindTrainerByDisplayName(playerDisplayName);
        if (player.Stage != 4)
        {
            return "❌ No puedes usar pociones en este momento";
        }
        else
        {
            Item? potion = player.Items.Find(item => item.Name == potionName);
            if (potion != null)
            {
                player.UseItem(potion, player.ActualPokemon);
                player.Items.Remove(potion);
                player.Stage = 3; // Ya gasto su turno
                return $"✨🧙 Usaste {potionName} en tu pokemon {player.ActualPokemon.Name} ✨🧙";
            }
            else
            {
                return "❌ No tienes esa poción";
            }
        }
    }

    /// <summary>
    /// Usuario gasta su turno atacando al oponente con un ataque de su pokemon
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="attackName">El nombre del ataque.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns> Un mensaje de confirmación del ataque </returns>
    public string AttackPokemon(string playerDisplayName, string attackName, string opponentDisplayName)
    {
        Trainer player = this.WaitingList.FindTrainerByDisplayName(playerDisplayName);
        Trainer opponent = this.WaitingList.FindTrainerByDisplayName(opponentDisplayName);
        if (player.Stage != 4)
        {
            return "❌ No puedes atacar en este momento";
        }
        else
        {
            Pokemon playerPokemon = player.ActualPokemon;
            Attack? attack = playerPokemon.AttackList.Find(selectedAttack => selectedAttack.Name == attackName);
            if (attack != null && attack.IsSpecial == false ) // Descartar los ataques especiales
            {
                playerPokemon.Attack(opponent.ActualPokemon, playerPokemon, attack);
                player.Stage = 3; // Ya gasto su turno
                return $"✨🔥 {playerPokemon.Name} atacó a {opponent.ActualPokemon.Name} con su ataque {attack.Name} 🔥✨";
            }
            else
            {
                return "❌ No tienes ese ataque";
            }
        }
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
        if (player.Stage != 4)
        {
            return "❌ No puedes atacar en este momento";
        }
        else
        {
            Pokemon playerPokemon = player.ActualPokemon;
            Attack? specialAttack = playerPokemon.AttackList.Find(selectedAttack => selectedAttack.Name == specialAttackName);
            if (specialAttack != null && specialAttack.IsSpecial == true ) // Descartar los ataques normales
            {
                playerPokemon.Attack(opponent.ActualPokemon, playerPokemon, specialAttack);
                player.Stage = 3; // Ya gasto su turno
                return $"✨🔥 {playerPokemon.Name} atacó a {opponent.ActualPokemon.Name} con su ataque especial {specialAttack.Name} 🔥✨";
            }
            else
            {
                return "❌ No tienes ese ataque especial";
            }
        }
    }
    
    public bool IsPlayerInGame(string playerDisplayName)
    {
        if (BattlesList.GetPlayerInBattle(playerDisplayName) == null)
        {
            return false;
        }

        return true;
    }

    public string? PlayerWithPokemon(string playerDisplayName)
    {
        Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
        
        if (player == null)
        {
            return "Comienza una batalla para elegir tus pokemon!";
        } 
        if (player.Pokemons.Count > 0)
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
    public string PokemonSelection(string playerDisplayName, string indices)
{
    // Limpia cualquier selección previa del jugador.
    UserPokemonSelectionService.ClearSelections(playerDisplayName);
    
    // Divide los índices proporcionados y verifica si tienen exactamente 6.
    var selectedIndices = indices.Split(' ')
                                 .Select(i => int.TryParse(i, out int index) ? index - 1 : -1)
                                 .ToList();

    if (selectedIndices.Count != 6)
    {
        return $"❌ Debes seleccionar exactamente 6 Pokémon. Has proporcionado {selectedIndices.Count} índices.";
    }

    if (selectedIndices.Any(index => index < 0))
    {
        return $"❌ Uno o más índices proporcionados no son válidos. Por favor, usa números enteros positivos que esten dentro del catálogo.";
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
        try
        {
            var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
            bool added = UserPokemonSelectionService.AddPokemon(playerDisplayName, pokemon);

            if (added)
            {
                result.AppendLine($"✅ **{pokemon.Name}** ha sido seleccionado.");
            }
            else
            {
                result.AppendLine($"❌ **{pokemon.Name}** ya está en tu lista de seleccionados, haz la seleccion denuevo.");
            }
        }
        catch (ArgumentException)
        {
            result.AppendLine($"❌ No se pudo agregar el Pokémon del índice {index}.");
        }
    }
    
    return result.ToString();
}


    /// <summary>
    /// Muestra los Pokémon actualmente seleccionados por un jugador.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador cuyas selecciones se mostrarán.</param>
    /// <returns>
    /// Una cadena que lista los Pokémon seleccionados por el jugador. Si no
    /// hay selecciones, se indica que la lista está vacía.
    /// </returns>
    public static string ShowCurrentSelections(string playerDisplayName)
    {
        var selections = UserPokemonSelectionService.GetUserSelections(playerDisplayName);
        if (selections.Count == 0)
        {
            return "📭 No has seleccionado ningún Pokémon aún.";
        }

        var sb = new StringBuilder();
        sb.AppendLine("📋 **Tus Pokémon seleccionados actualmente:**");
        for (int i = 0; i < selections.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {selections[i].Name}");
        }
        
        return sb.ToString();
    }
   
    
    /// <summary>
    /// Verifica si un jugador está listo para combatir.
    /// </summary>
    /// <param name="playerDisplayName">El nombre del jugador.</param>
    /// <returns>Un mensaje indicando si el jugador está listo o qué le falta para estarlo.</returns>
    public string CheckIfPlayerIsReady(string playerDisplayName)
    {
        // Obtener la lista de Pokémon seleccionados por el jugador
        var selectedPokemons = UserPokemonSelectionService.GetUserSelections(playerDisplayName);

        // Verificar si tiene exactamente 6 Pokémon seleccionados
        if (selectedPokemons.Count == 6)
        {
            Trainer? player = BattlesList.GetPlayerInBattle(playerDisplayName);
            if (player != null)
            {
                player.Stage = 3;
            }
            return $"✅ {playerDisplayName} está listo para combatir con los siguientes Pokémon:\n" +
                   string.Join("\n", selectedPokemons.Select((pokemon, index) => $"{index + 1}. {pokemon.Name}"));
        }

        // Determinar cuántos Pokémon faltan para llegar a 6
        int pokemonsFaltantes = 6 - selectedPokemons.Count;
        return $"❌ {playerDisplayName} aún no está listo para combatir. Le faltan {pokemonsFaltantes} Pokémon.";
    }
}

