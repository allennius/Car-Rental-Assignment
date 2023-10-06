using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;

namespace Car_Rental.Data.Classes;
public class CollectionData : IData
{
    public int NextVehicleId
    {
        get 
        {
            if (!_data.OfType<IVehicle>().Any())
                return 0;
            var id = _data.OfType<IVehicle>().Max(x => x.Id) + 1;
            if (id.Equals(null))
                return 0;
            return id;
        }
    }        
    public int NextBookingId
    {
        get 
        {
            if (!_data.OfType<IBooking>().Any())
                return 0;
            var id = _data.OfType<IBooking>().Max(x => x.Id) + 1;
            if (id.Equals(null))
                return 0;
            return id;
        }
    } 
    public int NextPersonId
    {
        get 
        {
            if (!_data.Any(x => x.GetType().Equals(typeof(IPerson))))
                return 0;
            var id = _data.OfType<IPerson>().Max(x => x.Id) + 1;
            if (id.Equals(null))
                return 0;
            return id;
        }
    } 

    readonly List<IPerson> _persons = new List<IPerson>();
    readonly List<IBooking> _bookings = new List<IBooking>();
    readonly List<IVehicle> _vehicles = new List<IVehicle>();

    readonly List<IEntity> _data = new List<IEntity>();
    readonly List<IEntity> _car = new List<IEntity>();

    public CollectionData() => SeedData();
    private void SeedData()
    {
        _data.Add(new Customer(NextPersonId, "Toker", "Tok", 99002));
        _data.Add(new Customer(NextPersonId, "Kloker", "Klok", 99032));
        _data.Add(new Customer(NextPersonId, "Glader", "Glad", 990321));

        _data.Add(new Car(NextVehicleId, VehicleTypes.Combi, 2, 100, "Volvo", "SBR110", 20000));
        _data.Add(new Car(NextVehicleId, VehicleTypes.Sedan, 1, 75, "Saab", "SSB005", 5000));
        _data.Add(new Car(NextVehicleId, VehicleTypes.Van, 3, 50, "GM", "BIG666", 40000));
        _data.Add(new Car(NextVehicleId, VehicleTypes.Sedan, 0.5, 150, "Tesla", "JMB007", 2000));
        _data.Add(new Car(NextVehicleId, VehicleTypes.Combi, 1.5, 80, "Subaru", "FWD224", 17000));
        _data.Add(new Car(NextVehicleId, VehicleTypes.Sedan, 0.75, 50, "Audi", "BRK900", 1000));

        _data.Add(new Motorcycle(NextVehicleId, VehicleTypes.Motorcycle, 0.75, 50, "Yamaha", "YZF250", 1000));
        _data.Add(new Motorcycle(NextVehicleId, VehicleTypes.Motorcycle, 1, 40, "Husqvarna", "TTC300", 50));

        IVehicle vehicle = _data.OfType<IVehicle>().Single((v => v.Id.Equals(0)));
        IVehicle vehicle2 = _data.OfType<IVehicle>().Single(v => v.Id.Equals(3));

        _data.Add(new Booking(NextBookingId, vehicle, _data.OfType<IPerson>().Single(p => p.SSN.Equals(99002)),
            vehicle.Odometer, DateOnly.FromDateTime(DateTime.Now)));
        
        _data.Add(new Booking(NextBookingId,vehicle2, _data.OfType<IPerson>().Single(p => p.SSN.Equals(99032)), 
            vehicle2.Odometer, DateOnly.FromDateTime(DateTime.Now)));

        // close one booking
        _data.OfType<IBooking>().Single(b => b.Id.Equals(1)).CloseBooking(DateOnly.FromDateTime(DateTime.Today.AddDays(3)), 20500);

        //_data.AddRange(_vehicles);
        //_data.AddRange(_persons);
        //_data.AddRange(_bookings);
        var car = Get<Car>(c => c.VehicleType.Equals(VehicleTypes.Sedan));
        foreach (var c in car)
        {
            _car.Add(c);
        }

        GetSingle<IPerson>(p => p.SSN.Equals("99002"));
        var single = GetSingle<IEntity>(s => s.Id.Equals(1));
        var single2 = GetSingle<IEntity>();
    }

    public List<T> Get<T>(Expression<Func<T, bool>>? expression = null) where T : class
    {
        try
        {
            if (expression == null) 
                return _data.OfType<T>().ToList();
        
            return _data.OfType<T>().Where(expression.Compile()).ToList();
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Could not retrieve List", ex);
        }

        //return _data.Where(x => x.GetType().Equals(typeof(T)));
    }
    public T? GetSingle<T>(Expression<Func<T, bool>>? expression = null) where T : class, IEntity
    {
        try
        {
            if (expression == null) return null;

            return _data.OfType<T>().FirstOrDefault(expression.Compile());
            //return (T)_data.FirstOrDefault(x => x.GetType().Equals(typeof(T)) && x.Id.Equals(id));
        }
        catch { return null; }
    }

    public void Add<T>(T item) where T : class, IEntity
    {
        _data.Add(item);
    }

    public IBooking RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            var vehicle = GetSingle<IVehicle>(v => v.Id.Equals(vehicleId));
            var person = GetSingle<IPerson>(p => p.Id.Equals(customerId));
            if (vehicle.Equals(null) || person.Equals(null))
                throw new ArgumentNullException($"Booking Failed: VehicleId: {vehicleId}, CustomerId: {customerId}");

            _data.Add(new Booking(NextBookingId, vehicle, person, vehicle.Odometer, DateOnly.FromDateTime(DateTime.Now)));
            vehicle.Book();

            return _data.OfType<IBooking>().Last();
        }
        catch { throw; }
    }

    public IBooking ReturnVehicle(int vehicleId)
    {
        try
        {
            var booking = GetSingle<IBooking>(b => b.Vehicle.Id.Equals(vehicleId));
            if (booking.Equals(null))
                throw new NullReferenceException($"Return Failed: VehicleId{vehicleId}");

            booking.CloseBooking(DateOnly.FromDateTime(DateTime.Now), GetSingle<IVehicle>(v => v.Id.Equals(vehicleId)).Odometer);

            return booking;
        }
        catch { throw; }
    }
}
