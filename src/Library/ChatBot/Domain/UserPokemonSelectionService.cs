using System.Collections.Concurrent;
using Poke.Clases;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Services
{
    public static class UserPokemonSelectionService
    {
        // Usa ConcurrentDictionary para manejar accesos concurrentes de múltiples usuarios
        private static readonly ConcurrentDictionary<string, List<Pokemon>> UserSelections = new();

        /// <summary>
        /// Agrega un Pokémon a la selección del usuario.
        /// </summary>
        public static bool AddPokemon(string playerDisplayName, Pokemon pokemon)
        {
            var selections = UserSelections.GetOrAdd(playerDisplayName, new List<Pokemon>());
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
        public static List<Pokemon> GetUserSelections(string playerDisplayName)
        {
            UserSelections.TryGetValue(playerDisplayName, out var selections);
            return selections ?? new List<Pokemon>();
        }
        

        /// <summary>
        /// Limpia todas las selecciones del usuario.
        /// </summary>
        public static void ClearSelections(string playerDisplayName)
        {
            UserSelections.TryRemove(playerDisplayName, out _);
        }
    }
}
