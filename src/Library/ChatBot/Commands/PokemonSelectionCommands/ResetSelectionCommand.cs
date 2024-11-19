using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Clase que implementa el comando 'resetselection'. Este comando permite
    /// a un usuario reiniciar su selección de Pokémon.
    /// </summary>
    public class ResetSelectionCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Comando 'resetselection'. Este comando elimina la selección de Pokémon
        /// realizada por el usuario y muestra un mensaje de confirmación.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        [Command("resetselection")]
        [Summary("Reinicia tu selección de Pokémon.")]
        public async Task ExecuteAsync()
        {
            string displayName = CommandHelper.GetDisplayName(Context);
            await ReplyAsync(Facade.Instance.ResetPokemonSelection(displayName));
        }
    }
}