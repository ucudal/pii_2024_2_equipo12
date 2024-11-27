using Discord.Commands;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.ChatBot.Commands.ConditionsCommands;
/// <summary>
/// Esta clase implementa el comando 'accept', el cual
/// permite al usuario aceptar las condiciones de su oponente
/// </summary> 

public class AcceptCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'accept', el cual
    /// permite al usuario aceptar las condiciones de su oponente
    /// </summary>
    [Command("accept")]
    [Summary("Permite al usuario aceptar las condiciones de su oponente")]
    public async Task ExecuteAsync(string opponentName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.ConditionsCheckAceptar(displayName, opponentName);
        await ReplyAsync(result);
    }
}