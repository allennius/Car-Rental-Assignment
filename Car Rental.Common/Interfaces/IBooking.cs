

using Car_Rental.Common.Enums;

namespace Car_Rental.Common.Interfaces;
public interface IBooking
{
    public int Id { get; init; }
    public IVehicle Vehicle { get; init; }
    public IPerson Customer { get; init; }
    public int KmRented { get; init; }
    public int? KmReturned { get; set; }
    public DateOnly RentedDate { get; init; }
    public DateOnly ReturnDate { get; set; }
    public double? Cost { get; set; }

    public double GetCost();
    public void CloseBooking(DateOnly date, int odometer);
}
