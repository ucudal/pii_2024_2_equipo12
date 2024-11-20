using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'playersWaitingList' del bot. Este comando muestra
/// la lista de jugadores esperando para jugar.
/// </summary>
// ReSharper disable once UnusedType.Global
public class PlayersWaitingCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'playersWaitingList'. Este comando muestra la lista de
    /// jugadores esperando para jugar.
    /// </summary>
    [Command("playersWaitingList")]
    [Summary("Muestra los usuarios en la lista de espera")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        string result = Facade.Instance.GetAllTrainersWaiting();

        await ReplyAsync(result);
    }
}
