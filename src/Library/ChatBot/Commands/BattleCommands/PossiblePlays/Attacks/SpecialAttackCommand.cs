using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'specialAttackPokemon' del bot. Este comando le permite al usuario
/// realizar un ataque especial a un pokemon enemigo.
/// </summary>
// ReSharper disable once UnusedType.Global
public class SpecialAttackCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'specialAttackPokemon'. Este comando le permite al usuario
    /// realizar un ataque especial a un pokemon enemigo.
    /// </summary>
    [Command("specialAttackPokemon")]
    [Summary("Ataca a un pokemon enemigo con un ataque especial")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string specialAttackName, string opponentName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.SpecialAttackPokemon(displayName, opponentName, specialAttackName);
        await ReplyAsync( $"{displayName}:\n {result}");
    }
}