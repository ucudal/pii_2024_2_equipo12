using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

public class UseInitialPokemonCommand : ModuleBase<SocketCommandContext>
{
    [Command("use")]
    [Summary("Selecciona el primer pokemon para batallar")]
    
    public async Task ExecuteAsync(string pokemonName, double health)
    {
        string displayName = CommandHelper.GetDisplayName(Context);

        await ReplyAsync($"{displayName}:\n{Facade.Instance.AssignActualPokemon(displayName, pokemonName)}"  +
                         $"\n   {Facade.Instance.GetPokemonAttacks(displayName, pokemonName, health)}" );
    }
}