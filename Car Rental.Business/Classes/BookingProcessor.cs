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
    private readonly IData _db;
    public string _error = string.Empty;
    public string _message = string.Empty;
    public bool loading = false;

    public BookingProcessor(IData db)
    {
        _db = db;
    }

    void ClearStrings()
    {
        _error = string.Empty;
        _message = string.Empty;
    }
    public IEnumerable<IPerson> GetCustomers() 
    {
        ClearStrings();
        try
        {
            return _db.Get<IPerson>();
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return new List<IPerson>();
        }
    }
    public IEnumerable<IBooking> GetBookings()
    {
        ClearStrings();
        try
        {
            return _db.Get<IBooking>();
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return new List<IBooking>();
        }
    }

    public IPerson? GetPerson(int ssn)
    {
        ClearStrings();
        try
        {
            var result = _db.GetSingle<IPerson>(p => p.SSN.Equals(ssn));
            if (result == null)
                _error = $"Couldn't find person with SSN: {ssn}";
            return result;
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return null;
        }
    }
    public IEnumerable<IVehicle> GetVehicles(VehicleStatuses status = default)
    {
        ClearStrings();
        try
        {
            if (status.Equals(VehicleStatuses.None))
                return _db.Get<IVehicle>();

            return _db.Get<IVehicle>(v => v.Status.Equals(status));
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return new List<IVehicle>();
        }

    }
    public IVehicle? GetVehicle(int vehicleId) 
    {
        ClearStrings();
        try
        {
            var result = _db.GetSingle<IVehicle>(v => v.Id.Equals(vehicleId));
            if (result == null)
                throw new ArgumentException($"Couldn't find vehicle with id {vehicleId}");
            return result;
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return null;
        }
    }
    public IVehicle? GetVehicle(string regNo)
    {
        ClearStrings();
        try
        {
            var result = _db.GetSingle<IVehicle>(v => v.RegNo.ToUpper().Equals(regNo.ToUpper()));
            if (result == null)
                _error = $"Couldn't find vehicle with regNo {regNo}";
            return result;
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return null;
        }
    }

    public async Task<IBooking>? RentVehicle(int vehicleId, int customerId)
    {
        ClearStrings();
        try
        {
            if(vehicleId < 0  || customerId < 0)
            {
                throw new ArgumentException("Check Input, did you select a customer?");
            }
            
            loading = true;
            await Task.Delay(5000);
            loading = false;
            var booking = _db.RentVehicle(vehicleId, customerId);
            if (booking.Equals(null))
            {
                loading = false;
                throw new ArgumentNullException($"Booking Failed: VehicleId: {vehicleId}, CustomerId: {customerId}");
            }
            _message = "RENTED: " + booking.ToString();

            return booking;
        }
        catch (Exception ex)
        {
            _error = ex.Message;
            return null;
        }
    }

    public IBooking? ReturnVehicle(int vehicleId, int? distance) 
    {
        ClearStrings();
        try
        {
            var newOdo = distance.Equals(null) ? 0 : distance.Value;
            var vehicle = _db.GetSingle<IVehicle>(v => v.Id.Equals(vehicleId));
            if (vehicle == null) throw new ArgumentNullException("Could not find vehicle in database");
            if (vehicle.Odometer > newOdo)
            {
                _error = "Did you drive backwards? Check odometer";
                return null;
            }   

            var booking = _db.ReturnVehicle(vehicleId, newOdo);
            _message = "RETURNED: " + booking.ToString();
            return booking;
        }
        catch ( Exception ex)
        {
            _error = ex.Message;
            return null;
        }
    }

    public void AddVehicle(string make, int odometer, 
        double costKm, double costDay,string regNo, VehicleTypes type, VehicleStatuses status = VehicleStatuses.Available)
    {
        try
        {
            ClearStrings();
            
            if (GetVehicle(regNo) != null)
                throw new Exception("Car already exists in db");

            ClearStrings();

            if (type.Equals(VehicleTypes.Motorcycle))
                _db.Add<IVehicle>(new Motorcycle(_db.NextVehicleId, type, costKm, costDay, make.ToUpper(), regNo.ToUpper(), odometer, status));
            else
                _db.Add<IVehicle>(new Car(_db.NextVehicleId, type, costKm, costDay, make.ToUpper(), regNo.ToUpper(), odometer, status));

            _message = $"Added new: {type}";
        }
        catch (Exception ex)
        {
            _error= ex.Message;
        }
    }

    public void AddCustomer(int? socialSecurityNumber, string firstName, string lastName)
    {
        ClearStrings();
        try
        {
            var ssn = socialSecurityNumber.Equals(null)
               ? throw new ArgumentException("SSN Has to have a value")
               : socialSecurityNumber.Value;
            if (GetPerson(ssn) != null)
                throw new Exception("Person already exist in Db");

            ClearStrings();
            _db.Add<IPerson>(new Customer(_db.NextPersonId, firstName, lastName, ssn));

           _message = "Added new customer";
        }
        catch (Exception ex)
        {

            _error = ex.Message;
        }
    }

    public string[] VehicleStatusNames => _db.VehicleStatusNames;
    public string[] VehicleTypeNames => _db.VehicleTypeNames;
    public VehicleTypes GetVehicleType(string name) => _db.GetVehicleType(name);
}
