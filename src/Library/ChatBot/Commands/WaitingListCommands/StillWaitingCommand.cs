using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'stillWaiting' del bot. Este comando le muestra
/// al usuario si él esta esperando para jugar, es decir, si esta dentro de la Waitlist.
/// </summary>
// ReSharper disable once UnusedType.Global
public class StillWaitingCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'stillWaiting'. Este comando le muestra al usuario si él
    /// esta esperando para jugar, es decir, si esta dentro de la Waitlist.
    /// </summary>
    [Command("stillWaiting")]
    [Summary("Muestra si el usuario que envía el mensaje está en la lista de espera")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.TrainerIsWaiting(displayName);
        await ReplyAsync(result);
    }
}