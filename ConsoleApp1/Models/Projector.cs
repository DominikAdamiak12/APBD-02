namespace ConsoleApp1.Models;

public class Projector : Equipment
{
    public int LumensCount { get; set; }
    public string Resolution { get; set; }

    public Projector(string name, int lumensCount, string resolution) : base(name)
    {
        LumensCount = lumensCount;
        Resolution = resolution;
    }

    public override string GetDetails()
    {
        return $"Jasność: {LumensCount} lm, Rozdzielczość: {Resolution}";
    }
}