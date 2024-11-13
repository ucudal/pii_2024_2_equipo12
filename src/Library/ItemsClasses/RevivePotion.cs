namespace Poke.Clases;

public class RevivePotion : Items
{
    public RevivePotion()
    {
        Name = "Revivir";
    }

    public override void Use(Pokemon objective)
    {
        if (objective.GetHp() == 0)
        {
            objective.AddHP(objective.Hp / 2);  // Revive con el 50% del HP total
        }
    }
}