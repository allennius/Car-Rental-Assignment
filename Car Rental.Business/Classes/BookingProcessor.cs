using Car_Rental.Common.Classes;
using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using Car_Rental.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Business.Classes;
public class BookingProcessor
{
    private readonly IData _db;

    public BookingProcessor(IData db)
    {
        _db = db;
    }

    public IEnumerable<IPerson> GetCustomers() 
    {
        return _db.GetPersons();
    }
    public IEnumerable<IVehicle> GetVehicles() 
    {
        return _db.GetVehicles(VehicleStatuses.None);
    }
    public IEnumerable<IBooking> GetBookings()
    {
        return _db.GetBookings();
    }
}
