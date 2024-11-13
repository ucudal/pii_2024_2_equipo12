using Poke.Clases;
using Type = System.Type;

namespace Ucu.Poo.DiscordBot.Domain;

public class PokemonCatalog
{
    /// <summary>
    /// Enum que define el catálogo de Pokémon.
    /// </summary>
    public enum Catalog
    {
        Bulbasaur,
        Charmander,
        Squirtle,
        Pikachu,
        Jigglypuff,
    }
    
    /// <summary>
    /// Método para crear instancias de Pokémon según el catálogo.
    /// </summary>
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
            default:
                throw new ArgumentException("Pokémon no encontrado en el catálogo.");
        }
    }
    
    /// <summary>
    /// Diccionario que asocia cada Pokémon con una lista de ataques.
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
                new IsAsleep("Sing", 0, Poke.Clases.Type.PokemonType.Normal, true)
            }
        }
    };
}
