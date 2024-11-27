using Discord.Commands;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.ChatBot.Commands.ConditionsCommands;
/// <summary>
/// Esta clase implementa el comando 'condicionPoke', el cual
/// permite al usuario imponer reglas para pokemones especificos
/// </summary> 

public class PokemonConditionCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'condicionPoke', el cual
    /// permite al usuario imponer reglas para pokemones especificos
    /// </summary>
    [Command("condicionPoke")]
    [Summary("Permite al usuario imponer reglas para pokemones especifico")]
    public async Task ExecuteAsync(string pokemonsListOfNames)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.PokeConditions(displayName, pokemonsListOfNames);
        await ReplyAsync(result);
    }
}