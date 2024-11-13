using Poke.Clases;

namespace Poke.Clases;

/// <summary>
/// Representa un Pokémon con sus atributos y capacidades de ataque.
/// </summary>
public class Pokemon
{
    /// <summary>Nombre del Pokémon.</summary>
    public string Name { get; set; }

    /// <summary>Capacidad de ataque del Pokémon, inicialmente en 1 si puede atacar.</summary>
    public double AttackCapacity { get; set; }

    /// <summary>Estado actual del Pokémon (dormido, paralizado, etc.), nulo si no tiene estado.</summary>
    public string? State { get; set; }

    /// <summary>Puntos de vida del Pokémon.</summary>
    public double Hp { get; set; }

    /// <summary>Tipo de Pokémon según la clase Type.</summary>
    public Type.PokemonType Type { get; set; }

    /// <summary>Lista de ataques disponibles para el Pokémon, máximo de 4 ataques.</summary>
    public List<Attack> AttackList { get; set; }

    /// <summary>Cantidad de turnos que el Pokémon estará dormido; nulo si no está dormido.</summary>
    public double? SleepState { get; set; }

    /// <summary>Indica si el Pokémon está paralizado.</summary>
    public bool Paralized { get; set; }

    /// <summary>Indica si el Pokémon está envenenado.</summary>
    public bool Poisoned { get; set; }

    /// <summary>Indica si el Pokémon está quemado.</summary>
    public bool Burned { get; set; }

    /// <summary>
    /// Constructor para inicializar un Pokémon con su nombre, puntos de vida, capacidad de ataque, estado y tipo.
    /// </summary>
    /// <param name="name">Nombre del Pokémon.</param>
    /// <param name="health">Puntos de vida del Pokémon.</param>
    /// <param name="AttackCapacity">Capacidad de ataque del Pokémon.</param>
    /// <param name="state">Estado inicial del Pokémon.</param>
    /// <param name="type">Tipo del Pokémon.</param>
    public Pokemon(string name, double health, double AttackCapacity, string state, Type.PokemonType type, List<Attack>? attackList = null)
    {
        this.Name = name;
        this.Hp = health;
        this.State = state;
        this.AttackCapacity = 1;
        this.Type = type;
        if (attackList == null)
        {
            this.AttackList = new List<Attack>();
        }
    }

    /// <summary>
    /// Verifica si el Pokémon está vivo.
    /// </summary>
    /// <returns>Mensaje indicando si el Pokémon está vivo o muerto.</returns>
    public string IsAlive()
    {
        if (Hp > 0)
        {
            return "El Pokemon está vivo";
        }
        else
        {
            return "El Pokemon está muerto";
        }
    }

    /// <summary>
    /// Realiza un ataque contra otro Pokémon si tiene la capacidad para hacerlo.
    /// </summary>
    /// <param name="opponentPokemon">El Pokémon oponente a atacar.</param>
    /// <param name="playerPokemon">El Pokémon atacante.</param>
    /// <param name="attack">El ataque específico a usar.</param>
    public void Attack(Pokemon opponentPokemon, Pokemon playerPokemon, Attack attack)
    {
        if (AttackCapacity == 1)
        {
            double attackDamage = attack.Damage;
            opponentPokemon.RecibeDamage(attackDamage);
        }
        else
        {
            Console.WriteLine($"{Name} no puede atacar en este turno ya que está {this.State}");
        }
    }

    /// <summary>
    /// Añade un ataque a la lista de ataques del Pokémon.
    /// </summary>
    /// <param name="nuevoAttack">El ataque a añadir.</param>
    public void AddAttack(Attack nuevoAttack)
    {
        if (AttackList.Count < 4)
        {
            AttackList.Add(nuevoAttack);
        }
        else
        {
            Console.WriteLine("No se pueden agregar más ataques, el límite es 4");
        }
    }

    /// <summary>
    /// Aplica daño al Pokémon, reduciendo sus puntos de vida.
    /// </summary>
    /// <param name="damage">Cantidad de daño recibido.</param>
    public void RecibeDamage(double damage)
    {
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
        }
        Console.WriteLine($"{this.Name} recibio {damage} puntos de daño. El HP restante:{Hp}");
    }

    /// <summary>
    /// Añade puntos de vida al Pokémon.
    /// </summary>
    /// <param name="hp">Cantidad de vida a añadir.</param>
    public void AddHP(double hp)
    {
        Hp += hp;
        Console.WriteLine($"{this.Name} recuperó {hp} puntos de vida.");
    }

    /// <summary>
    /// Obtiene la lista de ataques del Pokémon.
    /// </summary>
    /// <returns>Lista de ataques del Pokémon.</returns>
    public List<Attack> GetAttacks()
    {
        return AttackList;
    }

    /// <summary>
    /// Obtiene el tipo del Pokémon.
    /// </summary>
    /// <returns>Tipo del Pokémon.</returns>
    public Type.PokemonType GetType()
    {
        return Type;
    }

    /// <summary>
    /// Obtiene los puntos de vida del Pokémon.
    /// </summary>
    /// <returns>Puntos de vida del Pokémon.</returns>
    public double GetHp()
    {
        return Hp;
    }

    /// <summary>
    /// Aplica un estado al Pokémon objetivo si no tiene ya un estado aplicado.
    /// </summary>
    /// <param name="objective">El Pokémon al que se le aplica el estado.</param>
    /// <param name="state">El estado a aplicar.</param>
    public void ApplyState(Pokemon objective, string state)
    {
        if (objective.State == null)
        {
            objective.State = state;
        }
        else
        {
            Console.WriteLine($"{objective.Name} ya tiene un estado");
        }
    }

    /// <summary>
    /// Cura totalmente al Pokémon quitándole su estado cuando se usa el ítem "CuraTotal".
    /// </summary>
    /// <param name="objective">El Pokémon objetivo.</param>
    /// <param name="item">El ítem usado.</param>
    /// <param name="player">El entrenador que posee el ítem.</param>
    public void TotalCureWithItem(Pokemon objective, Items item, OriginalTrainer player)
    {
        if (item is TotalCure && (player.GetItem(item) == true))
        {
            objective.State = null;
            player.useItem(item, objective);
            player.RemoveItem(item);
        }
        else
        {
            Console.WriteLine("No se puede usar este item para quitarle el estado al Pokemon");
        }
    }

    /// <summary>
    /// Actualiza los efectos de los estados del Pokémon en cada turno.
    /// </summary>
    public void StateActualization()
    {
        if (SleepState.HasValue && SleepState > 0)
        {
            SleepState--;
            if (SleepState == 0)
            {
                SleepState = null;
            }
        }

        if (Poisoned)
        {
            RecibeDamage(Hp * 0.05);  // Pierde 5% del HP total si está envenenado
        }
        if (Burned)
        {
            RecibeDamage(Hp * 0.10);  // Pierde 10% del HP total si está quemado
        }

        if (Paralized)
        {
            Random random = new Random();
            double AttackCapacity = random.Next(0, 2); // 0 o 1 definen si puede atacar
        }
    }
}
