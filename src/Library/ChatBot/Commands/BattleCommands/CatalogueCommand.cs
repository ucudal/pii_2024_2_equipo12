using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Clase que implementa comandos relacionados con el catálogo de Pokémon.
/// </summary>
public class CatalogueCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Comando 'catalogue' que muestra el catálogo de Pokémon como imágenes en Discord.
    /// Permite a los usuarios visualizar un listado de Pokémon disponibles y seleccionar sus opciones.
    /// </summary>
    /// <returns>Una tarea asincrónica que envía las imágenes del catálogo al canal de Discord.</returns>
    [Command("catalogue")]
    [Summary(
        """
        Muestra el catálogo de los Pokémon como imagen en Discord.
        """
    )]
    public async Task ExecuteAsync()
    {
        // Obtiene el nombre de usuario del contexto del comando.
        string displayName = CommandHelper.GetDisplayName(Context);

        // Verifica si el usuario ya tiene un Pokémon seleccionado.
        string? result = Facade.Instance.PlayerWithPokemon(displayName);
        if (result != null)
        {
            // Si el usuario tiene un Pokémon seleccionado, muestra un mensaje de confirmación.
            await ReplyAsync(result);
        }
        else
        {
            // Obtiene la ruta al directorio base del repositorio y las imágenes del catálogo.
            string repoPath = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
            string imagePath1 = Path.Combine(repoPath, "Assets", "catalogo1.png");
            string imagePath2 = Path.Combine(repoPath, "Assets", "catalogo2.png");
            
            // Verifica si las imágenes existen antes de enviarlas.
            if (File.Exists(imagePath1) && File.Exists(imagePath2))
            {
                // Envía la primera imagen al canal.
                using (var stream = new FileStream(imagePath1, FileMode.Open))
                {
                    var message = new FileAttachment(stream, "catalogo1.png");
                    await Context.Channel.SendFileAsync(
                        message,
                        "Para seleccionar los Pokémon usa el comando '!select' seguido de los índices separados por espacio.\n" +
                        "Ejemplo: !select 2 5 7"
                    );
                }

                // Envía la segunda imagen al canal.
                using (var stream = new FileStream(imagePath2, FileMode.Open))
                {
                    var message = new FileAttachment(stream, "catalogo2.png");
                    await Context.Channel.SendFileAsync(message);
                }
            }
            else
            {
                // Informa al usuario si las imágenes no se encuentran en la ruta especificada.
                await ReplyAsync("No se encontró el catálogo.");
            }
        }
    }
}
