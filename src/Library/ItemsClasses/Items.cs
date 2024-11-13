namespace Poke.Clases;

public abstract class Items
{
    public string Name { get; set; }
    public abstract void Use(Pokemon objective);
}