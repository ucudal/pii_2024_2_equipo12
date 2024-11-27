namespace Poke.Clases;

public class RevivePotion : Item
{
    public RevivePotion()
    {
        Name = "RevivePotion";
    }

    public override void Use(Pokemon objective)
    {
        if (objective.GetHp() == 0)
        {
            objective.AddHP(objective.InitialHealth / 2);  // Revive con el 50% del HP total
        }
    }
}