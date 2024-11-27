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
    public bool AddTrainer(string displayName) 
    {
        if (this.FindTrainerByDisplayName(displayName) != null)
        {
            return false;
        }
        
        Trainer newTrainer = new Trainer(displayName);
        trainersList.Add(newTrainer);
        
        return true;
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
    
    /// <summary>
    /// Retorna un jugador cualquiera esperando para jugar. En esta
    /// implementación provista no es cualquiera, sino el primero. En la
    /// implementación definitiva, debería ser uno aleatorio.
    /// 
    /// </summary>
    /// <returns></returns>
    public Trainer? GetAnyoneWaiting()
    {
        if (trainersList.Count == 0)
        {
            return null;
        }

        return trainersList[0];
    }
   
    
    /// <summary>
    /// Retorna una lista de todos los jugadores en la lista de espera.
    /// </summary>
    /// <returns>Una lista de jugadores esperando para jugar.</returns>
    public List<Trainer> GetAllWaiting()
    {
        return this.trainersList;
    }
}