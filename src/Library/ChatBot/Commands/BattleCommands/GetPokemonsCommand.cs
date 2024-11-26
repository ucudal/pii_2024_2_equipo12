using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

public class GetPokemonsCommand : ModuleBase<SocketCommandContext>
{
    [Command("pokemon")]
    [Summary("Usa una pocion (ítem) en un pokemon")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string )
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        await ReplyAsync($"{displayName}:\n{Facade.Instance.GetPokemons(displayName).message}");
    }
}