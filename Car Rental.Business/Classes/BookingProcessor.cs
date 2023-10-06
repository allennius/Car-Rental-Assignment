using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;
public class BookingProcessor
{
    private readonly Data.Interfaces.IData _db;

    public BookingProcessor(Data.Interfaces.IData db)
    {
        _db = db;
    }

    public IEnumerable<IPerson> GetCustomers() 
    {
        return _db.Get<Customer>();
    }
    public IEnumerable<IBooking> GetBookings()
    {
        return _db.Get<IBooking>();
    }
    public IPerson? GetPerson(string ssn) => _db.GetSingle<IPerson>(p => p.SSN.Equals(ssn));
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        if (status.Equals(VehicleStatuses.None))
            return _db.Get<IVehicle>();

        return _db.Get<IVehicle>(v => v.Status.Equals(status));

    }
    public IVehicle? GetVehicle(int vehicleId) 
    {
        try
        {
            var result = _db.GetSingle<IVehicle>(v => v.Id.Equals(vehicleId));
            if (result == null)
                throw new ArgumentException($"Couldn't find vehicle with id {vehicleId}");
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Something went wrong, check Id input", ex);
        }
    }
    public IVehicle? GetVehicle(string regNo)
    {
        try
        {
            var result = _db.GetSingle<IVehicle>(v => v.RegNo.Equals(regNo));
            if (result == null)
                throw new ArgumentException($"Couldn't find vehicle with regNo {regNo}");
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Something went wrong, check regNo input", ex);
        }
    }

    public async Task<IBooking> RentVehicle(int vehicleId, int customerId)
    {
        try
        {
            var result = _db.RentVehicle(vehicleId, customerId);
            if (result.Equals(null))
                throw new ArgumentNullException($"Booking Failed: VehicleId: {vehicleId}, CustomerId: {customerId}");
            await Task.Delay(5000);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Something went wrong when creating new booking", ex);        
        }
    }

    public void AddVehicle(string make, string registrationNumber, int odometer, 
        double costKm, double costDay,string regNo, VehicleStatuses status, VehicleTypes type)
    {
        try
        {
            if (type.Equals(VehicleTypes.Motorcycle))
                _db.Add<Motorcycle>(new(_db.NextVehicleId, type, costKm, costDay, make, regNo, odometer, status));
            else
                _db.Add<Car>(new(_db.NextVehicleId, type, costKm, costDay, make, regNo, odometer, status));
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't Create new vehicle", ex);
        }
    }

    public void AddCustomer(string socialSecurityNumber, string firstName, string lastName)
    {
        try
        {
            if (int.TryParse(socialSecurityNumber, out int ssn))
                _db.Add<Customer>(new(_db.NextPersonId, firstName, lastName, ssn));
            else
                throw new ArgumentException($"Could not add Customer, Check Input");
        }
        catch (Exception ex)
        {

            throw new Exception($"Couldn't create new customer", ex);
        }
    }
}
