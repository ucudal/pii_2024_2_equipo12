using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Services;
using System.Text;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Esta clase implementa el comando 'selectpokemon' del bot. Este comando permite
    /// al jugador seleccionar hasta 6 Pokémon del catálogo.
    /// </summary>
    public class SelectPokemonCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Implementa el comando 'selectpokemon'. Este comando permite al jugador
        /// seleccionar un Pokémon del catálogo.
        /// </summary>
        [Command("selectpokemon")]
        [Summary("Permite al usuario seleccionar un Pokémon del catálogo. Uso: /selectpokemon <nombre_pokemon>")]
        public async Task ExecuteAsync([Remainder][Summary("Nombre del Pokémon a seleccionar")] string pokemonName)
        {
            if (Enum.TryParse<PokemonCatalog.Catalog>(pokemonName, true, out var catalogEntry))
            {
                try
                {
                    var pokemon = PokemonCatalog.CreatePokemon(catalogEntry);
                    bool added = UserPokemonSelectionService.AddPokemon(Context.User.Id, pokemon);

                    if (added)
                    {
                        await ReplyAsync($"✅ Has seleccionado a **{pokemon.Name}**.");
                        await ShowCurrentSelections(Context.User.Id);
                    }
                    else
                    {
                        var currentSelections = UserPokemonSelectionService.GetUserSelections(Context.User.Id);
                        if (currentSelections.Count >= 6)
                        {
                            await ReplyAsync("❌ Ya has seleccionado 6 Pokémon. No puedes seleccionar más.");
                        }
                        else
                        {
                            await ReplyAsync("❌ Este Pokémon ya lo has seleccionado.");
                        }
                    }
                }
                catch (ArgumentException)
                {
                    await ReplyAsync("❌ El nombre del Pokémon no es válido. Por favor, intenta nuevamente.");
                }
            }
            else
            {
                await ReplyAsync("❌ El nombre del Pokémon no es válido. Por favor, intenta nuevamente.");
            }
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
