using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class RemovePokemonCommand : ModuleBase<SocketCommandContext>
    {
        [Command("removepokemon")]
        [Summary("Permite al usuario eliminar un Pokémon de su selección. Uso: /removepokemon <nombre_pokemon>")]
        public async Task ExecuteAsync([Remainder][Summary("Nombre del Pokémon a eliminar")] string pokemonName)
        {
            bool removed = UserPokemonSelectionService.RemovePokemon(Context.User.Id, pokemonName);
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