using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase permite que el Trainer pueda hacer sus propias restricciones sobre los pokemons a usar. 
/// </summary>

public class RestringirPokemonCommand : ModuleBase<SocketCommandContext>
{

    [Command("restringirPokemons")]
    [Summary("Restringe el uso de las Pokemons seleccionados.")]

    public async Task ExecuteAsync(
        [Remainder] [Summary("Restricciones a seleccionar separados por espacios.")]
        string elementos)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        // obtenemos el nombre del usuario.

        var result = Facade.Instance.RestrinccionesPociones(displayName, elementos);

        await ReplyAsync($"{displayName}:\n{elementos}");

        if (elementos != null)
        {
            await ReplyAsync($"Elegiste las siguiente Pokemons para restringir: {elementos}");
        }

        if (elementos == null)
        {
            await ReplyAsync($"Por favor selecciona los Pokemons a restringir.");
        }
    }
}