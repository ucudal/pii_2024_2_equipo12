using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'attackPokemon' del bot. Este comando le permite al usuario
/// atacar a un pokemon enemigo.
/// </summary>
// ReSharper disable once UnusedType.Global

public class AttackCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'attackPokemon'. Este comando le permite al usuario
    /// atacar a un pokemon enemigo.
    /// </summary>
    [Command("attackPokemon")]
    [Summary("Ataca a un pokemon enemigo")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string attackName, string opponentName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.AttackPokemon(displayName, opponentName, attackName);
        await ReplyAsync(result);
    }
}