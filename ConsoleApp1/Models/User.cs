namespace ConsoleApp1.Models;

public abstract class User
{
    private static int _nextId = 1;

    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public abstract int MaxActiveRentals { get; }
    public abstract string UserType { get; }

    protected User(string firstName, string lastName)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
    {
        return $"[{Id}] {FirstName} {LastName} ({UserType}, limit: {MaxActiveRentals})";
    }
}

public class Student : User
{
    public override int MaxActiveRentals => 2;
    public override string UserType => "Student";

    public Student(string firstName, string lastName) : base(firstName, lastName) { }
}

public class Employee : User
{
    public override int MaxActiveRentals => 5;
    public override string UserType => "Pracownik";

    public Employee(string firstName, string lastName) : base(firstName, lastName) { }
}
