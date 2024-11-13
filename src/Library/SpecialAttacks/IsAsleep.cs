namespace Poke.Clases;

public class IsAsleep : Attack
{
    
    public IsAsleep(string name, double damage, Type.PokemonType attackType, bool isSpecial) : base(name, damage, attackType, isSpecial)
    {
       
    }  
    public void Sleep(Pokemon objective)
    {
        objective.State = "Dormido";
        if (objective.State == "Dormido")
        {
            Random random = new Random();
            double sleepTurns = random.Next(1,5); // Por 1 a 4 turnos no puede atacar
            double attackCapacity = 0;
        }
    }
}