using ConsoleApp1.Services;

// Kompozycja zależności
var penaltyCalculator = new PenaltyCalculator();
var equipmentService = new EquipmentService();
var userService = new UserService();
var rentalService = new RentalService(penaltyCalculator);
var reportService = new ReportService(equipmentService, rentalService, userService);