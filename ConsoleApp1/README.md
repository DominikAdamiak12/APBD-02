# Wypożyczalnia Sprzętu

Aplikacja konsolowa w C# obsługująca system wypożyczania sprzętu na uczelni.

## Uruchomienie

```bash
dotnet run
```

Wymagany .NET 9.0 SDK.

## Decyzje projektowe

### Podział na warstwy

Projekt dzieli się na trzy warstwy:

- **Models** — czyste klasy domenowe, bez logiki biznesowej i bez zależności od konsoli. Opisują *co istnieje* w systemie.
- **Services** — logika biznesowa. Odpowiadają na pytania *co można zrobić* i *jakie reguły obowiązują*.
- **UI** — prezentacja. Jedyny obszar, który wie o `Console`. Odpowiada za *jak pokazać* wyniki użytkownikowi.

### Kohezja

Każda klasa ma jedną wyraźną odpowiedzialność:

- `PenaltyCalculator` — wie tylko jak liczyć kary. Reguła naliczania jest w jednym miejscu.
- `RentalService` — zarządza procesem wypożyczania i zwrotów, sprawdza limity.
- `EquipmentService` — zarządza kolekcją sprzętu i zmianami statusu.
- `ReportService` — generuje raporty, korzystając z danych z innych serwisów.

Klasy modelu (`Equipment`, `User`, `Rental`) przechowują dane i proste metody związane z ich stanem (np. `IsOverdue`), ale nie wykonują operacji biznesowych.

### Coupling (sprzężenie)

- Serwisy nie zależą od siebie nawzajem, z wyjątkiem `ReportService`, który potrzebuje danych z kilku źródeł — to uzasadniona zależność.
- `RentalService` otrzymuje `PenaltyCalculator` przez konstruktor (dependency injection ręczne), co pozwala łatwo podmienić sposób naliczania kar.
- Warstwa UI zależy od serwisów, ale serwisy nie wiedzą o istnieniu UI.
- Modele nie zależą od niczego.

### Dziedziczenie

Użyte tam, gdzie wynika z domeny:

- `Equipment` → `Laptop`, `Projector`, `Camera` — każdy typ ma wspólne cechy (id, nazwa, status) i specyficzne pola. Metoda `GetDetails()` jest abstrakcyjna, bo każdy typ opisuje się inaczej.
- `User` → `Student`, `Employee` — kluczowa różnica to `MaxActiveRentals`. Zamiast if/switch na typie, polimorfizm pozwala serwisowi pytać `user.MaxActiveRentals` bez wiedzy o konkretnym typie.

### Obsługa błędów

Zamiast wyjątków do sterowania logiką, operacje zwracają `OperationResult` z flagą `Success` i komunikatem. Dzięki temu:
- Wywołujący nie musi łapać wyjątków
- Komunikat błędu jest czytelny i opisowy
- Łatwo wyświetlić użytkownikowi co poszło nie tak

### Reguły biznesowe

- Limity wypożyczeń (2 dla studenta, 5 dla pracownika) są zdefiniowane w klasach `Student`/`Employee` jako `MaxActiveRentals` — jedno miejsce zmiany.
- Stawka kary (5 PLN/dzień) jest w `PenaltyCalculator` — jedno miejsce zmiany.
- Reguły walidacji (czy sprzęt dostępny, czy limit nieprzekroczony) są w `RentalService`.
