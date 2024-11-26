using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para seleccionar el primer Pokémon para batallar.
    /// </summary>
    public class UseInitialPokemonCommand : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Ejecuta el comando para seleccionar un Pokémon.
        /// </summary>
        /// <param name="pokemonName">El nombre del Pokémon a seleccionar para la batalla.</param>
        /// <returns>Una tarea asincrónica que se completa cuando el comando se ha ejecutado.</returns>
        [Command("use")]
        [Summary("Selecciona el primer pokemon para batallar")]
        public async Task ExecuteAsync(string pokemonName)
        {
            // Obtiene el nombre del jugador desde el contexto del comando
            string displayName = CommandHelper.GetDisplayName(Context);

            // Responde con el mensaje de asignación del Pokémon y la lista de ataques
            await ReplyAsync($"{displayName}:\n{Facade.Instance.AssignActualPokemon(displayName, pokemonName).message}"  +
                             $"\n{Facade.Instance.GetPokemonAttacks(displayName, pokemonName)}");
        }
    }
}