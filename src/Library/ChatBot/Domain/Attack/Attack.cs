using Ucu.Poo.DiscordBot.Domain;

namespace Poke.Clases
{
    /// <summary>
    /// Representa un ataque que un Pokémon puede utilizar en combate.
    /// </summary>
    public class Attack
    {
        /// <summary>
        /// Obtiene o establece el nombre del ataque.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece el valor de daño base del ataque.
        /// </summary>
        public double Damage { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo del ataque (e.g., Fuego, Agua, etc.).
        /// </summary>
        public Type.PokemonType AttackType { get; set; }

        /// <summary>
        /// Indica si el ataque es especial (en lugar de físico).
        /// </summary>
        public bool IsSpecial { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Attack"/>.
        /// </summary>
        /// <param name="name">Nombre del ataque.</param>
        /// <param name="damage">Daño base del ataque.</param>
        /// <param name="attackType">Tipo del ataque.</param>
        /// <param name="isSpecial">Indica si el ataque es especial.</param>
        public Attack(string name, double damage, Type.PokemonType attackType, bool isSpecial)
        {
            Name = name;
            Damage = damage;
            AttackType = attackType;
            IsSpecial = isSpecial;
        }

        /// <summary>
        /// Calcula el daño final del ataque basado en la efectividad del tipo contra el tipo del objetivo.
        /// </summary>
        /// <param name="target">El Pokémon objetivo que recibe el ataque.</param>
        /// <returns>El daño final después de aplicar la ventaja o desventaja de tipo.</returns>
        public double CalculateDamage(Pokemon target)
        {
            // Consultar la ventaja de tipo entre el atacante y el objetivo
            var typeAdvantage = Type.GetTypeAdvantage(this.AttackType, target.Type);

            // Modificar el daño según la ventaja de tipo
            double effectiveness = 1.0;
            switch (typeAdvantage)
            {
                case Type.TypeAdvantage.Advantage:
                    effectiveness = 2.0;  // El tipo de ataque es fuerte contra el tipo objetivo
                    break;
                case Type.TypeAdvantage.Neutral:
                    effectiveness = 1.0;  // No hay ventaja ni desventaja
                    break;
                case Type.TypeAdvantage.Disadvantage:
                    effectiveness = 0.5;  // El tipo de ataque es débil contra el tipo objetivo
                    break;
            }

            // Calcular el daño final aplicando la efectividad
            return Damage * effectiveness;
        }

        public string AttackInfo()
        {
            return $"{Name} (Daño: {Damage}  Tipo: {AttackType})";
        }
        
        
    }
}
