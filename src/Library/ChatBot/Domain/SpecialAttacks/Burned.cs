namespace Poke.Clases;

/// <summary>
/// Representa un ataque especial que puede causar el estado de "Quemado" en un Pokémon.
/// </summary>
public class Burned : Attack
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Burned"/>.
    /// </summary>
    /// <param name="name">Nombre del ataque.</param>
    /// <param name="damage">Daño base del ataque.</param>
    /// <param name="attackType">Tipo del ataque según el tipo de Pokémon.</param>
    /// <param name="isSpecial">Indica si el ataque es especial.</param>
    public Burned(string name, double damage, Type.PokemonType attackType, bool isSpecial) 
        : base(name, damage, attackType, isSpecial)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Quemado" al Pokémon objetivo. 
    /// Este efecto reduce el HP del objetivo en un 10%.
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Quemado".</param>
    public void Burn(Pokemon objective)
    {
        objective.State = "Burned";
        if (objective.State == "Burned")
        {
            objective.Hp *= 0.9;
        }
    }
}