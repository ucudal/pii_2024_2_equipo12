using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'usepotion' del bot. Este comando permite
/// al jugador usar una poción en su Pokémon actual.
/// </summary>
// ReSharper disable once UnusedType.Global
public class UsePotionCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'usepotion'. Este comando permite al jugador
    /// usar una poción en su Pokémon actual.
    /// </summary>
    [Command("usepotion")]
    [Summary("Permite al usuario usar una poción en su Pokémon actual")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string potionName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        string result = Facade.Instance.UsePotion(displayName, potionName);
        await ReplyAsync(result);
    }
}