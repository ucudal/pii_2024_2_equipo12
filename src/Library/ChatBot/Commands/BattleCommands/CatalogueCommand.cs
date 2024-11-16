using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

public class CatalogueCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'selectPokemon'. Este comando permite al usuario
    /// visualizar el catalogo pokemon en pantalla.
    /// </summary>
    [Command("catalogue")]
    [Summary(
        """
        Muestra el catalogo de los pokemon como imagen en discord
        """)]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);

        if (true)//Facade.Instance.IsPlayerInGame(displayName))
        {
            string repoPath = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
            string imagePath1 = Path.Combine(repoPath, "Assets", "catalogo1.png");
            string imagePath2 = Path.Combine(repoPath, "Assets", "catalogo2.png");
            
            // Verifica si el archivo existe antes de enviarlo
            if (File.Exists(imagePath1) && File.Exists(imagePath2))
            {
                using (var stream = new FileStream(imagePath1, FileMode.Open))
                {
                    var message = new FileAttachment(stream, "catalogo1.png");
                    await Context.Channel.SendFileAsync(message, "Para seleccionar los pokemon usa el comando ''!select'' seguido de los indices separados por espacio \nEjemplo: !select 2 5 7");
                }
                using (var stream = new FileStream(imagePath2, FileMode.Open))
                {
                    var message = new FileAttachment(stream, "catalogo2.png");
                    await Context.Channel.SendFileAsync(message);
                }
            }
            else
            {
                await ReplyAsync("No se encontro el catalogo");
            }
        }
    }
}