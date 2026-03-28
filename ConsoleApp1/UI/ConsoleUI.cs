namespace ConsoleApp1.UI;

using ConsoleApp1.Models;
using ConsoleApp1.Services;

public class ConsoleUI
{
    private readonly EquipmentService _equipmentService;
    private readonly UserService _userService;
    private readonly RentalService _rentalService;
    private readonly ReportService _reportService;

    public ConsoleUI(
        EquipmentService equipmentService,
        UserService userService,
        RentalService rentalService,
        ReportService reportService)
    {
        _equipmentService = equipmentService;
        _userService = userService;
        _rentalService = rentalService;
        _reportService = reportService;
    }

    public void RunDemo()
    {
        PrintHeader("SCENARIUSZ DEMONSTRACYJNY");

        // -=- 1. Utworzenie i dodanie sprzętu sprzętu -=-
        PrintStep("1. Dodawanie sprzętu różnych typów");

        var laptop1 = new Laptop("Dell XPS 15", 16, 15.6);
        var laptop2 = new Laptop("MacBook Pro 14", 32, 14.2);
        var projector1 = new Projector("Epson EB-X51", 3800, "1024x768");
        var projector2 = new Projector("BenQ TH671ST", 3000, "1920x1080");
        var camera1 = new Camera("Canon EOS R6", 20.1, true);
        var camera2 = new Camera("Sony A7 III", 24.2, false);

        _equipmentService.Add(laptop1);
        _equipmentService.Add(laptop2);
        _equipmentService.Add(projector1);
        _equipmentService.Add(projector2);
        _equipmentService.Add(camera1);
        _equipmentService.Add(camera2);

        Console.WriteLine("Dodano 6 egzemplarzy sprzętu.\n");

        // -=- 2. Utworzenie i dodanie użytkowników -=-
        PrintStep("2. Dodawanie użytkowników");

        var student1 = new Student("Jan", "Kowalski");
        var student2 = new Student("Anna", "Nowak");
        var employee1 = new Employee("Piotr", "Wiśniewski");

        _userService.Add(student1);
        _userService.Add(student2);
        _userService.Add(employee1);

        foreach (var user in _userService.GetAll())
            Console.WriteLine($"  {user}");
        Console.WriteLine();

        // -=- 3. Wyświetlenie listy całego sprzętu -=-
        PrintStep("3. Lista całego sprzętu z aktualnym statusem");
        PrintEquipmentList(_equipmentService.GetAll());

        // -=- 4. Poprawne wypożyczenia -=-
        PrintStep("4. Poprawne wypożyczenia");

        PrintResult(_rentalService.Borrow(student1, laptop1, 7));
        PrintResult(_rentalService.Borrow(student1, camera1, 14));
        PrintResult(_rentalService.Borrow(employee1, projector1, 3));

        // -=- 5. Próba niepoprawnych operacji -=-
        PrintStep("5. Próby niepoprawnych operacji");

        Console.WriteLine("  -> Próba wypożyczenia przy przekroczonym limicie (student ma 2/2):");
        PrintResult(_rentalService.Borrow(student1, projector2, 7));

        Console.WriteLine("  -> Próba wypożyczenia sprzętu już wypożyczonego:");
        PrintResult(_rentalService.Borrow(student2, laptop1, 7));

        Console.WriteLine("  -> Oznaczenie sprzętu jako niedostępny:");
        PrintResult(_equipmentService.MarkAsUnavailable(camera2.Id));

        Console.WriteLine("  -> Próba wypożyczenia sprzętu niedostępnego:");
        PrintResult(_rentalService.Borrow(student2, camera2, 7));

        // -=- 6. Wyświetlenie dostępnego sprzętu -=-
        PrintStep("6. Sprzęt dostępny do wypożyczenia");
        PrintEquipmentList(_equipmentService.GetAvailable());

        // -=- 7. Aktywne wypożyczenia użytkownika -=-
        PrintStep("7. Aktywne wypożyczenia Jana Kowalskiego");
        var jansRentals = _rentalService.GetActiveRentals(student1.Id);
        foreach (var rental in jansRentals)
            Console.WriteLine($"  {rental}");
        Console.WriteLine();

        // -=- 8. Zwrot w terminie -=-
        PrintStep("8. Zwrot sprzętu w terminie");
        PrintResult(_rentalService.Return(laptop1.Id));

        // -=- 9. Symulacja opóźnionego zwrotu -=-
        PrintStep("9. Zwrot opóźniony (symulacja)");
        SimulateOverdueReturn(employee1, projector1);

        // -=- 10. Przeterminowane wypożyczenia -=-
        PrintStep("10. Lista przeterminowanych wypożyczeń");
        var overdue = _rentalService.GetOverdueRentals();
        if (overdue.Count == 0)
            Console.WriteLine("  Brak przeterminowanych wypożyczeń.\n");
        else
            foreach (var r in overdue)
                Console.WriteLine($"  {r}");
        Console.WriteLine();

        // -=- 11. Raport końcowy -=-
        PrintStep("11. Raport końcowy");
        Console.WriteLine(_reportService.GenerateSummary());
    }

    private void SimulateOverdueReturn(User user, Equipment equipment)
    {
        Console.WriteLine("  (Symulacja: tworzymy wypożyczenie z terminem 5 dni temu)");

        var overdueRental = new Rental(user, equipment, -5);
        equipment.Status = EquipmentStatus.Borrowed;
        var penaltyCalc = new PenaltyCalculator();
        decimal penalty = 5 * 5.00m; // 5 dni * 5 PLN
        overdueRental.CompleteReturn(penalty);
        equipment.Status = EquipmentStatus.Available;

        Console.WriteLine($"  Zwrócono \"{equipment.Name}\". Naliczono karę: {penalty:F2} PLN (5 dni opóźnienia).\n");
    }

    private void PrintEquipmentList(List<Equipment> equipment)
    {
        foreach (var e in equipment)
            Console.WriteLine($"  {e} | {e.GetDetails()}");
        Console.WriteLine();
    }

    private void PrintResult(OperationResult result)
    {
        string prefix = result.Success ? "  ✓" : "  ✗";
        Console.WriteLine($"{prefix} {result.Message}");
    }

    private void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine();
    }

    private void PrintStep(string step)
    {
        Console.WriteLine($"--- {step} ---");
    }
}