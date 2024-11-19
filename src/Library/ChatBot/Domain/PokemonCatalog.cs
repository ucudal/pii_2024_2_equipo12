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
        List<Attack> attacks = PokemonAttacks[catalogEntry];
        switch (catalogEntry)
        {
            case Catalog.Bulbasaur:
                return new Pokemon("Bulbasaur", 200, 1, null, Poke.Clases.Type.PokemonType.Plant, attacks);
            case Catalog.Charmander:
                return new Pokemon("Charmander", 200, 1, null, Poke.Clases.Type.PokemonType.Fire, attacks);
            case Catalog.Squirtle:
                return new Pokemon("Squirtle", 200, 1, null, Poke.Clases.Type.PokemonType.Water, attacks);
            case Catalog.Pikachu:
                return new Pokemon("Pikachu", 200, 1, null, Poke.Clases.Type.PokemonType.Electric, attacks);
            case Catalog.Jigglypuff:
                return new Pokemon("Jigglypuff", 200, 1, null, Poke.Clases.Type.PokemonType.Psychic, attacks);
            case Catalog.Eevee:
                return new Pokemon("Eevee", 190, 1, null, Poke.Clases.Type.PokemonType.Normal, attacks);
            case Catalog.Psyduck:
                return new Pokemon("Psyduck", 210, 1, null, Poke.Clases.Type.PokemonType.Water, attacks);
            case Catalog.Geodude:
                return new Pokemon("Geodude", 220, 1, null, Poke.Clases.Type.PokemonType.Rock, attacks);
            case Catalog.Machop:
                return new Pokemon("Machop", 230, 1, null, Poke.Clases.Type.PokemonType.Fighter, attacks);
            case Catalog.Gastly:
                return new Pokemon("Gastly", 190, 1, null, Poke.Clases.Type.PokemonType.Ghost, attacks);
            case Catalog.Oddish:
                return new Pokemon("Oddish", 180, 1, null, Poke.Clases.Type.PokemonType.Plant, attacks);
            case Catalog.Growlithe:
                return new Pokemon("Growlithe", 200, 1, null, Poke.Clases.Type.PokemonType.Fire, attacks);
            default:
                throw new ArgumentException("Pokémon no encontrado en el catálogo.");
        }
    }
    
    /// <summary>
    /// Diccionario que asocia cada entrada del catálogo con una lista de ataques específicos para ese Pokémon.
    /// </summary>
    public static readonly Dictionary<Catalog, List<Attack>> PokemonAttacks = new()
    {
        {
            Catalog.Bulbasaur, new List<Attack>
            {
                new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Vine Whip", 45, Poke.Clases.Type.PokemonType.Plant, false),
                new Poisoned("Poison Powder", 0, Poke.Clases.Type.PokemonType.Poison, true)
            }
        },
        {
            Catalog.Charmander, new List<Attack>
            {
                new Attack("Scratch", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Ember", 40, Poke.Clases.Type.PokemonType.Fire, true),
                new Burned("Flamethrower", 90, Poke.Clases.Type.PokemonType.Fire, true)
            }
        },
        {
            Catalog.Squirtle, new List<Attack>
            {
                new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Water Gun", 40, Poke.Clases.Type.PokemonType.Water, true),
                new Attack("Bubble", 40, Poke.Clases.Type.PokemonType.Water, true)
            }
        },
        {
            Catalog.Pikachu, new List<Attack>
            {
                new Attack("Quick Attack", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Thunder Shock", 40, Poke.Clases.Type.PokemonType.Electric, true),
                new Paralized("Thunder Wave", 0, Poke.Clases.Type.PokemonType.Electric, true)
            }
        },
        {
            Catalog.Jigglypuff, new List<Attack>
            {
                new Attack("Pound", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new IsAsleep("Sing", 0, Poke.Clases.Type.PokemonType.Normal, true),
                new Attack("Double Slap", 15, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Body Slam", 85, Poke.Clases.Type.PokemonType.Normal, false)
            }
        },
        {
            Catalog.Eevee, new List<Attack>
            {
                new Attack("Quick Attack", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Bite", 60, Poke.Clases.Type.PokemonType.Normal, false) 
            }
        },
        {
            Catalog.Psyduck, new List<Attack>
            {
                new Attack("Water Gun", 40, Poke.Clases.Type.PokemonType.Water, true),
                new Attack("Confusion", 50, Poke.Clases.Type.PokemonType.Psychic, true)
            }
        },
        {
            Catalog.Geodude, new List<Attack>
            {
                new Attack("Tackle", 40, Poke.Clases.Type.PokemonType.Normal, false),
                new Attack("Rock Throw", 50, Poke.Clases.Type.PokemonType.Rock, false)
            }
        },
        {
            Catalog.Machop, new List<Attack>
            {
                new Attack("Karate Chop", 50, Poke.Clases.Type.PokemonType.Fighter, true),
                new Attack("Low Kick", 50, Poke.Clases.Type.PokemonType.Fighter, false)
            }
        },
        {
            Catalog.Gastly, new List<Attack>
            {
                new Attack("Lick", 30, Poke.Clases.Type.PokemonType.Ghost, false),
                new Attack("Night Shade", 50, Poke.Clases.Type.PokemonType.Ghost, true)
            }
        },
        {
            Catalog.Oddish, new List<Attack>
            {
                new Attack("Absorb", 20, Poke.Clases.Type.PokemonType.Plant, true),
                new Poisoned("Poison Powder", 0, Poke.Clases.Type.PokemonType.Poison, true)
            }
        },
        {
            Catalog.Growlithe, new List<Attack>
            {
                new Attack("Ember", 40, Poke.Clases.Type.PokemonType.Fire, true),
                new Attack("Bite", 60, Poke.Clases.Type.PokemonType.Normal, false)
            }
        }
    };
}
