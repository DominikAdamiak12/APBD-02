namespace ConsoleApp1.Services;

using ConsoleApp1.Models;

public class EquipmentService
{
    private readonly List<Equipment> _equipment = new();

    public void Add(Equipment equipment)
    {
        _equipment.Add(equipment);
    }

    public Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(e => e.Id == id);
    }

    public List<Equipment> GetAll()
    {
        return _equipment.ToList();
    }

    public List<Equipment> GetAvailable()
    {
        return _equipment.Where(e => e.Status == EquipmentStatus.Available).ToList();
    }

    public OperationResult MarkAsUnavailable(int equipmentId)
    {
        var equipment = GetById(equipmentId);
        if (equipment == null)
            return OperationResult.Fail($"Nie znaleziono sprzętu o ID {equipmentId}.");

        if (equipment.Status == EquipmentStatus.Borrowed)
            return OperationResult.Fail($"Sprzęt \"{equipment.Name}\" jest aktualnie wypożyczony. Nie można oznaczyć jako niedostępny.");

        equipment.Status = EquipmentStatus.Unavailable;
        return OperationResult.Ok($"Sprzęt \"{equipment.Name}\" oznaczony jako niedostępny.");
    }
}