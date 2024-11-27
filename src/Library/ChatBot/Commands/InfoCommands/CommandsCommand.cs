using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'comand' del bot. Este comando le muestra
/// al usuario un listado con los comandos.
/// </summary>
// ReSharper disable once UnusedType.Global
public class CommandsCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Esta clase implementa el comando 'comand' del bot. Este comando le muestra
    /// al usuario un listado con los comandos.
    /// </summary>
    [Command("commands")]
    [Summary("Muestra al usuario los comandos posibles")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = "Listado de comandos:\n--Informacion:\n  !who\n  !items\n--WaitingList:\n  !join\n  !leave\n  !playerswaitinglist\n  !stillwaiting\n--Batalla:\n  !battle\n  !catalogue\n  !select\n  !use\n--Posibles Jugadas:\n  !attack\n  !change\n  !usePotion";
        await ReplyAsync(result);
    }
}