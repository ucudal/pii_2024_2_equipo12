namespace Poke.Clases;

/// <summary>
/// Representa un ataque especial que puede causar el estado de "Envenenado" en un Pokémon.
/// </summary>
public class Poisoned : Attack
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Poisoned"/>.
    /// </summary>
    /// <param name="name">Nombre del ataque.</param>
    /// <param name="damage">Daño base del ataque.</param>
    /// <param name="attackType">Tipo del ataque según el tipo de Pokémon.</param>
    /// <param name="isSpecial">Indica si el ataque es especial.</param>
    public Poisoned(string name, double damage, Type.PokemonType attackType, bool isSpecial) 
        : base(name, damage, attackType, isSpecial)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Envenenado" al Pokémon objetivo.
    /// Este estado reduce un porcentaje de la salud del Pokémon objetivo en cada turno.
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Envenenado".</param>
    public void Envenenar(Pokemon objective)
    {
        objective.State = "Poisoned";
        if (objective.State == "Poisoned")
        {
            objective.Hp *= 0.95; // Reduce la salud del objetivo en un 5%.
        }
    }
}