using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Services;
using System.Text;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Esta clase implementa el comando 'selectpokemon' del bot. Este comando permite
    /// al jugador seleccionar hasta 6 Pokémon del catálogo utilizando sus índices.
    /// </summary>
    public class SelectPokemonCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'selectpokemon'. Este comando permite al jugador
        /// seleccionar hasta 6 Pokémon del catálogo utilizando sus índices.
        /// </summary>
        [Command("selectpokemon")]
        [Summary("Permite al usuario seleccionar hasta 6 Pokémon del catálogo por sus índices. Uso: !selectpokemon <1 2 ... 6>")]
        public async Task ExecuteAsync([Remainder][Summary("Índices de los Pokémon a seleccionar separados por espacios")] string indices)
        {
            var selectedIndices = indices.Split(' ').Select(i => int.TryParse(i, out int index) ? index : -1).ToList();

            if (selectedIndices.Any(index => index < 0))
            {
                await ReplyAsync("❌ Uno o más índices proporcionados no son válidos. Por favor, usa números enteros positivos.");
                return;
            }

            var userSelections = UserPokemonSelectionService.GetUserSelections(Context.User.Id);
            if (userSelections.Count + selectedIndices.Count > 6)
            {
                await ReplyAsync($"❌ Solo puedes seleccionar un máximo de 6 Pokémon. Ya tienes {userSelections.Count} seleccionados.");
                return;
            }

            var catalog = Enum.GetValues(typeof(PokemonCatalog.Catalog)).Cast<PokemonCatalog.Catalog>().ToList();
            var sb = new StringBuilder();
            sb.AppendLine("📋 **Tu seleccion Pokemonha sido:**");

            foreach (var index in selectedIndices)
            {
                if (index < 0 || index >= catalog.Count)
                {
                    sb.AppendLine($"❌ Índice {index} no es válido.");
                    continue;
                }

                var catalogEntry = catalog[index];
                try
                {
                    var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
                    bool added = UserPokemonSelectionService.AddPokemon(Context.User.Id, pokemon);

                    if (added)
                    {
                        sb.AppendLine($"✅ **{pokemon.Name}** ha sido seleccionado.");
                    }
                    else
                    {
                        sb.AppendLine($"❌ **{pokemon.Name}** ya está en tu lista de seleccionados.");
                    }
                }
                catch (ArgumentException)
                {
                    sb.AppendLine($"❌ No se pudo agregar el Pokémon del índice {index}.");
                }
            }

            await ReplyAsync(sb.ToString());
            await ShowCurrentSelections(Context.User.Id);
        }

        /// <summary>
        /// Muestra la lista actual de Pokémon seleccionados por el usuario.
        /// </summary>
        private async Task ShowCurrentSelections(ulong userId)
        {
            var selections = UserPokemonSelectionService.GetUserSelections(userId);
            if (selections.Count == 0)
            {
                await ReplyAsync("📭 No has seleccionado ningún Pokémon aún.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("📋 **Tus Pokémon seleccionados:**");
            for (int i = 0; i < selections.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {selections[i].Name}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}
