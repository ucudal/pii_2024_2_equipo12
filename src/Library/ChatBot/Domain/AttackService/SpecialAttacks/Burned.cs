using Ucu.Poo.DiscordBot.Domain;

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
    public Burned(string name, double damage, Type.PokemonType attackType, bool isSpecial, string? specialType) 
        : base(name, damage, attackType, isSpecial, specialType)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Quemado" al Pokémon objetivo. 
    /// Este efecto reduce el HP del objetivo en un 10%.
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Quemado".</param>
    public string Burn(Pokemon objective)
    {
        objective.State = "Burned";
        objective.Burned = true;
        if (objective.State == "Burned")
        {
            objective.Hp *= 0.9;
        }

        return $"{objective.Name} esta quemandose, durante los proximos turnos ira perdiendo de a 10% del total de su HP.";
    }
    
    public override (string? message, string? specialAttackMessage) AttackOpponent(Trainer? player, Pokemon opponentPokemon, Pokemon playerPokemon, Attack attack)
    {
        if (playerPokemon.AttackCapacity == 1 && opponentPokemon.IsAlive)
        {
            double attackDamage = attack.Damage;
            string message = Burn(opponentPokemon);
            return (opponentPokemon.RecibeDamage(player, CalculateDamage(opponentPokemon)),message);
        }

        return ("No se pudo atacar al oponente",null);
    }
}