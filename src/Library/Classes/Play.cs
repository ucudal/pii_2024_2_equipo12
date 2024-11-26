namespace Poke.Clases;

/// <summary>
/// Clase que gestiona las posibles jugadas de un entrenador durante un turno.
/// </summary>
public class Play
{
    /// <summary>
    /// Permite al entrenador elegir entre atacar, cambiar de Pokémon o usar un ítem.
    /// </summary>
    /// <param name="player1">El entrenador que realiza la jugada.</param>
    /// <param name="player2">El entrenador oponente.</param>
    /// <param name="item">El ítem que el jugador puede usar.</param>
    /// <param name="objective">El Pokémon objetivo del ítem.</param>
    public void PossiblePlays(
        OriginalTrainer player1, 
        OriginalTrainer player2, 
        Item item, 
        Pokemon objective, 
        string? playsSecuence = null, //Para usar en los test de esta entrega 
        string? attackIndex = null)   //Para usar en los test de esta entrega
    {
        if (playsSecuence != null)
        {
            foreach (var i in playsSecuence)
            {
                if (i == '1')
                {
                    // Obtener el ataque seleccionado de la lista
                    Attack selectedAttack = player1.ActualPokemon.AttackList[0];

                    // Realizar el ataque con el ataque seleccionado
                    player1.ActualPokemon.Attack(null, player2.ActualPokemon, player1.ActualPokemon, selectedAttack);
             
                }
                else if (i == '2')
                {
                    // Cambiar al Pokémon seleccionado por el jugador
                    Console.WriteLine("Selecciona un Pokemon:");
                    for (int u = 0; u < player1.Pokemons.Count; u++)
                    {
                        Console.WriteLine($"{u + 1}. {player1.Pokemons[u].Name}");
                    }
                    string newPokemon = Console.ReadLine();
                    Pokemon selectedPokemon = player1.Pokemons[int.Parse(newPokemon) - 1];
                    player1.ActualPokemon = selectedPokemon;
                }
                else if (i == '3')
                {
                    // Usar un ítem en el Pokémon objetivo
                    item.Use(objective);
                }
                else
                {
                    // Mensaje de opción inválida
                    Console.WriteLine("Eleccion invalida. Turno perdido.");
                }
            }
        } else {
            Console.WriteLine("1. Atacar \n 2. Cambiar de Pokemon \n 3. Usar Item");
            string playElection = Console.ReadLine();

            if (playElection == "1")
            {
                // Mostrar los ataques disponibles
                Console.WriteLine("Selecciona un ataque:");
                for (int i = 0; i < player1.ActualPokemon.AttackList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {player1.ActualPokemon.AttackList[i].Name}");
                }

                // Leer la elección del usuario y validar la opción
                if (int.TryParse(Console.ReadLine(), out int selectedAttackIndex) &&
                    selectedAttackIndex > 0 &&
                    selectedAttackIndex <= player1.ActualPokemon.AttackList.Count)
                {
                    // Obtener el ataque seleccionado de la lista
                    Attack selectedAttack = player1.ActualPokemon.AttackList[selectedAttackIndex - 1];

                    // Realizar el ataque con el ataque seleccionado
                    player1.ActualPokemon.Attack(null, player2.ActualPokemon, player1.ActualPokemon, selectedAttack);
                }
            }
            else if (playElection == "2")
            {
                // Cambiar al Pokémon seleccionado por el jugador
                Console.WriteLine("Selecciona un Pokemon:");
                for (int i = 0; i < player1.Pokemons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {player1.Pokemons[i].Name}");
                }
                string newPokemon = Console.ReadLine();
                Pokemon selectedPokemon = player1.Pokemons[int.Parse(newPokemon) - 1];
                player1.ActualPokemon = selectedPokemon;
            }
            else if (playElection == "3")
            {
                // Usar un ítem en el Pokémon objetivo
                item.Use(objective);
            }
            else
            {
                // Mensaje de opción inválida
                Console.WriteLine("Eleccion invalida. Turno perdido.");
            }
        }
    }
}
