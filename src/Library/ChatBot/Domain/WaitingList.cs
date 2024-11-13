using System.Collections.ObjectModel;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa la lista de jugadores esperando para jugar.
/// </summary>
public class WaitingList
{
    private List<Trainer> trainersList = new List<Trainer>();

    public WaitingList(Trainer? player1 = null, Trainer? player2 = null)
    {
        if (player1 != null)
        {
            trainersList.Add(player1);
        }
        if (player2 != null)
        {
            trainersList.Add(player2);
        }
    }
    public int Count
    {
        get { return this.trainersList.Count; }
    }
    
    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a agregar.
    /// </param>
    /// <returns><c>true</c> si se agrega el usuario; <c>false</c> en caso
    /// contrario.</returns>
    public bool AddTrainerToWaitlist(string displayName, Trainer trainer) // Se ingresa los 2, uno tipo string para buscar y otro tipo Trainer para agregar
    {
        if (this.FindTrainerByDisplayName(displayName) != null)
        {
            return false;
        }
        else
        {
            trainersList.Add(trainer);
            return true;
        }
    }

    /// <summary>
    /// Remueve un jugador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a remover.
    /// </param>
    /// <returns><c>true</c> si se remueve el usuario; <c>false</c> en caso
    /// contrario.</returns>
    public bool RemoveTrainer(string displayName)
    {
        Trainer? trainer = this.FindTrainerByDisplayName(displayName);
        if (trainer == null) return false;
        trainersList.Remove(trainer);
        return true;
    }

    /// <summary>
    /// Busca un jugador por el nombre de usuario de Discord en el servidor del
    /// bot.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a buscar.
    /// </param>
    /// <returns>El jugador encontrado o <c>null</c> en caso contrario.
    /// </returns>
    public Trainer? FindTrainerByDisplayName(string displayName)
    {
        foreach (Trainer trainer in this.trainersList)
        {
            if (trainer.DisplayName == displayName)
            {
                return trainer;
            }
        }

        return null;
    }
    
    
    // hacer summary
    public List<Trainer> CheckIn()
    {
        var playersToPlay = new List<Trainer>();
        
        if (trainersList.Count >= 2)
        {
            playersToPlay.Add(trainersList[0]);
            playersToPlay.Add(trainersList[1]);

            Console.WriteLine($"{playersToPlay[0].DisplayName} y {playersToPlay[1].DisplayName} están listos para jugar.");

            trainersList.RemoveRange(0, 2); // Elimina los dos primeros elementos de la lista de espera
        }
        else
        {
            Console.WriteLine("No hay suficientes personas en la lista de espera para comenzar el juego.");
        }

        return playersToPlay;
    }

    /// <summary>
    /// Retorna un jugador cualquiera esperando para jugar. En esta
    /// implementación provista no es cualquiera, sino el primero. En la
    /// implementación definitiva, debería ser uno aleatorio.
    /// 
    /// </summary>
    /// <returns></returns>
    public Trainer? GetAnyoneWaiting()
    {
        if (this.trainersList.Count == 0)
        {
            return null;
        }

        return this.trainersList[0];
    }
    public bool HasEnoughPlayers()
    {
        return trainersList.Count >= 2;
    }
}