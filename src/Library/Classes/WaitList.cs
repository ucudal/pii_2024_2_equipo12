namespace Poke.Clases;

public class WaitList
{
    private List<OriginalTrainer> waitList;

    public WaitList(OriginalTrainer? player1 = null, OriginalTrainer? player2 = null)
    {
        waitList = new List<OriginalTrainer>();
        waitList.Add(player1);
        waitList.Add(player2);
    }

    public void AddToWaitList(OriginalTrainer originalTrainer)
    {
        if (originalTrainer != null)
        {
            waitList.Add(originalTrainer);
            Console.WriteLine($"{originalTrainer.name} ha sido añadido a la lista de espera.");
        }
    }

    public List<OriginalTrainer> CheckIn()
    {
        var playersToPlay = new List<OriginalTrainer>();
        
        if (waitList.Count >= 2)
        {
            playersToPlay.Add(waitList[0]);
            playersToPlay.Add(waitList[1]);

            Console.WriteLine($"{playersToPlay[0].name} y {playersToPlay[1].name} están listos para jugar.");

            waitList.RemoveRange(0, 2); // Elimina los dos primeros elementos de la lista de espera
        }
        else
        {
            Console.WriteLine("No hay suficientes personas en la lista de espera para comenzar el juego.");
        }

        return playersToPlay;
    }

    public bool HasPlayers()
    {
        return waitList.Count >= 2;
    }
}