using Poke.Clases;
using Type = System.Type;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Clase que representa un catálogo de Pokémon y proporciona funcionalidad para
/// crear instancias de Pokémon y asociarlos con sus ataques.
/// </summary>
public static class PokemonCatalog
{
    /// <summary>
    /// Enum que define el catálogo de Pokémon disponibles.
    /// </summary>
    public enum Catalog
    {
        Bulbasaur = 1,
        Charmander,
        Squirtle,
        Pikachu,
        Jigglypuff,
        Eevee,
        Psyduck,
        Geodude,
        Machop,
        Gastly,
        Oddish,
        Growlithe
    }
    
    /// <summary>
    /// Crea una instancia de un Pokémon según la entrada del catálogo.
    /// </summary>
    /// <param name="catalogEntry">La entrada del catálogo que representa el Pokémon deseado.</param>
    /// <returns>Una instancia del Pokémon especificado.</returns>
    /// <exception cref="ArgumentException">Se lanza si el Pokémon no está en el catálogo.</exception>
    public static Pokemon CreatePokemon(Catalog catalogEntry)
    {
        List<Attack> attacks = CreatAttackList(catalogEntry);
        switch (catalogEntry)
        {
            case Catalog.Bulbasaur:
                return new Pokemon("Bulbasaur", 30, 1, null, Poke.Clases.Type.PokemonType.Plant, attacks);
            case Catalog.Charmander:
                return new Pokemon("Charmander", 30, 1, null, Poke.Clases.Type.PokemonType.Fire, attacks);
            case Catalog.Squirtle:
                return new Pokemon("Squirtle", 30, 1, null, Poke.Clases.Type.PokemonType.Water, attacks);
            case Catalog.Pikachu:
                return new Pokemon("Pikachu", 30, 1, null, Poke.Clases.Type.PokemonType.Electric, attacks);
            case Catalog.Jigglypuff:
                return new Pokemon("Jigglypuff", 30, 1, null, Poke.Clases.Type.PokemonType.Psychic, attacks);
            case Catalog.Eevee:
                return new Pokemon("Eevee", 30, 1, null, Poke.Clases.Type.PokemonType.Normal, attacks);
            case Catalog.Psyduck:
                return new Pokemon("Psyduck", 30, 1, null, Poke.Clases.Type.PokemonType.Water, attacks);
            case Catalog.Geodude:
                return new Pokemon("Geodude", 30, 1, null, Poke.Clases.Type.PokemonType.Rock, attacks);
            case Catalog.Machop:
                return new Pokemon("Machop", 30, 1, null, Poke.Clases.Type.PokemonType.Fighter, attacks);
            case Catalog.Gastly:
                return new Pokemon("Gastly", 30, 1, null, Poke.Clases.Type.PokemonType.Ghost, attacks);
            case Catalog.Oddish:
                return new Pokemon("Oddish", 30, 1, null, Poke.Clases.Type.PokemonType.Plant, attacks);
            case Catalog.Growlithe:
                return new Pokemon("Growlithe", 30, 1, null, Poke.Clases.Type.PokemonType.Fire, attacks);
            default:
                throw new ArgumentException("Pokémon no encontrado en el catálogo.");
        }
    }
    
    /// <summary>
    /// Función que asocia cada entrada del catálogo con una lista de ataques específicos para ese Pokémon.
    /// </summary>
    public static List<Attack> CreatAttackList(Catalog catalogEntry)
    {
        switch (catalogEntry)
        {
            case Catalog.Bulbasaur:
                return new List<Attack> 
                {
                    new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("VineWhip", 45, Poke.Clases.Type.PokemonType.Plant, false),
                    new Poisoned("PoisonPowder", 60, Poke.Clases.Type.PokemonType.Poison, true),
                    new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true),
                };
            case Catalog.Charmander:
                return new List<Attack>
                {
                    new Attack("Scratch", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("Ember", 40, Poke.Clases.Type.PokemonType.Fire, false),
                    new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true),
                    new Paralized("ThunderWave", 100, Poke.Clases.Type.PokemonType.Electric, true),
                };
            case Catalog.Squirtle:
                return new List<Attack>
                {
                    new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("WaterGun", 35, Poke.Clases.Type.PokemonType.Water, false),
                    new Paralized("Bubble", 60, Poke.Clases.Type.PokemonType.Water, true),
                    new IsAsleep("Sing", 100, Poke.Clases.Type.PokemonType.Normal, true),
                };
            case Catalog.Pikachu:
                return new List<Attack>
                {
                    new Attack("QuickAttack", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("ThunderShock", 40, Poke.Clases.Type.PokemonType.Electric, false),
                    new Paralized("ThunderWave", 65, Poke.Clases.Type.PokemonType.Electric, true),
                    new IsAsleep("Thunder", 100, Poke.Clases.Type.PokemonType.Electric, true)
                };
            case Catalog.Jigglypuff:
                return new List<Attack>
                {
                    new Attack("Pound", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("Sing", 0, Poke.Clases.Type.PokemonType.Normal, false),
                    new Burned("DoubleSlap", 65, Poke.Clases.Type.PokemonType.Normal, true),
                    new Poisoned("BodySlam", 85, Poke.Clases.Type.PokemonType.Normal, true),
                };
            case Catalog.Eevee:
                return new List<Attack>
                {
                    new Attack("QuickAttack", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("Bite", 60, Poke.Clases.Type.PokemonType.Normal, false),
                    new Burned("Flamethrower", 70, Poke.Clases.Type.PokemonType.Fire, true),
                    new IsAsleep("Swift", 100, Poke.Clases.Type.PokemonType.Normal, true),
                };
            case Catalog.Psyduck:
                return new List<Attack>
                {
                    new Attack("WaterGun", 40, Poke.Clases.Type.PokemonType.Water, false),
                    new Attack("Confusion", 50, Poke.Clases.Type.PokemonType.Psychic, false),
                    new IsAsleep("Disable", 30, Poke.Clases.Type.PokemonType.Psychic, true),
                    new Paralized("HydroPump", 100, Poke.Clases.Type.PokemonType.Water, true),
                };
            case Catalog.Geodude:
                return new List<Attack>
                {
                    new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false),
                    new Attack("RockThrow", 50, Poke.Clases.Type.PokemonType.Rock, false),
                    new Burned("Magnitude", 80, Poke.Clases.Type.PokemonType.Rock, true),
                    new Poisoned("Earthquake", 100, Poke.Clases.Type.PokemonType.Psychic, true),
                };
            case Catalog.Machop:
                return new List<Attack>
                {
                    new Attack("KarateChop", 50, Poke.Clases.Type.PokemonType.Fighter, false),
                    new Attack("LowKick", 50, Poke.Clases.Type.PokemonType.Fighter, false),
                    new IsAsleep("SeismicToss", 80, Poke.Clases.Type.PokemonType.Fighter, true),
                    new Burned("FirePunch", 100, Poke.Clases.Type.PokemonType.Fire, true),
                    
                };
            case Catalog.Gastly:
                return new List<Attack>
                {
                    new Attack("Lick", 30, Poke.Clases.Type.PokemonType.Ghost, false),
                    new Attack("NightShade", 50, Poke.Clases.Type.PokemonType.Ghost, false),
                    new IsAsleep("DreamEater", 75, Poke.Clases.Type.PokemonType.Psychic, true),
                    new Burned("ShadowBall", 100, Poke.Clases.Type.PokemonType.Ghost, true),
                };
            case Catalog.Oddish:
                return new List<Attack>
                {
                    new Attack("Absorb", 20, Poke.Clases.Type.PokemonType.Plant, false),
                    new Attack("PoisonPowder", 35, Poke.Clases.Type.PokemonType.Poison, false),
                    new Paralized("Acid", 40, Poke.Clases.Type.PokemonType.Poison, true),
                    new IsAsleep("SolarBeam", 100, Poke.Clases.Type.PokemonType.Plant, true),
                };
            case Catalog.Growlithe:
                return new List<Attack>
                {
                    new Attack("Ember", 40, Poke.Clases.Type.PokemonType.Fire, false),
                    new Attack("Bite", 60, Poke.Clases.Type.PokemonType.Normal, false),
                    new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true),
                    new Poisoned("FireBlast", 100, Poke.Clases.Type.PokemonType.Fire, true),
                };
            default:
                throw new ArgumentException("Pokémon no encontrado en el catálogo.");
        }
    }
}
