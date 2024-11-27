using Discord.Commands;
using Ucu.Poo.DiscordBot.Commands;

namespace Ucu.Poo.DiscordBot.ChatBot.Commands.Restrictions;

public class RestrictionsCommand
{
    /// <summary>
    /// Esta clase implementa el comando 'restrictions' del bot. Este comando permite
    /// al usuario visualizar en pantalla las restricciones que se pueden hacer a la batalla.
    /// </summary>
    public class SelectPokemonCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'restrictions' del bot. Este comando permite
        /// al usuario visualizar en pantalla las restricciones que se pueden hacer a la batalla.
        /// </summary>
        [Command("restrictions")]
        [Summary("Permite al usuario visualizar en pantalla las restricciones que se pueden hacer a la batalla. Uso: !restrictions (nombres de los tipos separados por espacio)")]
        public async Task ExecuteAsync()
        {
            await ReplyAsync("Las restricciones posibles sobre la batalla son:\n  !typeRestriction (nombres de los tipos separados por espacio)\n  !pokemonRestriction (Indice de los pokemon a restringir, el indice lo puedes ver con !catalogue)\n  !itemRestriction (nombre de los item a restringir)");
            
        }
    }
}