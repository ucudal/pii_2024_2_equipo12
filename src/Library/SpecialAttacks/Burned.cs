namespace Poke.Clases;

public class Burned : Attack
{
    public Burned(string name, double damage, Type.PokemonType attackType, bool isSpecial) : base(name, damage, attackType, isSpecial)
    {
       
    }  
    public void Burn(Pokemon objective)
    {
        objective.State = "Burned";
        if (objective.State == "Burned")
        {
            objective.Hp *= 0.9;
        }
    }
}