using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'changepokemon' del bot. Este comando permite
/// al jugador cambiar su Pokémon actual por otro del catálogo.
/// </summary>
// ReSharper disable once UnusedType.Global
public class ChangePokemonCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'changepokemon'. Este comando permite al jugador
    /// cambiar su Pokémon actual por otro del catálogo.
    /// </summary>
    [Command("changepokemon")]
    [Summary("Permite al usuario cambiar su Pokémon actual por otro del catálogo")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string pokemonName)
    {
        if (Enum.TryParse<PokemonCatalog.Catalog>(pokemonName, true, out var catalogEntry))
        {
            var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
            // Aquí puedes agregar la lógica para asignar el nuevo Pokémon al jugador
            await ReplyAsync($"Has cambiado a {pokemon.Name}");
        }
        else
        {
            await ReplyAsync("El nombre del Pokémon no es válido. Por favor, intenta nuevamente.");
        }
    }
}