namespace Poke.Clases;

/// <summary>
/// Representa un ataque especial que puede causar el estado de "Paralizado" en un Pokémon.
/// </summary>
public class Paralized : Attack
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Paralized"/>.
    /// </summary>
    /// <param name="name">Nombre del ataque.</param>
    /// <param name="damage">Daño base del ataque.</param>
    /// <param name="attackType">Tipo del ataque según el tipo de Pokémon.</param>
    /// <param name="isSpecial">Indica si el ataque es especial.</param>
    public Paralized(string name, double damage, Type.PokemonType attackType, bool isSpecial) 
        : base(name, damage, attackType, isSpecial)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Paralizado" al Pokémon objetivo.
    /// Este estado puede impedir que el Pokémon ataque, dependiendo de un cálculo aleatorio.
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Paralizado".</param>
    public void Paralize(Pokemon objective)
    {
        objective.State = "Paralizado";
        if (objective.State == "Paralizado")
        {
            Random random = new Random();
            double attackCapacity = random.Next(0, 2); // 0 o 1 definen si puede atacar.
        }
    }
}