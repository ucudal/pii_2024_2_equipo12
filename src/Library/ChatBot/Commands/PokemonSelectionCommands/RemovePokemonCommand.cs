using Discord.Commands;
using Ucu.Poo.DiscordBot.Services;
using System.Text;

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
        [Command("removepokemon")]
        [Summary("Permite al usuario eliminar uno o varios Pokémon de su selección utilizando índices. Uso: /removepokemon <índice1 índice2 ...>")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Índices de los Pokémon a eliminar, separados por espacios")] string indices)
        {
            // Convierte los índices proporcionados en una lista de enteros
            var selectedIndices = indices.Split(' ').Select(i => int.TryParse(i, out int index) ? index - 1 : -1).ToList();

            if (selectedIndices.Any(index => index < 0))
            {
                await ReplyAsync("❌ Uno o más índices proporcionados no son válidos. Por favor, usa números enteros positivos.");
                return;
            }

            var userSelections = UserPokemonSelectionService.GetUserSelections(Context.User.Id);
            if (!userSelections.Any())
            {
                await ReplyAsync("📭 No tienes Pokémon seleccionados para eliminar.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("📋 **Resultados de tu eliminación:**");

            foreach (var index in selectedIndices)
            {
                if (index < 0 || index >= userSelections.Count)
                {
                    sb.AppendLine($"❌ Índice {index + 1} no es válido.");
                    continue;
                }

                var pokemon = userSelections[index];
                bool removed = UserPokemonSelectionService.RemovePokemon(Context.User.Id, pokemon.Name);

                if (removed)
                {
                    sb.AppendLine($"✅ **{pokemon.Name}** ha sido eliminado de tu selección.");
                }
                else
                {
                    sb.AppendLine($"❌ No se pudo eliminar a **{pokemon.Name}**.");
                }
            }

            await ReplyAsync(sb.ToString());
        }
    }
}
