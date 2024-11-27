using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.ChatBot.Commands.Restrictions;


/// <summary>
/// Esta clase implementa el comando 'typeRestriction' del bot. Este comando permite
/// al jugador añadir restricciones de tipos de pokemon a la batalla.
/// </summary>
public class ItemRestrictionCommand : ModuleBase<SocketCommandContext>
{
    [Command("typeRestriction")]
    [Summary("Permite al usuario añadir restricciones de tipos de pokemon previo a comenzar la batalla. Uso: !typeRestriction (nombres de los tipos separados por espacio)")]
    public async Task ExecuteAsync(
        [Remainder]
        [Summary("Índices de los Pokémon a seleccionar separados por espacios")] string typeString)
    {
        await ReplyAsync(Facade.Instance.AddTypeRestriction(typeString));
    }
}