using NUnit.Framework;
using Poke.Clases;
using Type = Poke.Clases.Type;

namespace LibraryTests;

[TestFixture]
public class AttackTest
{
    
    /*  Como jugador, quiero atacar en mi turno y hacer daño basado en la efectividad de los tipos de Pokémon.
        Criterios de aceptación:
        El jugador puede seleccionar un ataque que inflige daño basado en la efectividad de tipos.
        El sistema aplica automáticamente la ventaja o desventaja del tipo de ataque. */
    
    [Test]
    public void TestDamageBasedOnTypeEffectiveness()
    {
        // Crear Pokémon
        Pokemon firePokemon = new Pokemon("Charmander", 100, 1, null, Type.PokemonType.Fire);
        Pokemon waterPokemon = new Pokemon("Squirtle", 100, 1, null, Type.PokemonType.Water);
        Pokemon plantPokemon = new Pokemon("Bulbasaur", 100, 1, null, Type.PokemonType.Plant);

        // Crear ataques
        NormalAttack fireAttack = new NormalAttack("Fire Blast", 40,  Type.PokemonType.Fire, false);
        NormalAttack waterAttack = new NormalAttack("Water Gun", 40, Type.PokemonType.Water, false );
        NormalAttack plantAttack = new NormalAttack("Vine Whip", 40, Type.PokemonType.Plant, false );

        // Aplicar ventaja de tipo: Fire > Plant
        double expectedFireDamage = fireAttack.CalculateDamage(plantPokemon);
        plantPokemon.RecibeDamage(expectedFireDamage);
        Assert.That(plantPokemon.GetHp(), Is.EqualTo(20));

        // Aplicar ventaja de tipo: Water > Fire
        double expectedWaterDamage = waterAttack.CalculateDamage(firePokemon);
        firePokemon.RecibeDamage(expectedWaterDamage);
        Assert.That(firePokemon.GetHp(), Is.EqualTo(20));

        // Aplicar desventaja de tipo: Water < Plant
        double expectedPlantDamage = plantAttack.CalculateDamage(waterPokemon);
        waterPokemon.RecibeDamage(expectedPlantDamage);
        Assert.That(waterPokemon.GetHp(), Is.EqualTo(20));
    }
}
