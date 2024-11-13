namespace Poke.Clases;

public class Paralized : Attack
{
    
    public Paralized(string name, double damage, Type.PokemonType attackType, bool isSpecial) : base(name, damage, attackType, isSpecial)
    {
       
    }  
    public void Paralize(Pokemon objective)
    {
        objective.State = "Paralizado";
        if (objective.State == "Paralizado")
            
        {
            Random random = new Random();
            double attackCapacity = random.Next(0,2); // 0 o 1 definen si puede atacar
        }
    }
}