namespace Poke.Clases;

public class SuperPotion : Items
{
    public SuperPotion()
    {
        Name = "Súper Pocion";
    }

    public override void Use(Pokemon objective)
    {
        objective.AddHP(70);  // Recupera 70 puntos de HP
    }
}
