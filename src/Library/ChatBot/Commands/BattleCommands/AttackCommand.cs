using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'attack' del bot. Este comando le permite al usuario
/// atacar a un pokemon enemigo luego de pasar los chequeos necesarios.
/// </summary>
// ReSharper disable once UnusedType.Global
public class AttackCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'attack'. Este comando le permite al usuario
    /// atacar a un pokemon enemigo luego de pasar los chequeos necesarios.
    /// </summary>
    [Command("attack")]
    [Summary("Ataca a un pokemon enemigo")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string enemyName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.Attack(displayName, enemyName);
        await ReplyAsync(result);
    }
}