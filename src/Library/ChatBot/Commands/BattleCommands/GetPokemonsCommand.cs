using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

public class GetPokemonsCommand : ModuleBase<SocketCommandContext>
{
    [Command("pokedex")]
    [Summary("Usa una pocion (ítem) en un pokemon")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        await ReplyAsync($"{displayName}:\n{Facade.Instance.GetPokemon(displayName)}");
    }
}