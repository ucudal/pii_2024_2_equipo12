namespace Ucu.Poo.DiscordBot.Domain;
using Poke.Clases;

/// <summary>
    /// Esta clase representa un jugador en el juego Pokémon.
    /// </summary>
    public class Trainer
    {
        /// <summary>
        /// El nombre de usuario de Discord en el servidor del bot del jugador.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// El estado del entrenador.
        /// </summary>
        public int Stage { get; set; }
        // 0: Cumple los requisitos para jugar
        // 1: Esperando en la lista de espera
        // 2: En una batalla, sin seleccionar los pokemones todavia
        // 3: En una batalla
        // 4: En una batalla, en su turno
        
        /// <summary>
        /// Lista de pokemones del entrenador.
        /// </summary>
        public List<Pokemon> Pokemons { get; }

        /// <summary>
        /// El pokemon actualmente activo del entrenador.
        /// </summary>
        public Pokemon ActualPokemon { get; private set; }

        /// <summary>
        /// Lista de items que el entrenador tiene disponibles.
        /// </summary>
        public List<Item> Items { get; }

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
            Stage = 0;
            Pokemons = new List<Pokemon> { };
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
        /// Agrega un pokemon a la lista de pokemones del entrenador.
        /// </summary>
        /// <param name="pokemon">El pokemon a agregar.</param>
        public void AddPokemon(Pokemon pokemon)
        {
            if (Pokemons.Count < 6)
            {
                Pokemons.Add(pokemon);
            }
            else
            {
                Console.WriteLine("El entrenador ya tiene el número máximo de pokemones.");
            }
        }

        /// <summary>
        /// Calcula la vida total de todos los pokemones del entrenador.
        /// </summary>
        /// <returns>La suma de los puntos de vida de todos los pokemones.</returns>
        public double GetTotalPokemonLife()
        {
            double totalHp = 0;
            foreach (var pokemon in Pokemons)
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
            foreach (var pokemon in Pokemons)
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
            if (Pokemons.Contains(pokemon))
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
            }
            else
            {
                Console.WriteLine("El entrenador no tiene este item.");
            }
        }
    }
