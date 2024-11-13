namespace Poke.Clases;

public class Poisoned : Attack
{
    
    public Poisoned(string name, double damage, Type.PokemonType attackType, bool isSpecial) : base(name, damage, attackType, isSpecial)
    {
       
    }  
    public void Envenenar(Pokemon objective)
    {
        objective.State = "Poisoned";
        if (objective.State == "Poisoned")
        {
            objective.Hp *= 0.95;
        }
    }
}