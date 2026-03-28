namespace ConsoleApp1.Services;

using ConsoleApp1.Models;

public class RentalService
{
    private readonly List<Rental> _rentals = new();
    private readonly PenaltyCalculator _penaltyCalculator;

    public RentalService(PenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator;
    }

    public OperationResult Borrow(User user, Equipment equipment, int rentalDays)
    {
        if (equipment.Status != EquipmentStatus.Available)
            return OperationResult.Fail($"Sprzęt \"{equipment.Name}\" nie jest dostępny (status: {equipment.Status}).");

        int activeCount = GetActiveRentals(user.Id).Count;
        if (activeCount >= user.MaxActiveRentals)
            return OperationResult.Fail(
                $"Użytkownik {user.FirstName} {user.LastName} osiągnął limit wypożyczeń ({user.MaxActiveRentals}).");

        var rental = new Rental(user, equipment, rentalDays);
        _rentals.Add(rental);
        equipment.Status = EquipmentStatus.Borrowed;

        return OperationResult.Ok(
            $"Wypożyczono \"{equipment.Name}\" dla {user.FirstName} {user.LastName}. " +
            $"Termin zwrotu: {rental.DueDate:dd.MM.yyyy}");
    }

    public OperationResult Return(int equipmentId)
    {
        var rental = _rentals.FirstOrDefault(r => r.IsActive && r.Equipment.Id == equipmentId);
        if (rental == null)
            return OperationResult.Fail($"Nie znaleziono aktywnego wypożyczenia dla sprzętu o ID {equipmentId}.");

        decimal penalty = _penaltyCalculator.Calculate(rental);
        rental.CompleteReturn(penalty);
        rental.Equipment.Status = EquipmentStatus.Available;

        string message = $"Zwrócono \"{rental.Equipment.Name}\".";
        if (penalty > 0)
            message += $" Naliczono karę: {penalty:F2} PLN za opóźnienie.";
        else
            message += " Zwrot w terminie, brak kary.";

        return OperationResult.Ok(message);
    }

    public List<Rental> GetActiveRentals(int userId)
    {
        return _rentals.Where(r => r.IsActive && r.User.Id == userId).ToList();
    }

    public List<Rental> GetOverdueRentals()
    {
        return _rentals.Where(r => r.IsOverdue).ToList();
    }

    public List<Rental> GetAllRentals()
    {
        return _rentals.ToList();
    }
}