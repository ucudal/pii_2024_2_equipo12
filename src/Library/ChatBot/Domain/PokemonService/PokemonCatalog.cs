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
                return new Pokemon("Bulbasaur", 200, 1, "Normal", Poke.Clases.Type.PokemonType.Plant, attacks);
            case Catalog.Charmander:
                return new Pokemon("Charmander", 200, 1, "Normal", Poke.Clases.Type.PokemonType.Fire, attacks);
            case Catalog.Squirtle:
                return new Pokemon("Squirtle", 200, 1, "Normal", Poke.Clases.Type.PokemonType.Water, attacks);
            case Catalog.Pikachu:
                return new Pokemon("Pikachu", 200, 1, "Normal", Poke.Clases.Type.PokemonType.Electric, attacks);
            case Catalog.Jigglypuff:
                return new Pokemon("Jigglypuff", 200, 1, "Normal", Poke.Clases.Type.PokemonType.Psychic, attacks);
            case Catalog.Eevee:
                return new Pokemon("Eevee", 190, 1, "Normal", Poke.Clases.Type.PokemonType.Normal, attacks);
            case Catalog.Psyduck:
                return new Pokemon("Psyduck", 210, 1, "Normal", Poke.Clases.Type.PokemonType.Water, attacks);
            case Catalog.Geodude:
                return new Pokemon("Geodude", 220, 1, "Normal", Poke.Clases.Type.PokemonType.Rock, attacks);
            case Catalog.Machop:
                return new Pokemon("Machop", 230, 1, "Normal", Poke.Clases.Type.PokemonType.Fighter, attacks);
            case Catalog.Gastly:
                return new Pokemon("Gastly", 190, 1, "Normal", Poke.Clases.Type.PokemonType.Ghost, attacks);
            case Catalog.Oddish:
                return new Pokemon("Oddish", 180, 1, "Normal", Poke.Clases.Type.PokemonType.Plant, attacks);
            case Catalog.Growlithe:
                return new Pokemon("Growlithe", 200, 1, "Normal", Poke.Clases.Type.PokemonType.Fire, attacks);
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
                    new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false, null),
                    new Attack("VineWhip", 45, Poke.Clases.Type.PokemonType.Plant, false, null),
                    new Poisoned("PoisonPowder", 60, Poke.Clases.Type.PokemonType.Poison, true, "Poisoned"),
                    new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true, "Burned"),
                };
            case Catalog.Charmander:
                return new List<Attack>
                {
                    new Attack("Scratch", 40, Poke.Clases.Type.PokemonType.Normal, false, null),
                    new Attack("Ember", 40, Poke.Clases.Type.PokemonType.Fire, false, null),
                    new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true, "Burned"),
                    new Paralized("ThunderWave", 100, Poke.Clases.Type.PokemonType.Electric, true, "Paralized"),
                };
            case Catalog.Squirtle:
                return new List<Attack>
                {
                    new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false, null),
                    new Attack("WaterGun", 35, Poke.Clases.Type.PokemonType.Water, false, null),
                    new Paralized("Bubble", 60, Poke.Clases.Type.PokemonType.Water, true, "Paralized"),
                    new Asleep("Sing", 100, Poke.Clases.Type.PokemonType.Normal, true, "Asleep"),
                };
            case Catalog.Pikachu:
                return new List<Attack>
                {
                    new Attack("QuickAttack", 40, Poke.Clases.Type.PokemonType.Normal, false, null),
                    new Attack("ThunderShock", 40, Poke.Clases.Type.PokemonType.Electric, false,null),
                    new Paralized("ThunderWave", 65, Poke.Clases.Type.PokemonType.Electric, true,"Paralized"),
                    new Asleep("Thunder", 100, Poke.Clases.Type.PokemonType.Electric, true,"Asleep")
                };
            case Catalog.Jigglypuff:
                return new List<Attack>
                {
                    new Attack("Pound", 40, Poke.Clases.Type.PokemonType.Normal, false,null),
                    new Attack("Sing", 0, Poke.Clases.Type.PokemonType.Normal, false,null),
                    new Burned("DoubleSlap", 65, Poke.Clases.Type.PokemonType.Normal, true,"Burned"),
                    new Poisoned("BodySlam", 85, Poke.Clases.Type.PokemonType.Normal, true,"Poisoned"),
                };
            case Catalog.Eevee:
                return new List<Attack>
                {
                    new Attack("QuickAttack", 40, Poke.Clases.Type.PokemonType.Normal, false,null),
                    new Attack("Bite", 60, Poke.Clases.Type.PokemonType.Normal, false,null),
                    new Burned("Flamethrower", 70, Poke.Clases.Type.PokemonType.Fire, true,"Burned"),
                    new Asleep("Swift", 100, Poke.Clases.Type.PokemonType.Normal, true,"Asleep"),
                };
            case Catalog.Psyduck:
                return new List<Attack>
                {
                    new Attack("WaterGun", 40, Poke.Clases.Type.PokemonType.Water, false,null),
                    new Attack("Confusion", 50, Poke.Clases.Type.PokemonType.Psychic, false,null),
                    new Asleep("Disable", 30, Poke.Clases.Type.PokemonType.Psychic, true,"Asleep"),
                    new Paralized("HydroPump", 100, Poke.Clases.Type.PokemonType.Water, true,"Paralized"),
                };
            case Catalog.Geodude:
                return new List<Attack>
                {
                    new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false,null),
                    new Attack("RockThrow", 50, Poke.Clases.Type.PokemonType.Rock, false,null),
                    new Burned("Magnitude", 80, Poke.Clases.Type.PokemonType.Rock, true,"Burned"),
                    new Poisoned("Earthquake", 100, Poke.Clases.Type.PokemonType.Psychic, true,"Poisoned"),
                };
            case Catalog.Machop:
                return new List<Attack>
                {
                    new Attack("KarateChop", 50, Poke.Clases.Type.PokemonType.Fighter, false,null),
                    new Attack("LowKick", 50, Poke.Clases.Type.PokemonType.Fighter, false,null),
                    new Asleep("SeismicToss", 80, Poke.Clases.Type.PokemonType.Fighter, true,"Asleep"),
                    new Burned("FirePunch", 100, Poke.Clases.Type.PokemonType.Fire, true,"Burned"),
                    
                };
            case Catalog.Gastly:
                return new List<Attack>
                {
                    new Attack("Lick", 30, Poke.Clases.Type.PokemonType.Ghost, false,null),
                    new Attack("NightShade", 50, Poke.Clases.Type.PokemonType.Ghost, false,null),
                    new Asleep("DreamEater", 75, Poke.Clases.Type.PokemonType.Psychic, true,"Asleep"),
                    new Burned("ShadowBall", 100, Poke.Clases.Type.PokemonType.Ghost, true,"Burned"),
                };
            case Catalog.Oddish:
                return new List<Attack>
                {
                    new Attack("Absorb", 20, Poke.Clases.Type.PokemonType.Plant, false,null),
                    new Attack("PoisonPowder", 35, Poke.Clases.Type.PokemonType.Poison, false,null),
                    new Paralized("Acid", 40, Poke.Clases.Type.PokemonType.Poison, true,"Paralized"),
                    new Asleep("SolarBeam", 100, Poke.Clases.Type.PokemonType.Plant, true,"Asleep"),
                };
            case Catalog.Growlithe:
                return new List<Attack>
                {
                    new Attack("Ember", 40, Poke.Clases.Type.PokemonType.Fire, false,null),
                    new Attack("Bite", 60, Poke.Clases.Type.PokemonType.Normal, false,null),
                    new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true,"Burned"),
                    new Poisoned("FireBlast", 100, Poke.Clases.Type.PokemonType.Fire, true,"Poisoned"),
                };
            default:
                throw new ArgumentException("Pokémon no encontrado en el catálogo.");
        }
    }
}
