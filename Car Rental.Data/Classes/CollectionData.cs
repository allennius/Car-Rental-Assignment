using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace Car_Rental.Data.Classes;
public class CollectionData : IData
{
    readonly List<IVehicle> _vehicles = new List<IVehicle>();
    readonly List<IBooking> _bookings = new List<IBooking>();
    readonly List<IPerson> _persons = new List<IPerson>();

    public int NextVehicleId => _vehicles.Count.Equals(0) ? 1 : _vehicles.Max(v => v.Id) + 1;

    public int NextBookingId => _bookings.Count.Equals(0) ? 1 : _bookings.Max(v => v.Id) + 1;

    public int NextPersonId => _persons.Count.Equals(0) ? 1 : _persons.Max(v => v.Id) + 1;


    public CollectionData() => SeedData();
    private void SeedData()
    {
        _persons.Add(new Customer(NextPersonId, "Toker", "Tok", 99002));
        _persons.Add(new Customer(NextPersonId, "Kloker", "Klok", 99032));
        _persons.Add(new Customer(NextPersonId, "Glader", "Glad", 990321));

        _vehicles.Add(new Car(NextVehicleId, VehicleTypes.Combi, 2, 100, "Volvo", "SBR110", 20000));
        _vehicles.Add(new Car(NextVehicleId, VehicleTypes.Sedan, 1, 75, "Saab", "SSB005", 5000));
        _vehicles.Add(new Car(NextVehicleId, VehicleTypes.Van, 3, 50, "GM", "BIG666", 40000));
        _vehicles.Add(new Car(NextVehicleId, VehicleTypes.Sedan, 0.5, 150, "Tesla", "JMB007", 2000));
        _vehicles.Add(new Car(NextVehicleId, VehicleTypes.Combi, 1.5, 80, "Subaru", "FWD224", 17000));
        _vehicles.Add(new Car(NextVehicleId, VehicleTypes.Sedan, 0.75, 50, "Audi", "BRK900", 1000));

        _vehicles.Add(new Motorcycle(NextVehicleId, VehicleTypes.Motorcycle, 0.75, 50, "Yamaha", "YZF250", 1000));
        _vehicles.Add(new Motorcycle(NextVehicleId, VehicleTypes.Motorcycle, 1, 40, "Husqvarna", "TTC300", 50));

        IVehicle vehicle = _vehicles.Single(v => v.Id.Equals(1));
        IVehicle vehicle2 = _vehicles.Single(v => v.Id.Equals(3));

        _bookings.Add(new Booking(NextBookingId, vehicle, _persons.OfType<IPerson>().Single(p => p.SSN.Equals(99002)),
            vehicle.Odometer, DateOnly.FromDateTime(DateTime.Now)));

        _bookings.Add(new Booking(NextBookingId, vehicle2, _persons.OfType<IPerson>().Single(p => p.SSN.Equals(99032)),
            vehicle2.Odometer, DateOnly.FromDateTime(DateTime.Now)));

        // close one booking
        _bookings.First(b => b.Id.Equals(1)).CloseBooking(DateOnly.FromDateTime(DateTime.Now).AddDays(3), 20500);
    }


    public List<T> GetDataList<T>()
    {
        var field = typeof(CollectionData).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).SingleOrDefault(f => f.FieldType.Equals(typeof(List<T>)));
        if (field == null)
            throw new ArgumentNullException(nameof(field));

        var data = field.GetValue(this);
        
        if (data == null)
            return new List<T>();

        return (List<T>)data;
    }

    public List<T> Get<T>(Expression<Func<T, bool>>? expression = null) where T : class
    {
        try
        {
            var data = GetDataList<T>();

            if (expression == null)
                return data.ToList();

            return data.Where(expression.Compile()).ToList();
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not retrieve List", ex);
        }
    }

    public T? GetSingle<T>(Expression<Func<T, bool>>? expression = null) where T : class, IEntity
    {
        try
        {
            var data = GetDataList<T>();

            if (expression == null) return null;

            var entity = data.SingleOrDefault(expression.Compile());
            if (entity == null)
                throw new NullReferenceException("Could not find entity in db");
            return entity;
        }
        catch (NullReferenceException ex) 
        { 
            ex.Data.Add("Error", ex.Message);
            return null;
        }
        catch (Exception) { return null; }
    }

    public void Add<T>(T item) where T : class, IEntity
    {
        var data = GetDataList<T>();
        data.Add(item);
    }

    public IBooking RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            var vehicle = GetSingle<IVehicle>(v => v.Id.Equals(vehicleId));
            var person = GetSingle<IPerson>(p => p.Id.Equals(customerId));
            if (vehicle.Equals(null) || person.Equals(null))
                throw new ArgumentNullException($"Booking Failed: VehicleId: {vehicleId}, CustomerId: {customerId}");

            var data = GetDataList<IBooking>();

            var booking = new Booking(NextBookingId, vehicle, person, vehicle.Odometer, DateOnly.FromDateTime(DateTime.Now));
            data.Add(booking);
            vehicle.Book();

            return booking;
        }
        catch (Exception ex) { throw new Exception($"Booking Failed", ex); }
    }

    public IBooking ReturnVehicle(int vehicleId, int distance)
    {
        try
        {
            var booking = GetSingle<IBooking>(b => b.Vehicle.Id.Equals(vehicleId) && b.Status.Equals(BookingStatus.Booked));
            if (booking.Equals(null))
                throw new NullReferenceException($"Return Failed: VehicleId{vehicleId}");

            booking.CloseBooking(DateOnly.FromDateTime(DateTime.Now), distance);

            return booking;
        }
        catch { throw; }
    }


    public string[] VehicleStatusNames => Enum.GetNames(typeof(VehicleStatuses));
    public string[] VehicleTypeNames => Enum.GetNames(typeof(VehicleTypes));
    public VehicleTypes GetVehicleType(string name)
    {
        var test = (VehicleTypes) Enum.Parse(typeof(VehicleTypes), name);

        return test;
    }
}
