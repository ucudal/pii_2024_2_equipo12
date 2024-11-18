using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;
using System.Text;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Clase que implementa el comando 'viewselection'. Este comando permite
    /// a los usuarios ver los Pokémon que han seleccionado actualmente.
    /// </summary>
    public class ViewSelectionCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Comando 'viewselection'. Este comando muestra la lista de Pokémon 
        /// seleccionados por el usuario.
        /// </summary>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        [Command("viewselection")]
        [Summary("Muestra los Pokémon que has seleccionado actualmente.")]
        public async Task ExecuteAsync()
        {
            string displayName = CommandHelper.GetDisplayName(Context);
            await ReplyAsync(Facade.Instance.ShowCurrentSelections(displayName));
        }
    }
}