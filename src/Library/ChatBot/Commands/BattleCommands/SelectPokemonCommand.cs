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
        [Command("select")]
        [Summary("Permite al usuario seleccionar hasta 6 Pokémon del catálogo por sus índices. Uso: !selectpokemon <1 2 ... 6>")]
        public async Task ExecuteAsync(
            [Remainder]
            [Summary("Índices de los Pokémon a seleccionar separados por espacios")] string indices)
        {
            string displayName = CommandHelper.GetDisplayName(Context);
            var result = Facade.Instance.PokemonSelection(displayName, indices);
            await ReplyAsync($"{displayName}:\n{result.message}");
            if (result.ReadyForBattleMessage != null)
            {
                await ReplyAsync($"{result.ReadyForBattleMessage}\n\nUsa el comando !use para elegir tu pokemon inicial!");
            }
            if (result.InitialTurn != null)
            {
                await ReplyAsync($"Es el turno de {result.InitialTurn.DisplayName}");
            }
        }
    }
}