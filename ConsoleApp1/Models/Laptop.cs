namespace ConsoleApp1.Models;

public class Laptop : Equipment
{
    public int RamGB { get; set; }
    public double ScreenSizeInch { get; set; }

    public Laptop(string name, int ramGB, double screenSizeInch) : base(name)
    {
        RamGB = ramGB;
        ScreenSizeInch = screenSizeInch;
    }

    public override string GetDetails()
    {
        return $"RAM: {RamGB}GB, Ekran: {ScreenSizeInch}\"";
    }
}