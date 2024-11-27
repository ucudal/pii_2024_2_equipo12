namespace Poke.Clases;

public class SuperPotion : Item
{
    public SuperPotion()
    {
        Name = "SuperPotion";
    }

    public override void Use(Pokemon objective)
    {
        objective.AddHP(70);  // Recupera 70 puntos de HP
    }
}
