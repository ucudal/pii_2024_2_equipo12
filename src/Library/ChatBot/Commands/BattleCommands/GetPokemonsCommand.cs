using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Clase que implementa el comando para obtener los Pokémon asociados a un jugador.
/// </summary>
public class GetPokemonsCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Comando 'pokemon' que permite al usuario listar los Pokémon que tiene asociados.
    /// </summary>
    /// <returns>
    /// Una tarea asincrónica que envía un mensaje al canal de Discord con los Pokémon del usuario.
    /// </returns>
    [Command("pokemon")]
    [Summary("Muestra la lista de Pokémon asociados a un jugador.")]
    public async Task ExecuteAsync()
    {
        // Obtiene el nombre del usuario que ejecutó el comando en el contexto del mensaje.
        string displayName = CommandHelper.GetDisplayName(Context);

        // Llama al método de la fachada para obtener los Pokémon del usuario y envía la respuesta al canal.
        await ReplyAsync($"{displayName}:\n{Facade.Instance.GetPokemon(displayName)}");
    }
}