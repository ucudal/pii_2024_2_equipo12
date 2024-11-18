using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Clase que implementa el comando 'removepokemon'. Este comando permite
    /// a los usuarios eliminar un Pokémon de su selección actual.
    /// </summary>
    public class RemovePokemonCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Comando 'removepokemon'. Este comando elimina un Pokémon específico
        /// de la selección del usuario.
        /// </summary>
        /// <param name="pokemonName">
        /// El nombre del Pokémon que el usuario desea eliminar de su selección.
        /// </param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        [Command("removepokemon")]
        [Summary("Permite al usuario eliminar un Pokémon de su selección. Uso: /removepokemon <nombre_pokemon>")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Nombre del Pokémon a eliminar")] string pokemonName)
        {
            // Intenta eliminar el Pokémon de la selección del usuario
            bool removed = UserPokemonSelectionService.RemovePokemon(Context.User.Id, pokemonName);

            // Envía un mensaje según el resultado de la operación
            if (removed)
            {
                await ReplyAsync($"✅ Has eliminado a **{pokemonName}** de tu selección.");
            }
            else
            {
                await ReplyAsync("❌ No encontré ese Pokémon en tu selección.");
            }
        }
    }
}