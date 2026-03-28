namespace ConsoleApp1.Services;

using ConsoleApp1.Models;

public class PenaltyCalculator
{
    private const decimal PenaltyPerDay = 5.00m;

    public decimal Calculate(Rental rental)
    {
        if (rental.DueDate >= DateTime.Now)
            return 0;

        int overdueDays = (DateTime.Now - rental.DueDate).Days;
        return overdueDays * PenaltyPerDay;
    }
}