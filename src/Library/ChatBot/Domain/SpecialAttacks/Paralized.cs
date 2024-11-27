using Ucu.Poo.DiscordBot.Domain;

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
    public Paralized(string name, double damage, Type.PokemonType attackType, bool isSpecial, string? specialType) 
        : base(name, damage, attackType, isSpecial, specialType)
    {
    }  

    /// <summary>
    /// Aplica el efecto de "Paralizado" al Pokémon objetivo.
    /// Este estado puede impedir que el Pokémon ataque, dependiendo de un cálculo aleatorio.
    /// </summary>
    /// <param name="objective">El Pokémon objetivo que será afectado por el estado "Paralizado".</param>
    public string Paralize(Pokemon objective)
    {
        objective.State = "Paralizado";
        objective.Paralized = true;
        if (objective.State == "Paralizado")
        {
            Random random = new Random();
            double attackCapacity = random.Next(0, 2); // 0 o 1 definen si puede atacar.
        }

        return $"{objective.Name} esta paralizado y no podra atacar, tal vez en el proximo turno se vuelva a mover o tal vez no.";
    }
    
    public override (string? message, string? specialAttackMessage) AttackOpponent(Trainer? player, Pokemon opponentPokemon, Pokemon playerPokemon, Attack attack)
    {
        if (playerPokemon.AttackCapacity == 1 && opponentPokemon.IsAlive)
        {
            double attackDamage = attack.Damage;
            string message = Paralize(opponentPokemon);
            return (opponentPokemon.RecibeDamage(player, CalculateDamage(opponentPokemon)),message);
        }

        return ("No se pudo atacar al oponente",null);
    }
}