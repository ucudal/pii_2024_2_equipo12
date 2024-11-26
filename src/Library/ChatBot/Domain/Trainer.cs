namespace Ucu.Poo.DiscordBot.Domain;
using Poke.Clases;

/// <summary>
    /// Esta clase representa un jugador en el juego Pokémon.
    /// </summary>
    public class Trainer
    {
        private Battle? battle;
        
        /// <summary>
        /// El nombre de usuario de Discord en el servidor del bot del jugador.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// El estado del entrenador. 1: Esperando en la lista de espera,
        /// 2: En una batalla, con los pokemones seleccionados
        /// </summary>
        public int Stage { get; set; }
        
        /// <summary>
        /// Lista de pokemones del entrenador.
        /// </summary>
        public List<Pokemon> PokemonList { get; set; }

        /// <summary>
        /// El pokemon actualmente activo del entrenador.
        /// </summary>
        public Pokemon ActualPokemon { get; set; }

        /// <summary>
        /// Lista de items que el entrenador tiene disponibles.
        /// </summary>
        public List<Item> Items { get; }
        
        
        public int CoolDown { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Trainer"/> con
        /// el nombre de usuario de Discord y el pokemon inicial del entrenador.
        /// </summary>
        /// <param name="displayName">El nombre de usuario de Discord.</param>
        /// <param name="name">Nombre del entrenador.</param>
        /// <param name="actualPokemon">El pokemon inicial del entrenador.</param>
        public Trainer(string displayName)
        {
            DisplayName = displayName;
            PokemonList = new List<Pokemon> { };
            Items = new List<Item>
            {
                new SuperPotion(),
                new SuperPotion(),
                new SuperPotion(),
                new SuperPotion(),
                new TotalCure(),
                new TotalCure(),
                new RevivePotion()
            };
        }

        /// <summary>
        /// Agrega un Pokémon a la selección del usuario.
        /// </summary>
        public string? AddPokemon(Pokemon pokemon)
        {
            if (PokemonList.Count >= 6)
            {
                return "❌ Ya cuentas con 6 pokemon, comienza a pelear!";
            }

            if (!PokemonList.Contains(pokemon))
            {
                PokemonList.Add(pokemon);
                return $"✅ **{pokemon.Name}** ha sido seleccionado.";
            }
            
            return $"❌ Ya cuentas con {pokemon.Name} en tu lista.";
        }

        /// <summary>
        /// Calcula la vida total de todos los pokemones del entrenador.
        /// </summary>
        /// <returns>La suma de los puntos de vida de todos los pokemones.</returns>
        public double GetTotalPokemonLife()
        {
            double totalHp = 0;
            foreach (var pokemon in PokemonList)
            {
                totalHp += pokemon.Hp;
            }
            return totalHp;
        }

        /// <summary>
        /// Verifica si el entrenador tiene un item específico.
        /// </summary>
        /// <param name="item">El item a buscar.</param>
        /// <returns>Verdadero si el entrenador tiene el item; de lo contrario, falso.</returns>
        public bool HasItem(Item item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// Muestra la información de todos los pokemones que posee el entrenador.
        /// </summary>
        public void ShowPokemonsInfo()
        {
            foreach (var pokemon in PokemonList)
            {
                Console.WriteLine($"Nombre: {pokemon.Name}, Vida: {pokemon.Hp}, Nivel: {pokemon.State}");
            }
        }

        /// <summary>
        /// Establece el pokemon actualmente activo del entrenador.
        /// </summary>
        /// <param name="pokemon">El pokemon que será el activo.</param>
        public void SetActualPokemon(Pokemon pokemon)
        {
            if (PokemonList.Contains(pokemon))
            {
                ActualPokemon = pokemon;
            }
            else
            {
                Console.WriteLine("Este pokemon no pertenece al entrenador.");
            }
        }

        /// <summary>
        /// Elimina un item específico de la lista de items del entrenador.
        /// </summary>
        /// <param name="item">El item a eliminar.</param>
        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        /// <summary>
        /// Agrega un item a la lista de items del entrenador.
        /// </summary>
        /// <param name="item">El item a agregar.</param>
        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Usa un item específico en un pokemon objetivo.
        /// </summary>
        /// <param name="item">El item a usar.</param>
        /// <param name="objective">El pokemon en el cual usar el item.</param>
        public void UseItem(Item item, Pokemon objective)
        {
            if (HasItem(item))
            {
                item.Use(objective);
                RemoveItem(item); // Remueve el item después de usarlo si es consumible
                battle.ActualTurn += 1;
                battle.Turn = battle.Turn == battle.Player1 ? battle.Player2 : battle.Player1; // Cambia el turno
            }
            else
            {
                Console.WriteLine("El entrenador no tiene este item.");
            }
        }

        public Pokemon? GetPokemon(string pokemonName)
        {
            foreach (var pokemon in PokemonList)
            {
                if (pokemon.Name == pokemonName)
                {
                    return pokemon;
                }
            }
            return null;
        }
        
        
    }
