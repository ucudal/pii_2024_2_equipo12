using Discord.Commands;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.ChatBot.Commands.ConditionsCommands;
/// <summary>
/// Esta clase implementa el comando 'condicionItem', el cual
/// permite al usuario imponer reglas para los items prohibidos
/// </summary> 

public class ItemsConditionCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'condicionItem', el cual
    /// permite al usuario imponer reglas para los items prohibidos
    /// </summary>
    [Command("condicionItem")]
    [Summary("Permite al usuario imponer reglas para los items prohibidos")]
    public async Task ExecuteAsync(string potionsListOfNames)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.ItemConditions(displayName, potionsListOfNames);
        await ReplyAsync(result);
    }
}