using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'selectpokemon' del bot. Este comando permite
/// al jugador seleccionar un Pokémon del catálogo.
/// </summary>
// ReSharper disable once UnusedType.Global
public class SelectPokemonCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'selectpokemon'. Este comando permite al jugador
    /// seleccionar un Pokémon del catálogo.
    /// </summary>
    [Command("selectpokemon")]
    [Summary("Permite al usuario seleccionar un Pokémon del catálogo")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string pokemonName)
    {
        if (Enum.TryParse<PokemonCatalog.Catalog>(pokemonName, true, out var catalogEntry))
        {
            var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
            // Aquí puedes agregar la lógica para asignar el Pokémon al jugador
            await ReplyAsync($"Has seleccionado a {pokemon.Name}");
        }
        else
        {
            await ReplyAsync("El nombre del Pokémon no es válido. Por favor, intenta nuevamente.");
        }
    }
}