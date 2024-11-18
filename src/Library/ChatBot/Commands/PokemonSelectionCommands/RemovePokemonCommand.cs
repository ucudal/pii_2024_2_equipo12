using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;
using System.Text;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Clase que implementa el comando 'removepokemon'. Este comando permite
    /// a los usuarios eliminar uno o varios Pokémon de su selección actual
    /// utilizando índices.
    /// </summary>
    public class RemovePokemonCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Comando 'removepokemon'. Este comando elimina uno o varios Pokémon
        /// específicos de la selección del usuario utilizando sus índices.
        /// </summary>
        /// <param name="indices">
        /// Los índices de los Pokémon que el usuario desea eliminar.
        /// </param>
        /// <returns>Una tarea que representa la operación asincrónica.</returns>
        [Command("remove")]
        [Summary(
            "Permite al usuario eliminar uno o varios Pokémon de su selección utilizando índices. Uso: /removepokemon <índice1 índice2 ...>")]
        public async Task ExecuteAsync(
            [Remainder] [Summary("Índices de los Pokémon a eliminar, separados por espacios")]
            string indices)
        {
            string displayName = CommandHelper.GetDisplayName(Context);
            await ReplyAsync(Facade.Instance.RemovePokemon(displayName, indices));
        }
    }
}
