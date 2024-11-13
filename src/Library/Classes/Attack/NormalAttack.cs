namespace Poke.Clases;

public class NormalAttack : Attack
{
    ///<summary>
    /// Constructor para inicializar un ataque
    /// </summary>
    public NormalAttack(string name, double damage, Type.PokemonType attackType, bool isSpecial) : base(name, damage, attackType, isSpecial)
    {
       
    }  
}