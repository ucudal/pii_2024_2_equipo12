using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'Start' del bot. Este comando permite
/// al jugador empezar la batalla.
/// </summary>
public class StartBattleCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'start'. Este comando permite
    /// al jugador empezar la batalla
    /// </summary>
    [Command("start")]
    [Summary(
        "Este comando permite al jugador empezar la batalla")]
    public async Task ExecuteAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);

        if (Facade.Instance.StartBattle(displayName))
        {
            await ReplyAsync($"Comienza {displayName} vs {Facade.Instance.GetOpponent(displayName).DisplayName}");
        }
    }
}