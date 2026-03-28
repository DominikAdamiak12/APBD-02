namespace ConsoleApp1.Models;

public enum EquipmentStatus
{
    Available,
    Borrowed,
    Unavailable
}

public abstract class Equipment
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Name { get; set; }
    public EquipmentStatus Status { get; set; }
    public DateTime AddedDate { get; }

    protected Equipment(string name)
    {
        Id = _nextId++;
        Name = name;
        Status = EquipmentStatus.Available;
        AddedDate = DateTime.Now;
    }

    public abstract string GetDetails();

    public override string ToString()
    {
        return $"[{Id}] {Name} ({GetType().Name}) - {Status}";
    }
}