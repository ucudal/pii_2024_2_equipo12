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
    [Command("items")]
    [Summary("Muestra los items disponibles para usar")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string displayName)
    {
        // Responde con el mensaje de lista de Items
        await ReplyAsync($"{displayName}:\n{Facade.Instance.GetAvailableItems(displayName).message}");
    }
}