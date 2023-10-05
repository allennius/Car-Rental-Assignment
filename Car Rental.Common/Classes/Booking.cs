using Car_Rental.Common.Enums;
using Car_Rental.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Rental.Common.Classes;
public class Booking : IBooking
{
    public int Id { get; init; }
    public IVehicle Vehicle { get; init; }
    public IPerson Customer { get; init; }
    public int KmRented { get; init; }
    public int? KmReturned { get; set; } = default;
    public DateOnly RentedDate { get; init; }
    public DateOnly ReturnDate { get ; set ; } = default;
    public double? Cost { get; set; } = default;


    public Booking(int id, IVehicle vehicle, IPerson customer, int kmRented, DateOnly rentedDate)
    {
        Id = id;
        Vehicle = vehicle;
        Customer = customer;
        KmRented = kmRented;
        RentedDate = rentedDate;

        Vehicle.Book();
    }

    public double GetCost()
    {
        var kmCost = 0.0;
        if (KmReturned is not null) kmCost = (double)(KmReturned - KmRented) * Vehicle.CostKM;
        var daysCost = (ReturnDate.DayNumber - RentedDate.DayNumber) * Vehicle.CostDay;

        return (kmCost + daysCost);
    }

    public void CloseBooking(DateOnly date, int odometer)
    {
        ReturnDate = date;
        KmReturned = odometer;
        Cost = GetCost();
        Vehicle.ReturnVehicle(odometer);
    }
}
