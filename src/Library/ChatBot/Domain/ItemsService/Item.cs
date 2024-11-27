namespace Poke.Clases;

public abstract class Item
{
    public string Name { get; set; }
    public abstract void Use(Pokemon objective);
}