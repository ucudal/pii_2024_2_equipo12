namespace Poke.Clases
{
    /// <summary>
    /// Clase estática que define los tipos de Pokémon y sus ventajas/desventajas de tipo.
    /// </summary>
    public class Type
    {
        /// <summary>
        /// Enumera los tipos de Pokémon disponibles.
        /// </summary>
        public enum PokemonType
        {
            Water,
            Bug,
            Dragon,
            Electric,
            Ghost,
            Fire,
            Ice,
            Fighter,
            Normal,
            Plant,
            Psychic,
            Rock,
            Earth,
            Poison,
            Flying
        }

        /// <summary>
        /// Define el tipo de ventaja en una batalla.
        /// </summary>
        public enum TypeAdvantage
        {
            /// <summary>
            /// Ventaja de tipo.
            /// </summary>
            Advantage = 1,

            /// <summary>
            /// Tipo neutral.
            /// </summary>
            Neutral = 0,

            /// <summary>
            /// Desventaja de tipo.
            /// </summary>
            Disadvantage = -1
        }

        /// <summary>
        /// Diccionario que contiene las combinaciones de tipos y sus respectivas ventajas.
        /// </summary>
        public static readonly Dictionary<(PokemonType, PokemonType), TypeAdvantage> typeAdvantages = new()
        {
            { (PokemonType.Water, PokemonType.Fire), TypeAdvantage.Advantage },
            { (PokemonType.Water, PokemonType.Rock), TypeAdvantage.Advantage },
            { (PokemonType.Water, PokemonType.Earth), TypeAdvantage.Advantage },
            { (PokemonType.Bug, PokemonType.Plant), TypeAdvantage.Advantage },
            { (PokemonType.Bug, PokemonType.Psychic), TypeAdvantage.Advantage },
            { (PokemonType.Dragon, PokemonType.Dragon), TypeAdvantage.Advantage },
            { (PokemonType.Electric, PokemonType.Water), TypeAdvantage.Advantage },
            { (PokemonType.Electric, PokemonType.Flying), TypeAdvantage.Advantage },
            { (PokemonType.Ghost, PokemonType.Ghost), TypeAdvantage.Advantage },
            { (PokemonType.Ghost, PokemonType.Psychic), TypeAdvantage.Advantage },
            { (PokemonType.Fire, PokemonType.Plant), TypeAdvantage.Advantage },
            { (PokemonType.Fire, PokemonType.Bug), TypeAdvantage.Advantage },
            { (PokemonType.Fire, PokemonType.Ice), TypeAdvantage.Advantage },
            { (PokemonType.Ice, PokemonType.Plant), TypeAdvantage.Advantage },
            { (PokemonType.Ice, PokemonType.Earth), TypeAdvantage.Advantage },
            { (PokemonType.Ice, PokemonType.Flying), TypeAdvantage.Advantage },
            { (PokemonType.Ice, PokemonType.Dragon), TypeAdvantage.Advantage },
            { (PokemonType.Fighter, PokemonType.Normal), TypeAdvantage.Advantage },
            { (PokemonType.Fighter, PokemonType.Rock), TypeAdvantage.Advantage },
            { (PokemonType.Fighter, PokemonType.Ice), TypeAdvantage.Advantage },
            { (PokemonType.Plant, PokemonType.Water), TypeAdvantage.Advantage },
            { (PokemonType.Plant, PokemonType.Earth), TypeAdvantage.Advantage },
            { (PokemonType.Plant, PokemonType.Rock), TypeAdvantage.Advantage },
            { (PokemonType.Psychic, PokemonType.Fighter), TypeAdvantage.Advantage },
            { (PokemonType.Rock, PokemonType.Fire), TypeAdvantage.Advantage },
            { (PokemonType.Rock, PokemonType.Flying), TypeAdvantage.Advantage },
            { (PokemonType.Rock, PokemonType.Ice), TypeAdvantage.Advantage },
            { (PokemonType.Rock, PokemonType.Bug), TypeAdvantage.Advantage },
            { (PokemonType.Earth, PokemonType.Fire), TypeAdvantage.Advantage },
            { (PokemonType.Earth, PokemonType.Electric), TypeAdvantage.Advantage },
            { (PokemonType.Earth, PokemonType.Poison), TypeAdvantage.Advantage },
            { (PokemonType.Earth, PokemonType.Rock), TypeAdvantage.Advantage },
            { (PokemonType.Poison, PokemonType.Plant), TypeAdvantage.Advantage },
            { (PokemonType.Flying, PokemonType.Plant), TypeAdvantage.Advantage },
            { (PokemonType.Flying, PokemonType.Fighter), TypeAdvantage.Advantage },
            { (PokemonType.Flying, PokemonType.Bug), TypeAdvantage.Advantage }
        };

        /// <summary>
        /// Obtiene la ventaja de tipo entre dos Pokémon.
        /// </summary>
        /// <param name="type1">Tipo del Pokémon atacante.</param>
        /// <param name="type2">Tipo del Pokémon defensor.</param>
        /// <returns>La ventaja de tipo entre los dos Pokémon (ventaja, neutral o desventaja).</returns>
        public static TypeAdvantage GetTypeAdvantage(PokemonType type1, PokemonType type2)
        {
            if (type1 == type2)
                return TypeAdvantage.Neutral;

            return typeAdvantages.TryGetValue((type1, type2), out var advantage)
                ? advantage
                : TypeAdvantage.Disadvantage;
        }
    }
}
