using Discord.Commands;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.ChatBot.Commands.ConditionsCommands;
/// <summary>
/// Esta clase implementa el comando 'reject', el cual
/// permite al usuario rechazar las condiciones de su oponente
/// </summary> 

public class RejectCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'reject', el cual
    /// permite al usuario rechazar las condiciones de su oponente
    /// </summary>
    [Command("reject")]
    [Summary("Permite al usuario rechazar las condiciones de su oponente")]
    public async Task ExecuteAsync(string opponentName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.ConditionsCheckRechazar(displayName, opponentName);
        await ReplyAsync(result);
    }
}