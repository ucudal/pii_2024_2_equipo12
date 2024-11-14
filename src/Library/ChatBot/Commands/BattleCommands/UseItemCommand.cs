using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'useItem' del bot. Este comando le permite al usuario
/// usar un item en un pokemon luego de pasar los chequeos necesarios.
/// </summary>
// ReSharper disable once UnusedType.Global

public class UseItemCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'useItem'. Este comando le permite al usuario
    /// usar un item en un pokemon luego de pasar los chequeos necesarios.
    /// </summary>
    [Command("useItem")]
    [Summary("Usa un item en un pokemon")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string potionName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.UsePotion(displayName, potionName);
        await ReplyAsync(result);
    }
}