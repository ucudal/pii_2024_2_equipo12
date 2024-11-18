using System.Collections.Concurrent;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Services
{
    public static class UserPokemonSelectionService
    {
        // Usa ConcurrentDictionary para manejar accesos concurrentes de múltiples usuarios
        private static readonly ConcurrentDictionary<ulong, List<Pokemon>> UserSelections = new();

        /// <summary>
        /// Agrega un Pokémon a la selección del usuario.
        /// </summary>
        public static bool AddPokemon(ulong userId, Pokemon pokemon)
        {
            var selections = UserSelections.GetOrAdd(userId, new List<Pokemon>());
            if (selections.Count >= 6)
            {
                return false; // Límite alcanzado
            }

            if (selections.Any(p => p.Name.Equals(pokemon.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Pokémon ya seleccionado
            }

            selections.Add(pokemon);
            return true;
        }

        /// <summary>
        /// Obtiene la lista de Pokémon seleccionados por el usuario.
        /// </summary>
        public static List<Pokemon> GetUserSelections(ulong userId)
        {
            UserSelections.TryGetValue(userId, out var selections);
            return selections ?? new List<Pokemon>();
        }

        /// <summary>
        /// Elimina un Pokémon de la selección del usuario.
        /// </summary>
        public static bool RemovePokemon(ulong userId, string pokemonName)
        {
            if (UserSelections.TryGetValue(userId, out var selections))
            {
                var pokemon = selections.FirstOrDefault(p => p.Name.Equals(pokemonName, StringComparison.OrdinalIgnoreCase));
                if (pokemon != null)
                {
                    selections.Remove(pokemon);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Limpia todas las selecciones del usuario.
        /// </summary>
        public static void ClearSelections(ulong userId)
        {
            UserSelections.TryRemove(userId, out _);
        }
    }
}
