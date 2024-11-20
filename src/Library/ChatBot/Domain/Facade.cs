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
            return $"{displayName} no está esperando";
        }
        
        return $"{displayName} está esperando";
    }


    private string CreateBattle(string playerDisplayName, string opponentDisplayName)
    {
        // Aunque playerDisplayName y opponentDisplayName no estén en la lista
        // esperando para jugar los removemos igual para evitar preguntar si
        // están para luego removerlos.
        this.WaitingList.RemoveTrainer(playerDisplayName);
        this.WaitingList.RemoveTrainer(opponentDisplayName);
        
        BattlesList.AddBattle(playerDisplayName, opponentDisplayName);
        return $"Comienza {playerDisplayName} vs {opponentDisplayName}";
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
            return this.CreateBattle(playerDisplayName, opponent!.DisplayName);
        }

        // El símbolo ! luego de opponentDisplayName indica que sabemos que esa
        // variable no es null. Estamos seguros porque OpponentProvided hubiera
        // retorna false antes y no habríamos llegado hasta aquí.
        opponent = this.WaitingList.FindTrainerByDisplayName(opponentDisplayName!);
        
        if (!OpponentFound())
        {
            return $"{opponentDisplayName} no está esperando";
        }
        
        return this.CreateBattle(playerDisplayName, opponent!.DisplayName);
        
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
    /// Usuario gasta su turno eligiendo una poción luego de seleccionarla de 
    /// su lista de pociones impresas en la consola.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="potionName">El nombre de la poción.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string UsePotion(string playerDisplayName, string potionName)
    {
        // Primer check: si el jugador está en la lista de espera
        if (this.WaitingList.FindTrainerByDisplayName(playerDisplayName) != null)
        {
            return $"{playerDisplayName} está en la lista de espera y no puede usar una poción"; // Sale si está en la lista de espera
        }
        else
        {
            // Segundo check: si el jugador está en una batalla
            if (IsPlayerInGame(playerDisplayName) == false)
            {
                return $"{playerDisplayName} no está en una batalla"; // Sale si no está en una batalla
            }
            else
            {
                // Segundo check: si es el turno del jugador
                if ( true /* turno actual NO corresponde al jugador */ )
                {
                    return $"{playerDisplayName} no es su turno, por lo tanto no puede usar la pocion"; // Sale si no es su turno
                }
                else
                {
                    // Si llega hasta acá, es porque el jugador está en una batalla y es su turno
                    // es decir, puede usar la poción

                    // Busca el ítem en el inventario del entrenador
                    Trainer? trainerToUse = this.WaitingList.FindTrainerByDisplayName(playerDisplayName);
                    Item? itemToUse = trainerToUse.Items.FirstOrDefault(i => i.GetType().Name == potionName); // FistOrDefault devuelve el primer elemento que cumple la condición
                    if (itemToUse == null)
                    {
                        return $"{playerDisplayName} no tiene un {potionName}.";
                    }
                    else
                    {
                        // Usa la poción
                        itemToUse.Use(trainerToUse.ActualPokemon);
                        return $"{playerDisplayName} usó {potionName}";
                    }
                }
            }   
        }
    }

    public bool IsPlayerInGame(string playerDisplayName)
    {
        return BattlesList.FindPlayerInBattle(playerDisplayName);
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
            return $"✅ {playerDisplayName} está listo para combatir con los siguientes Pokémon:\n" +
                   string.Join("\n", selectedPokemons.Select((pokemon, index) => $"{index + 1}. {pokemon.Name}"));
        }

        // Determinar cuántos Pokémon faltan para llegar a 6
        int pokemonsFaltantes = 6 - selectedPokemons.Count;
        return $"❌ {playerDisplayName} aún no está listo para combatir. Le faltan {pokemonsFaltantes} Pokémon.";
    }
}
