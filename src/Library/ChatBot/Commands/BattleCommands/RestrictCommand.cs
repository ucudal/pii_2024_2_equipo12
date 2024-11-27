using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'usePotion' del bot. Este comando le permite al usuario
/// usar un item en un pokemon luego de pasar los chequeos necesarios.
/// </summary>
// ReSharper disable once UnusedType.Global

public class RestrictCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'usePotion'. Este comando le permite al usuario
    /// usar un item en un pokemon luego de pasar los chequeos necesarios.
    /// </summary>
    [Command("restrict")]
    [Summary("Restringe una regla")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string restrictionName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        await ReplyAsync($"{displayName}:\n{Facade.Instance.RestrictRule(displayName, restrictionName)}");
    }
}