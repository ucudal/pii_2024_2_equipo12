namespace Poke.Clases;

/// <summary>
/// Representa un ataque especial que puede causar el estado de "Dormido" en un Pokémon.
/// </summary>
public class IsAsleep : Attack
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IsAsleep"/>.
    /// </summary>
    /// <param name="name">Nombre del ataque.</param>
    /// <param name="damage">Daño base del ataque.</param>
    /// <param name="attackType">Tipo del ataque según el tipo de Pokémon.</param>
    /// <param name="isSpecial">Indica si el ataque es especial.</param>
    public IsAsleep(string name, double damage, Type.PokemonType attackType, bool isSpecial) 
        : base(name, damage, attackType, isSpecial)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Dormido" al Pokémon objetivo. 
    /// Este efecto impide que el Pokémon objetivo pueda atacar durante un número aleatorio de turnos (1 a 4).
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Dormido".</param>
    public void Sleep(Pokemon objective)
    {
        objective.State = "Dormido";
        if (objective.State == "Dormido")
        {
            Random random = new Random();
            double sleepTurns = random.Next(1, 5); // Por 1 a 4 turnos no puede atacar.
            double attackCapacity = 0; // La capacidad de atacar se reduce a 0 mientras está dormido.
        }
    }
}