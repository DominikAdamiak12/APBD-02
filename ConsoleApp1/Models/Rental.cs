namespace ConsoleApp1.Models;

public class Rental
{
    private static int _nextId = 1;

    public int Id { get; }
    public User User { get; }
    public Equipment Equipment { get; }
    public DateTime RentalDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }
    public decimal Penalty { get; private set; }

    public bool IsActive => ReturnDate == null;
    public bool IsOverdue => IsActive && DateTime.Now > DueDate;

    public Rental(User user, Equipment equipment, int rentalDays)
    {
        Id = _nextId++;
        User = user;
        Equipment = equipment;
        RentalDate = DateTime.Now;
        DueDate = RentalDate.AddDays(rentalDays);
        ReturnDate = null;
        Penalty = 0;
    }

    public void CompleteReturn(decimal penalty)
    {
        ReturnDate = DateTime.Now;
        Penalty = penalty;
    }

    public override string ToString()
    {
        string status = IsActive ? (IsOverdue ? "PRZETERMINOWANE" : "Aktywne") : "Zakończone";
        string penaltyInfo = Penalty > 0 ? $", Kara: {Penalty:C}" : "";
        return $"[{Id}] {User.FirstName} {User.LastName} -> {Equipment.Name} " +
               $"({RentalDate:dd.MM.yyyy} - {DueDate:dd.MM.yyyy}) [{status}]{penaltyInfo}";
    }
}