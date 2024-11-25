using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'changepokemon' del bot. Este comando permite
/// al jugador cambiar su Pokémon actual por otro de los seleccionados.
/// </summary>
// ReSharper disable once UnusedType.Global
public class ChangePokemonCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'changepokemon'. Este comando permite al jugador
    /// cambiar su Pokémon actual por otro de los seleccionados.
    /// </summary>
    [Command("change")]
    [Summary("Permite al usuario cambiar su Pokémon actual por otro de los seleccionados.")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string pokemonName)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        await ReplyAsync(  $"{displayName}:\n{Facade.Instance.ChangePokemon(displayName, pokemonName)}");
    }
}