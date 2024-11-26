using Discord;
using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;
namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'attackPokemon' del bot. Este comando le permite al usuario
/// atacar a un pokemon enemigo.
/// </summary>
// ReSharper disable once UnusedType.Global

public class AttackCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'attackPokemon'. Este comando le permite al usuario
    /// atacar a un pokemon enemigo.
    /// En el caso de que el enemigo no le queden pokemon, entonces el jugador
    /// ganó y se hace display del mensaje correspondiente junto con una imagen ubicada en Assets.
    /// </summary>
    [Command("attack")]
    [Summary("Ataca a un pokemon enemigo")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(string attackName)
    {
            string displayName = CommandHelper.GetDisplayName(Context);
            var result = Facade.Instance.DetermineAttack(displayName, attackName);
            if (result.OpponentDisplayName != null)
            {
                await ReplyAsync($"{result.OpponentDisplayName}:\n {result.message}");
            } 
            else await ReplyAsync($"{displayName}:\n {result.message}");

            if (result.message == "✅ {Player2.DisplayName} ha ganado, no le quedan mas pokemones vivos al oponente!")
            {
                string repoPath = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
                string victoriaImage = Path.Combine(repoPath, "Assets", "VictoriaImage.png");
                
                // Verifica si el archivo existe antes de enviarlo
                if (File.Exists(victoriaImage))
                {
                    using (var stream = new FileStream(victoriaImage, FileMode.Open))
                    {
                        var message = new FileAttachment(stream, "VictoriaImage.png");
                        await Context.Channel.SendFileAsync(message);
                    }
                }
            }
    }
}