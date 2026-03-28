namespace ConsoleApp1.Models;

public class Camera : Equipment
{
    public double MegaPixels { get; set; }
    public bool HasStabilization { get; set; }

    public Camera(string name, double megaPixels, bool hasStabilization) : base(name)
    {
        MegaPixels = megaPixels;
        HasStabilization = hasStabilization;
    }

    public override string GetDetails()
    {
        string stabilization = HasStabilization ? "Tak" : "Nie";
        return $"Rozdzielczość: {MegaPixels}MP, Stabilizacja: {stabilization}";
    }
}