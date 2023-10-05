using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;

namespace Car_Rental.Data.Classes;
public class CollectionData : IData
{
    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IBooking> _bookings = new List<IBooking>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();

    public CollectionData() => SeedData();
    private void SeedData()
    {
        _persons.Add(new Customer("Toker", "Tok", 99002));
        _persons.Add(new Customer("Kloker", "Klok", 99032));
        _persons.Add(new Customer("Glader", "Glad", 990321));

        _vehicles.Add(new Car(VehicleTypes.Combi, 2, 100, "Volvo", "SBR110", 20000));
        _vehicles.Add(new Car(VehicleTypes.Sedan, 1, 75, "Saab", "SSB005", 5000));
        _vehicles.Add(new Car(VehicleTypes.Van, 3, 50, "GM", "BIG666", 40000));
        _vehicles.Add(new Car(VehicleTypes.Sedan, 0.5, 150, "Tesla", "JMB007", 2000));
        _vehicles.Add(new Car(VehicleTypes.Combi, 1.5, 80, "Subaru", "FWD224", 17000));
        _vehicles.Add(new Car(VehicleTypes.Sedan, 0.75, 50, "Audi", "BRK900", 1000));

        _vehicles.Add(new Motorcycle(VehicleTypes.Motorcycle, 0.75, 50, "Yamaha", "YZF250", 1000));
        _vehicles.Add(new Motorcycle(VehicleTypes.Motorcycle, 1, 40, "Husqvarna", "TTC300", 50));

        var vehicle = _vehicles.Single(v => v.RegNo.Equals("SBR110"));
        var vehicle2 = _vehicles.Single(v => v.RegNo.Equals("YZF250"));

        _bookings.Add(new Booking(_bookings.Count() != 0 ? _bookings.Max(b => b.Id) + 1 : 1 ,vehicle, _persons.Single(p => p.SSN.Equals(99002)),
            vehicle.Odometer, DateOnly.FromDateTime(DateTime.Now)));
        
        _bookings.Add(new Booking(_bookings.Count() != 0 ? _bookings.Max(b => b.Id) + 1 : 1 ,vehicle2, _persons.Single(p => p.SSN.Equals(99032)), 
            vehicle2.Odometer, DateOnly.FromDateTime(DateTime.Now)));

        // close one booking
        _bookings.Single(b => b.Id.Equals(1)).CloseBooking(DateOnly.FromDateTime(DateTime.Today.AddDays(3)), 20500);
    }

    public IEnumerable<IBooking> GetBookings() => _bookings;

    public IEnumerable<IPerson> GetPersons() => _persons;

    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if (status == VehicleStatuses.None) return _vehicles;

        return _vehicles.Where(v => v.Status.Equals(status));
    }

}
