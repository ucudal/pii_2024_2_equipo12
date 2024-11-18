using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;
using System.Text;

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
            // Obtiene la lista de Pokémon seleccionados por el usuario
            var selections = UserPokemonSelectionService.GetUserSelections(Context.User.Id);

            // Verifica si el usuario no ha seleccionado ningún Pokémon
            if (selections.Count == 0)
            {
                await ReplyAsync("📭 No has seleccionado ningún Pokémon aún.");
                return;
            }

            // Construye un mensaje con la lista de Pokémon seleccionados
            var sb = new StringBuilder();
            sb.AppendLine("📋 **Tus Pokémon seleccionados:**");
            for (int i = 0; i < selections.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {selections[i].Name}");
            }

            // Envía el mensaje al usuario
            await ReplyAsync(sb.ToString());
        }
    }
}