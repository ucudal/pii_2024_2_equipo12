using Discord.Commands;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.ChatBot.Commands.ConditionsCommands;
/// <summary>
/// Esta clase implementa el comando 'condicionTipo', el cual
/// permite al usuario imponer reglas para los tipos de pokemones
/// </summary> 

public class TypeConditionCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'condicionTipo', el cual
    /// permite al usuario imponer reglas para los tipos de pokemones
    /// </summary>
    [Command("condicionTipo")]
    [Summary("Permite al usuario imponer reglas para los tipos de pokemones")]
    public async Task ExecuteAsync(string typesListOfNames)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.TypeCondition(displayName, typesListOfNames);
        await ReplyAsync(result);
    }
}