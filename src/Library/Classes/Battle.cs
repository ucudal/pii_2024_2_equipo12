    namespace Poke.Clases;

    /// <summary>
    /// Representa una batalla entre dos entrenadores Pokémon, gestionando turnos, jugadas, y verificando el final del juego.
    /// </summary>
    public class Battle
    {
        /// <summary>
        /// Instancia para gestionar las jugadas posibles en cada turno.
        /// </summary>
        public Plays plays;

        /// <summary>
        /// Representa el turno actual en la batalla, alternando entre los jugadores.
        /// </summary>
        public double Turn;

        /// <summary>
        /// Pokemon del jugador principal.
        /// </summary>
        public Pokemon PlayerPokemon { get; set; }

        /// <summary>
        /// Pokemon del oponente.
        /// </summary>
        public Pokemon OpponentPokemon { get; set; }

        /// <summary>
        /// Contador del turno actual.
        /// </summary>
        public double ActualTurn { get; set; }

        /// <summary>
        /// Instancia de WaitList para gestionar los entrenadores en espera.
        /// </summary>
        public WaitList waitList;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Battle"/>.
        /// </summary>
        /// <param name="playerPokemon">El pokemon del jugador.</param>
        /// <param name="opponentPokemon">El pokemon oponente.</param>
        public Battle(Pokemon playerPokemon, Pokemon opponentPokemon, WaitList? waitList = null)
        {
            PlayerPokemon = playerPokemon;
            OpponentPokemon = opponentPokemon;
            plays = new Plays();
            ActualTurn = 1;
            this.waitList = waitList;
        }

        /// <summary>
        /// Determina de forma aleatoria cuál jugador comienza el primer turno.
        /// </summary>
        public void InitialTurn()
        {
            Random random = new Random();
            Turn = random.Next(1, 3); // 1 para el primer jugador, 2 para el segundo jugador
        }

        /// <summary>
        /// Ejecuta un turno de la batalla, permitiendo a un jugador realizar una jugada y luego alterna el turno al otro jugador.
        /// </summary>
        /// <param name="player1">El primer entrenador.</param>
        /// <param name="player2">El segundo entrenador.</param>
        public void PlayTurn(OriginalTrainer player1, OriginalTrainer player2, string? playsSecuence = null, string? attackIndex = null)
        {
            if (Turn == 1)
            {
                Console.WriteLine("Turno del jugador 1");
                plays.PossiblePlays(player1, player2, player1.Items[0], player2.ActualPokemon, playsSecuence, attackIndex);
                Turn = 2; // Cambia de turno para que vaya el otro jugador
            }
            else
            {
                Console.WriteLine("Turno del jugador 2");
                plays.PossiblePlays(player2, player1, player2.Items[0], player1.ActualPokemon, playsSecuence, attackIndex);
                Turn = 1; // Cambia de turno para el jugador 1
            }

            ActualTurn += 1;
        }

        /// <summary>
        /// Verifica si la batalla ha terminado, es decir, si uno de los jugadores ha ganado.
        /// </summary>
        /// <param name="player1">El primer entrenador.</param>
        /// <param name="player2">El segundo entrenador.</param>
        /// <returns>True si uno de los jugadores ha ganado; de lo contrario, False.</returns>
        public bool BattleFinished(OriginalTrainer player1, OriginalTrainer player2)
        {
            if (player1.PokemonLife() == 0)
            {
                Console.WriteLine("El jugador 2 ha ganado");
                return true;
            }
            else if (player2.PokemonLife() == 0)
            {
                Console.WriteLine("El jugador 1 ha ganado");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Muestra el estado actual de los Pokémon de ambos jugadores, incluyendo el turno actual.
        /// </summary>
        /// <param name="player1">El primer entrenador.</param>
        /// <param name="player2">El segundo entrenador.</param>
        private void InfoTurn(OriginalTrainer player1, OriginalTrainer player2)
        {
            Console.WriteLine("Turno: " + ActualTurn);
            Console.WriteLine($"Información de los pokemones totales del juego:");
            player1.GetPokemonsInfo();
            player2.GetPokemonsInfo();
        }

        /// <summary>
        /// Ejecuta la batalla completa entre dos pokemon, alternando turnos hasta que uno de los jugadores gane.
        /// </summary>
        /// <param name="player1">El primer entrenador.</param>
        /// <param name="player2">El segundo entrenador.</param>
        public void CompleteBattle(OriginalTrainer player1, OriginalTrainer player2, string? playsSecuence = null, string? attackIndex = null)
        {
            // Verifica si hay suficientes entrenadores en la lista de espera para iniciar una batalla
            if (waitList.HasPlayers())
            {
                var playersToPlay = waitList.CheckIn();
                if (playersToPlay.Count == 2)
                {
                    Console.WriteLine("Comienza la batalla entre los entrenadores en la lista de espera!");

                    player1 = playersToPlay[0];
                    player2 = playersToPlay[1];

                    // Inicializa el turno y ejecuta la batalla entre los dos jugadores
                    InitialTurn();
                    while (!BattleFinished(player1, player2))
                    {
                        InfoTurn(player1, player2);
                        PlayTurn(player1, player2, playsSecuence, attackIndex);
                    }

                    Console.WriteLine("La batalla ha terminado.");
                    // Verificar nuevamente la lista de espera después de la batalla
                    CompleteBattle(player1, player2);
                }
            }
            else
            {
                Console.WriteLine("No hay suficientes entrenadores en la lista de espera para iniciar una batalla.");
            }
        } 
    }
