using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class ResetSelectionCommand : ModuleBase<SocketCommandContext>
    {
        [Command("resetselection")]
        [Summary("Reinicia tu selección de Pokémon.")]
        public async Task ExecuteAsync()
        {
            UserPokemonSelectionService.ClearSelections(Context.User.Id);
            await ReplyAsync("🗑️ Has reiniciado tu selección de Pokémon.");
        }
    }
}