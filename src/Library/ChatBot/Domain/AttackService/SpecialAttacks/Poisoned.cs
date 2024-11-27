using Ucu.Poo.DiscordBot.Domain;

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
    public Poisoned(string name, double damage, Type.PokemonType attackType, bool isSpecial, string? specialType) 
        : base(name, damage, attackType, isSpecial, specialType)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Envenenado" al Pokémon objetivo.
    /// Este estado reduce un porcentaje de la salud del Pokémon objetivo en cada turno.
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Envenenado".</param>
    public string Envenenar(Pokemon objective)
    {
        objective.State = "Poisoned";
        objective.Poisoned = true;
        if (objective.State == "Poisoned")
        {
            objective.Hp *= 0.95; // Reduce la salud del objetivo en un 5%.
        }

        return $"{objective.Name} esta envenenado, durante los proximos turnos ira perdiendo de a 5% del total de su HP.";
    }
    
    public override (string? message, string? specialAttackMessage) AttackOpponent(Trainer? player, Pokemon opponentPokemon, Pokemon playerPokemon, Attack attack)
    {
        if (playerPokemon.AttackCapacity == 1 && opponentPokemon.IsAlive)
        {
            double attackDamage = attack.Damage;
            string message = Envenenar(opponentPokemon);
            return (opponentPokemon.RecibeDamage(player, CalculateDamage(opponentPokemon)),message);
        }

        return ("No se pudo atacar al oponente",null);
    }
}