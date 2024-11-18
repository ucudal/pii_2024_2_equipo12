using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;
using System.Text;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class ViewSelectionCommand : ModuleBase<SocketCommandContext>
    {
        [Command("viewselection")]
        [Summary("Muestra los Pokémon que has seleccionado actualmente.")]
        public async Task ExecuteAsync()
        {
            var selections = UserPokemonSelectionService.GetUserSelections(Context.User.Id);
            if (selections.Count == 0)
            {
                await ReplyAsync("📭 No has seleccionado ningún Pokémon aún.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("📋 **Tus Pokémon seleccionados:**");
            for (int i = 0; i < selections.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {selections[i].Name}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}