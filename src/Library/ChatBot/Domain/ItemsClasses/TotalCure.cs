namespace Poke.Clases;

public class TotalCure : Item
{
    public TotalCure()
    {
        Name = "Cura Total";
    }

    public override void Use(Pokemon objective)
    {
        // Lógica para eliminar efectos de state (dormido, paralizado, etc.)
    }
}