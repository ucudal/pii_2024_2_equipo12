using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'availableItems' del bot. Este comando le permite al usuario
/// ver los items disponibles para usar.
/// </summary>
// ReSharper disable once UnusedType.Global

public class AvailableItemsCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'availableItems'. Este comando le permite al usuario
    /// ver los items disponibles para usar.
    /// </summary>
    [Command("availableItems")]
    [Summary("Muestra los items disponibles para usar")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string displayName)
    {
        string result = Facade.Instance.GetAvailableItems(displayName);
        await ReplyAsync(result);
    }
}