namespace ConsoleApp1.Services;

using ConsoleApp1.Models;

public class ReportService
{
    private readonly EquipmentService _equipmentService;
    private readonly RentalService _rentalService;
    private readonly UserService _userService;

    public ReportService(EquipmentService equipmentService, RentalService rentalService, UserService userService)
    {
        _equipmentService = equipmentService;
        _rentalService = rentalService;
        _userService = userService;
    }

    public string GenerateSummary()
    {
        var allEquipment = _equipmentService.GetAll();
        var allRentals = _rentalService.GetAllRentals();
        var overdueRentals = _rentalService.GetOverdueRentals();
        var users = _userService.GetAll();

        int available = allEquipment.Count(e => e.Status == EquipmentStatus.Available);
        int borrowed = allEquipment.Count(e => e.Status == EquipmentStatus.Borrowed);
        int unavailable = allEquipment.Count(e => e.Status == EquipmentStatus.Unavailable);
        int activeRentals = allRentals.Count(r => r.IsActive);
        int completedRentals = allRentals.Count(r => !r.IsActive);
        decimal totalPenalties = allRentals.Sum(r => r.Penalty);

        return $"""
                -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                         RAPORT WYPOŻYCZALNI SPRZĘTU
                -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                
                SPRZĘT:
                  Łącznie:        {allEquipment.Count}
                  Dostępny:       {available}
                  Wypożyczony:    {borrowed}
                  Niedostępny:    {unavailable}

                UŻYTKOWNICY:
                  Łącznie:        {users.Count}

                WYPOŻYCZENIA:
                  Aktywne:        {activeRentals}
                  Zakończone:     {completedRentals}
                  Przeterminowane:{overdueRentals.Count}

                KARY:
                  Suma naliczonych kar: {totalPenalties:F2} PLN
                -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                """;
    }
}