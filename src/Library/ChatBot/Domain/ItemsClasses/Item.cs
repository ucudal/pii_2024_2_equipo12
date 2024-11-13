namespace Poke.Clases;

public abstract class Item
{
    public string Name { get; set; }
    public abstract void Use(Pokemon objective);
    
    public bool FindItemByName(string name)
    {
        if (this.Name == name)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}