namespace Poke.Clases;

public class TotalCure : Item
{
    public TotalCure()
    {
        Name = "TotalCure";
    }

    public override void Use(Pokemon objective)
    {
        // Elimina todos los estados negativos del Pokémon objetivo
        objective.State = "Normal";
        objective.SleepState = null;
        objective.Paralized = false;
        objective.Poisoned = false;
        objective.Burned = false;
    }
}