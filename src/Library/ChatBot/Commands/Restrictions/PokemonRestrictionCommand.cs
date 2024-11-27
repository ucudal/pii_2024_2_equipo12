using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.ChatBot.Commands.Restrictions;

public class PokemonRestrictionCommand: ModuleBase<SocketCommandContext>
{
    
    /// <summary>
    /// Implementa el comando 'pokemonRestriction'. Este comando permite
    /// al jugador añadir restricciones de tipos de pokemon a la proxima batalla que comience.
    /// </summary>
    [Command("pokemonRestriction")]
    [Summary("Permite al usuario añadir restricciones de tipos de pokemon previo a comenzar la batalla. Uso: !typeRestriction (nombres de los tipos separados por espacio)")]
    public async Task ExecuteAsync(
        [Remainder]
        [Summary("Índices de los Pokémon a seleccionar separados por espacios")] string typeString)
    {
        await ReplyAsync(Facade.Instance.AddTypeRestriction(typeString));
    }
    
}